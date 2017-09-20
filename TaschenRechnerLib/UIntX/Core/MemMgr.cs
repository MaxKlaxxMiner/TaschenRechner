using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib.UIntX.Core
{
  /// <summary>
  /// Klasse zum verwalten von Arbeitsspeicher
  /// </summary>
  public static unsafe class MemMgr
  {
    /// <summary>
    /// Mindestgröße eines Elementes (2er Potenz, sinnvolle Werte: 8-256, default: 32)
    /// </summary>
    const int MinElementSize = 32;

    /// <summary>
    /// Mindestgröße eines initialen Blockes (2er Potenz, sinnvolle Werte: 1024-65536, default: 4096)
    /// </summary>
    const int MinBlockBytes = 4096;

    /// <summary>
    /// minimale Anzahl der Elemente pro Block (sinnvolle Werte: 2-10, default: 5)
    /// </summary>
    const int MinElementsCount = 5;

    /// <summary>
    /// maximale Anzahl der Blöcke pro Level (sinnvolle Werte: 8-24, default: 16)
    /// </summary>
    const int MaxBlocksPerLevel = 16;

    /// <summary>
    /// maximale Anzahl der möglichen Levels (sibbvolle Werte: 16-32, default: 26)
    /// </summary>
    const int MaxLevels = 26;

    #region # class MemBlock // Klasse zur Handhabung mehrere kleineren Speicherbereiche
    /// <summary>
    /// Klasse zur Handhabung mehrere kleineren Speicherbereiche
    /// </summary>
    sealed class MemBlock
    {
      #region # // --- Werte ---
      /// <summary>
      /// merkt sich den Zeiger auf den Anfangsbereich des reservierten Arbeitsspeichers
      /// </summary>
      readonly byte* pointer;
      /// <summary>
      /// merkt sich merkt sich die Länge des reservierten Speichers in Bytes
      /// </summary>
      readonly long totalBytes;
      /// <summary>
      /// merkt sich den Zeiger auf die eigentlichen Daten (zwischen pointer und data befindet sich die FreeBitmap)
      /// </summary>
      readonly byte* data;
      /// <summary>
      /// Größe der Elemente in Bytes
      /// </summary>
      readonly int elementSize;
      /// <summary>
      /// Bitgröße der Elemente (0 = 1, 1 = 2, 2 = 4, 3 = 8, 4 = 16 usw.)
      /// </summary>
      readonly int elementBits;
      /// <summary>
      /// Maximale Anzahl der Elemente
      /// </summary>
      readonly int elementsMax;
      /// <summary>
      /// Anzahl der freien Elemente
      /// </summary>
      int elementsFree;
      /// <summary>
      /// nächste Suchposition in der Bitmap zum finden des nächsten freien Platzes
      /// </summary>
      byte* freeSearch;
      #endregion

      #region # // --- Properties ---
      /// <summary>
      /// gibt den freien Speicherplatz in Bytes an
      /// </summary>
      public long FreeBytes { get { return (long)elementsFree << elementBits; } }

      /// <summary>
      /// gibt die Größe des gesamten reservierten Speichers ingesamt an
      /// </summary>
      public long TotalBytes { get { return totalBytes; } }

      /// <summary>
      /// gibt die Größe des reinen Daten-Speichers an
      /// </summary>
      public long DataBytes { get { return totalBytes + pointer - data; } }

      /// <summary>
      /// gibt die maximale Anzahl der speicherbaren Elemente an
      /// </summary>
      public int MaxElements { get { return elementsMax; } }

      /// <summary>
      /// gibt die Größe der Elemente zurück
      /// </summary>
      public int ElementSize { get { return elementSize; } }

      /// <summary>
      /// gibt die Speicheradresse der Daten zurück
      /// </summary>
      public byte* DataPointer { get { return data; } }

      /// <summary>
      /// zeigt auf das Ende des Speicherbereiches
      /// </summary>
      public byte* DataPointerEnd { get { return pointer + totalBytes; } }
      #endregion

      #region # // --- Methoden ---
      /// <summary>
      /// reserviert ein neues Element und gibt den entsprechenden Zeiger zurück (Größe des benutzbaren Speichers siehe <see cref="ElementSize"/>
      /// </summary>
      /// <returns>Zeiger auf die entsprechende Speicherposition</returns>
      public byte* AllocElement()
      {
        if (elementsFree == 0) throw new OutOfMemoryException(); // keine freien Plätze vorhanden

        // --- freies Element in der Bitmap suchen ---
        while (freeSearch < data && *freeSearch == 0xff) freeSearch++;
        if (freeSearch == data)
        {
          freeSearch = pointer;
          while (freeSearch < data && *freeSearch == 0xff) freeSearch++;
          if (freeSearch == data) throw new OutOfMemoryException(); // keinen freien Platz gefunden?!? -> sollte nicht auftreten
        }

        // --- freies Bit Suchen und absolute Speicherposition berechnen ---
        byte b = *freeSearch;
        for (int bit = 0; bit < 8; bit++)
        {
          if ((b & 1 << bit) == 0) // freies Bit gefunden
          {
            *freeSearch = (byte)(b | 1 << bit); // Bit als belegt markieren

            Debug.Assert((*freeSearch & 1 << bit) != 0);

            elementsFree--;
            long elementOffset = (freeSearch - pointer << 3) + bit;
            var result = data + (elementOffset << elementBits);

            Debug.Assert(result >= data);
            Debug.Assert(result + elementSize <= DataPointerEnd);

            return result;
          }
        }

        throw new OutOfMemoryException(); // kein freies Bit gefunden?!? -> sollte nicht auftreten
      }

      /// <summary>
      /// prüft, ob die Speicheradresse zu einem benutzbaren Speicherbereich gehört
      /// </summary>
      /// <param name="p">Zeiger auf die absolute Speicheradresse</param>
      /// <returns>true, wenn die Speicheradresse gültig ist</returns>
      public bool ValidPtr(byte* p)
      {
        Debug.Assert(p >= DataPointer);
        Debug.Assert(p + elementSize <= DataPointerEnd);

        long elementPos = p - data >> elementBits;
        Debug.Assert(elementPos >= 0 && elementPos < elementsMax);

        if (data + (elementPos << elementBits) != p) return false; // ungültige Adresse innerhalb eines Elementes

        return (pointer[elementPos >> 3] & 1 << (int)(elementPos & 7)) != 0; // ist das entsprechende Reserviert-Bit gesetzt?
      }

      /// <summary>
      /// löscht die Reservierung eines bestimmten Elementes und gibt true, wenn erfolgreich
      /// </summary>
      /// <param name="p">Zeiger auf die absolute Speicheradresse des Elementes</param>
      /// <returns>true, wenn die Freigabe erfolgreich war</returns>
      public bool FreeElement(byte* p)
      {
        Debug.Assert(p >= DataPointer);
        Debug.Assert(p + elementSize <= DataPointerEnd);

        long elementPos = p - data >> elementBits;
        Debug.Assert(elementPos >= 0 && elementPos < elementsMax);

        if (data + (elementPos << elementBits) != p) return false; // ungültige Adresse innerhalb eines Elementes

        int bits = pointer[elementPos >> 3];
        int marker = bits & 1 << (int)(elementPos & 7);
        if (marker == 0) return false; // Element war bereits gelöscht

        pointer[elementPos >> 3] = (byte)(bits ^ marker); // Bit wieder löschen
        elementsFree++;
        return true;
      }
      #endregion

      #region # // --- Konstruktor ---
      /// <summary>
      /// Konstruktor
      /// </summary>
      /// <param name="elementSize">Größe der Elemente in Bytes (muss einer zweipotenz entsprechen, z.B. 1,2,4,8,16,32... usw. max: 1073741824)</param>
      /// <param name="elementCount">Anzahl der Elemente</param>
      public MemBlock(int elementSize, int elementCount)
      {
        switch (elementSize)
        {
          case 1 << 0: elementBits = 0; break;
          case 1 << 1: elementBits = 1; break;
          case 1 << 2: elementBits = 2; break;
          case 1 << 3: elementBits = 3; break;
          case 1 << 4: elementBits = 4; break;
          case 1 << 5: elementBits = 5; break;
          case 1 << 6: elementBits = 6; break;
          case 1 << 7: elementBits = 7; break;
          case 1 << 8: elementBits = 8; break;
          case 1 << 9: elementBits = 9; break;
          case 1 << 10: elementBits = 10; break;
          case 1 << 11: elementBits = 11; break;
          case 1 << 12: elementBits = 12; break;
          case 1 << 13: elementBits = 13; break;
          case 1 << 14: elementBits = 14; break;
          case 1 << 15: elementBits = 15; break;
          case 1 << 16: elementBits = 16; break;
          case 1 << 17: elementBits = 17; break;
          case 1 << 18: elementBits = 18; break;
          case 1 << 19: elementBits = 19; break;
          case 1 << 20: elementBits = 20; break;
          case 1 << 21: elementBits = 21; break;
          case 1 << 22: elementBits = 22; break;
          case 1 << 23: elementBits = 23; break;
          case 1 << 24: elementBits = 24; break;
          case 1 << 25: elementBits = 25; break;
          case 1 << 26: elementBits = 26; break;
          case 1 << 27: elementBits = 27; break;
          case 1 << 28: elementBits = 28; break;
          case 1 << 29: elementBits = 29; break;
          case 1 << 30: elementBits = 30; break;
          default: throw new ArgumentException("elementSize");
        }
        if (elementCount < 1 || elementCount > 1000000000) throw new ArgumentException("elementCount");

        this.elementSize = elementSize;
        totalBytes = (elementCount + 7) / 8 + (long)elementSize * elementCount;
        pointer = (byte*)Marshal.AllocHGlobal((IntPtr)totalBytes);
        Zero(pointer, (elementCount + 7) / 8); // Bitmap leeren
        if ((elementCount & 7) != 0)
        {
          for (int nx = elementCount; nx < elementCount + 8; nx++) // restlichen Bits als belegt markieren (welche außerhalb des gültigen Bereiches liegen)
          {
            pointer[nx >> 3] = (byte)(pointer[nx >> 3] | 1 << (nx & 7));
          }
        }
        data = pointer + (elementCount + 7) / 8; // Zeiger auf den Datenbereich setzen (direkt nach der Bitmap)
        freeSearch = pointer;
        elementsMax = elementCount;
        elementsFree = elementCount;
      }

      /// <summary>
      /// Destructor zum freigeben des Speichers
      /// </summary>
      ~MemBlock()
      {
        Marshal.FreeHGlobal((IntPtr)pointer);
      }

      /// <summary>
      /// gibt den Inhalt als lesbare Zeichenkette zurück
      /// </summary>
      /// <returns>lesbare Zeichenkette</returns>
      public override string ToString()
      {
        return (new { size = totalBytes, elementSize, elementsFree }).ToString();
      }
      #endregion
    }
    #endregion

    #region # // --- statische Variablen ---
    /// <summary>
    /// merkt sich alle erstellten Memory-Blöcke
    /// </summary>
    static readonly MemBlock[] MemBlocks = new MemBlock[MaxBlocksPerLevel * MaxLevels];

    /// <summary>
    /// merkt sich die Memory-Blöcke sortiert nach Speicher-Adressen
    /// </summary>
    static readonly KeyValuePair<long, MemBlock>[] MemBlocksPointer = new KeyValuePair<long, MemBlock>[MemBlocks.Length];

    /// <summary>
    /// Anzahl der bereits erstellten Memory-Blöcke
    /// </summary>
    static int memBlocksCount;
    #endregion

    #region # // --- private Methoden ---
    /// <summary>
    /// gibt einen MemoryBlock zurück, welcher genug freien Speicher besitzt
    /// </summary>
    /// <param name="bytes">Größe des Speicherbereiches, welcher reserviert werden soll</param>
    /// <returns>Memoryblock mit genügend freien Speicherplatz</returns>
    static MemBlock GetMemBlockFree(int bytes)
    {
      // --- echte Größe ermitteln und passenden Index Suchen ---
      int targetSize = MinElementSize;
      int targetIndex = 0;
      while (targetSize < bytes)
      {
        targetIndex += MaxBlocksPerLevel;
        targetSize <<= 1;
      }

      // --- bekannte Blöcke mit zu wenig freien Platz überspringen ---
      while (MemBlocks[targetIndex] != null && MemBlocks[targetIndex].FreeBytes < targetSize) targetIndex++;

      if (MemBlocks[targetIndex] == null) // neuen Block erstellen?
      {
        int targetFirst = targetIndex / MaxBlocksPerLevel * MaxBlocksPerLevel;
        int newCount = MemBlocks[targetFirst] != null ? MemBlocks[targetFirst].MaxElements * 2 : Math.Max(MinElementsCount, MinBlockBytes / targetSize);
        while (targetIndex > targetFirst)
        {
          MemBlocks[targetIndex] = MemBlocks[targetIndex - 1];
          targetIndex--;
        }
        MemBlocks[targetIndex] = new MemBlock(targetSize, newCount);

        // --- neuen Block in die Pointer-Liste einsortieren ---
        var p = new KeyValuePair<long, MemBlock>((long)MemBlocks[targetIndex].DataPointer, MemBlocks[targetIndex]);
        int pIdx = memBlocksCount;
        while (pIdx > 0 && MemBlocksPointer[pIdx - 1].Key > p.Key)
        {
          MemBlocksPointer[pIdx] = MemBlocksPointer[pIdx - 1];
          pIdx--;
        }
        MemBlocksPointer[pIdx] = p;
        memBlocksCount++;
      }

      return MemBlocks[targetIndex];
    }

    /// <summary>
    /// gibt den zugehörigen Memory-Block anhand eines absolute Speicher-Zeigers zurück (oder null, wenn nicht gefunden)
    /// </summary>
    /// <param name="p">Speicher-Adresse, welche gesucht werden soll</param>
    /// <returns>gefundener Memory-Block oder null, wenn nicht gefunden</returns>
    static MemBlock GetMemBlockPtr(byte* p)
    {
      int startPos = 0;
      int endPos = memBlocksCount;
      long searchPointer = (long)p;
      do
      {
        int midPos = (startPos + endPos) >> 1;
        if (MemBlocksPointer[midPos].Key > searchPointer) endPos = midPos; else startPos = midPos;
      } while (endPos - startPos > 1);

      if (MemBlocksPointer[startPos].Key <= searchPointer && MemBlocksPointer[startPos].Value != null && searchPointer < (long)MemBlocksPointer[startPos].Value.DataPointerEnd)
      {
        return MemBlocksPointer[startPos].Value;
      }

      return null; // Memoryblock nicht gefunden
    }
    #endregion

    /// <summary>
    /// reserviert einen neuen Speicherbereich (Inhalt ist undefiniert)
    /// </summary>
    /// <param name="bytes">Größe in Bytes, welche reserviert werden soll</param>
    /// <returns>Zeiger auf den Speicherbereich</returns>
    public static byte* AllocUnsafe(int bytes)
    {
      if (bytes < 0 || bytes > 1024 * 1024 * 1024) throw new OutOfMemoryException();

      var block = GetMemBlockFree(bytes);

      Debug.Assert(block.FreeBytes >= bytes);
      Debug.Assert(block.ElementSize >= bytes);

      return block.AllocElement();
    }

    /// <summary>
    /// reserviert einen neuen Speicherbereich (gefüllt mit Nullen)
    /// </summary>
    /// <param name="bytes">Größe in Bytes, welche reserviert werden soll</param>
    /// <returns>Zeiger auf den Speicherbereich</returns>
    public static byte* Alloc(int bytes)
    {
      var p = AllocUnsafe(bytes);
      Zero(p, bytes);
      return p;
    }

    /// <summary>
    /// gibt die maximal erlaubte Größe des reservierte Speicherbereiches zurück
    /// (gibt 0 zurück, wenn der Bereich bereits mit <see cref="Free"/> gelöscht wurde oder nicht vom Speichermanager <see cref="MemMgr"/> verwaltet wird)
    /// </summary>
    /// <param name="p">Zeiger auf den reservierten Speicherbereich</param>
    /// <returns>Größe des nutzbaren Speicherbereiches in Bytes, oder 0 wenn ungültig</returns>
    public static int GetSize(byte* p)
    {
      var block = GetMemBlockPtr(p);
      if (block == null) return 0; // passenden Memory-Block nicht gefunden (wird nicht von MemMgr verwaltet)

      if (!block.ValidPtr(p)) return 0; // Speicheradresse ungültig oder Bereich wurde bereits freigegeben

      return block.ElementSize;
    }

    /// <summary>
    /// gibt einen reservierten Speicherbereich wieder frei
    /// </summary>
    /// <param name="p">Zeiger auf den resevierten Speicherbereich</param>
    /// <returns>true, wenn die Freigabe des Speichers erfolgreich war</returns>
    public static bool Free(byte* p)
    {
      var block = GetMemBlockPtr(p);
      if (block == null) return false; // passenden Memory-Block nicht gefunden (wird nicht von MemMgr verwaltet)

      return block.FreeElement(p);
    }

    /// <summary>
    /// gibt in Bytes an, wieviel Arbeitsspeicher insgesamt vom Manager reserviert wurde
    /// </summary>
    public static long AllocatedTotal
    {
      get
      {
        long total = 0;

        foreach (var b in MemBlocks)
        {
          if (b == null) continue;
          total += b.TotalBytes;
        }

        return total;
      }
    }

    /// <summary>
    /// gibt in Bytes an, wieviel Arbeitsspeicher momentan in Benutzung ist (inkl. BitMaps)
    /// </summary>
    public static long UsedTotal
    {
      get
      {
        long total = 0;

        foreach (var b in MemBlocks)
        {
          if (b == null) continue;
          total += b.TotalBytes - b.FreeBytes;
        }

        return total;
      }
    }

    /// <summary>
    /// gibt in Bytes an, wieviel Arbeitsspeicher momentan von reinen Daten in Benutzung ist
    /// </summary>
    public static long UsedData
    {
      get
      {
        long total = 0;

        foreach (var b in MemBlocks)
        {
          if (b == null) continue;
          total += b.DataBytes - b.FreeBytes;
        }

        return total;
      }
    }

    /// <summary>
    /// füllt einen bestimmten Speicherbereich mit Nullen
    /// </summary>
    /// <param name="p">Zeiger auf den Speicherbereich</param>
    /// <param name="bytes">Anzahl der Bytes, welche befüllt werden sollen</param>
    public static void Zero(byte* p, int bytes)
    {
      while (bytes >= sizeof(ulong)) // 8-Byte Schritte
      {
        *(ulong*)p = 0;
        p += sizeof(ulong);
        bytes -= sizeof(ulong);
      }

      while (bytes != 0) // restliche Bytes einzeln setzen
      {
        *p++ = 0;
        bytes--;
      }
    }
  }
}
