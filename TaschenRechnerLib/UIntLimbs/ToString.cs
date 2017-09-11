using System.Runtime.InteropServices;

namespace TaschenRechnerLib
{
  public sealed partial class UIntLimbs
  {
    /// <summary>
    /// merkt sich die Char-Map für die Zahlen von 0 bis 999 ('0','0','0', '0','0','1', '0','0','2', '0','0','3', '0','0','4', usw. bis '9', '9', '9')
    /// </summary>
    internal static readonly unsafe char* DirectChars1K = GenDirectChars1K();

    /// <summary>
    /// generiert die Char-Map für die Zahlen von 0 bis 999
    /// </summary>
    /// <returns>Zeiger auf die fertige Char-Map</returns>
    static unsafe char* GenDirectChars1K()
    {
      var result = Marshal.AllocHGlobal(1000 * 3 * 2);

      var chars = (char*)result;
      for (char n0 = '0'; n0 <= '9'; n0++)
      {
        for (char n1 = '0'; n1 <= '9'; n1++)
        {
          for (char n2 = '0'; n2 <= '9'; n2++)
          {
            *chars++ = n0;
            *chars++ = n1;
            *chars++ = n2;
          }
        }
      }

      // --- check values ---
      // var dummy = new char[1000 * 3];
      // Marshal.Copy(result, dummy, 0, dummy.Length);

      return (char*)result;
    }

    /// <summary>
    /// gibt Anzahl der benötigten Ziffern eines Limbs zurück (1 - 9)
    /// </summary>
    /// <param name="limb">Limb, welches berechnet werden soll</param>
    /// <returns>Anzahl der Ziffer, welche benötigt werden</returns>
    static int GetStrLen(int limb)
    {
      if (limb < 100) return limb < 10 ? 1 : 2; // 1-2 Ziffern
      // --- 3-9 Ziffern ---
      if (limb < 10000) return limb < 1000 ? 3 : 4; // 3-4 Ziffern
      // --- 5-9 Ziffern ---
      if (limb < 1000000) return limb < 100000 ? 5 : 6; // 5-6 Ziffern
      // --- 7-9 Ziffern ---
      if (limb < 10000000) return 7; // 7 Ziffern
      return limb < 100000000 ? 8 : 9; // 8-9 Ziffern
    }

    /// <summary>
    /// berechnet die Anzahl der benötigten Ziffern um eine Limbs-Zahl darzustellen
    /// </summary>
    /// <param name="limbs">Limbs, welche berechnet werden sollen</param>
    /// <returns>Anzahl der Ziffer, welche benötigt werden</returns>
    static int GetStrLen(int[] limbs)
    {
      if (limbs.Length == 1) return GetStrLen(limbs[0]);

      return (limbs.Length - 1) * 9 + GetStrLen(limbs[limbs.Length - 1]);
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette zurück
    /// </summary>
    /// <returns>lesbare Zeichenkette</returns>
    public unsafe override string ToString()
    {
      int len = GetStrLen(limbs);
      string tmp = UnsafeHelper.FastAllocateString(len);

      return "";
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette mit Tausender-Punkten zurück
    /// </summary>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll (default: ',')</param>
    /// <returns>lesbare Zeichenkette</returns>
    public unsafe string ToStringThousandsSep(char sepChar = ',')
    {
      return "";
    }
  }
}
