using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TaschenRechnerLib.UIntX.Core
{
  /// <summary>
  /// Klasse zum verwalten von Arbeitsspeicher
  /// </summary>
  internal unsafe static class MemMgr
  {
    /// <summary>
    /// Mindestgröße eines Elementes
    /// </summary>
    const int MinElementSize = 32;

    #region # class MemBlock // Klasse zur Handhabung mehrere kleineren Speicherbereiche
    /// <summary>
    /// Klasse zur Handhabung mehrere kleineren Speicherbereiche
    /// </summary>
    class MemBlock
    {
      #region # // --- Werte ---
      /// <summary>
      /// merkt sich den Zeiger auf den Anfangsbereich des reservierten Arbeitsspeichers
      /// </summary>
      readonly byte* pointer;
      /// <summary>
      /// merkt sich merkt sich die Länge des reservierten Speichers in Bytes
      /// </summary>
      readonly long bytes;
      /// <summary>
      /// merkt sich den Zeiger auf die eigentlichen Daten (zwischen pointer und data befindet sich die FreeBitmap)
      /// </summary>
      readonly byte* data;
      /// <summary>
      /// Anzahl der freien Elemente
      /// </summary>
      readonly long elementsFree;
      /// <summary>
      /// Größe der Element in Bytes
      /// </summary>
      readonly int elementSize;
      /// <summary>
      /// Bitgröße der Elemente (0 = 1, 1 = 2, 2 = 4, 3 = 8, 4 = 16 usw.)
      /// </summary>
      readonly int elementBits;
      /// <summary>
      /// nächste Suchposition in der Bitmap zum finden des nächsten freien Platzes
      /// </summary>
      byte* freeSearch;
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
        elementCount = (elementCount + 63) / 64 * 64;
        bytes = elementCount / 8 + (long)elementSize * elementCount;
        pointer = (byte*)Marshal.AllocHGlobal((IntPtr)bytes);
        data = pointer + elementCount / 8;
        freeSearch = pointer;
        elementsFree = elementCount;
      }
      #endregion

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
        return (new { size = bytes, elementSize, elementsFree }).ToString();
      }
    }
    #endregion

    /// <summary>
    /// reserviert einen neuen Speicherbereich (der Inhalt ist undefiniert)
    /// </summary>
    /// <param name="bytes">Mindestgröße in Bytes, welche reserviert werden sollen</param>
    /// <returns>Zeiger auf den Speicherbereich</returns>
    public static byte* AllocUnsafe(int bytes)
    {
      return null;
    }

    /// <summary>
    /// gibt einen reservierten Speicherbereich wieder frei
    /// </summary>
    /// <param name="p">Zeiger auf den resevierten Speicherbereich</param>
    /// <returns></returns>
    public static bool Free(byte* p)
    {
      return false;
    }
  }
}
