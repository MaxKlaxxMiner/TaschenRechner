using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public sealed partial class UIntLimbs
  {
    /// <summary>
    /// merkt sich die Char-Map für die Zahlen von 0 bis 999 ('0','0','0', '0','0','1', '0','0','2', '0','0','3', '0','0','4', usw. bis '9', '9', '9')
    /// </summary>
    static readonly unsafe char* DirectChars1K = GenDirectChars1K();

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
    /// gibt die Zahl als Zeichenfolge zurück (langsame Berechnung)
    /// </summary>
    /// <param name="limbs">Limbs, welche verwendet werden sollen</param>
    /// <returns>fertige Zahl als Zeichenkette</returns>
    static string GetStringSlow(int[] limbs)
    {
      var sb = new StringBuilder(limbs[limbs.Length - 1].ToString());

      for (int i = limbs.Length - 2; i >= 0; i--)
      {
        sb.Append(limbs[i].ToString("D9"));
      }

      return sb.ToString();
    }

    /// <summary>
    /// gibt die Zahl als Zeichenfolge mit Tausender-Punkten zurück (langsame Berechnung)
    /// </summary>
    /// <param name="limbs">Limbs, welche verwendet werden sollen</param>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll</param>
    /// <returns>fertige Zahl als Zeichenkette</returns>
    static string GetStringThSlow(int[] limbs, char sepChar)
    {
      int first = limbs[limbs.Length - 1];
      var sb = new StringBuilder(first.ToString());
      if (first >= 1000)
      {
        sb.Insert(sb.Length - 3, sepChar);
        if (first >= 1000000) sb.Insert(sb.Length - 7, sepChar);
      }

      for (int i = limbs.Length - 2; i >= 0; i--)
      {
        sb.Append(sepChar).Append(limbs[i].ToString("D9").Insert(6, sepChar.ToString()).Insert(3, sepChar.ToString()));
      }

      return sb.ToString();
    }

    /// <summary>
    /// berechnet die Anzahl der benötigten Ziffern um eine Limbs-Zahl darzustellen
    /// </summary>
    /// <param name="limbs">Limbs, welche berechnet werden sollen</param>
    /// <returns>Anzahl der Ziffer, welche benötigt werden</returns>
    static int GetStrLen(int[] limbs)
    {
      return (limbs.Length - 1) * 9 + GetStrLen(limbs[limbs.Length - 1]);
    }

    /// <summary>
    /// setzt fix die nächsten 3 Teilziffern in die Zeichenfolge
    /// </summary>
    /// <param name="p">Zeiger der Zeichenfolge, wohin die Zeichen gesetzt werden sollen</param>
    /// <param name="limb3">Teil-Limb, welches verwendet werden soll</param>
    static unsafe void Mark3Chars(char* p, int limb3)
    {
      var c = DirectChars1K + limb3 * 3;
      *(ulong*)p |= *(ulong*)c & 0xffffffffffff;
    }

    /// <summary>
    /// setzt die nächsten 3 Teilziffern in die Zeichenfolge mit Limit-Angabe
    /// </summary>
    /// <param name="p">Zeiger der Zeichenfolge, wohin die Zeichen gesetzt werden sollen</param>
    /// <param name="limb3">Teil-Limb, welches verwendet werden soll</param>
    /// <param name="len">Länge der Limb-Folge (1-3)</param>
    static unsafe void Mark3Chars(char* p, int limb3, int len)
    {
      var c = DirectChars1K + limb3 * 3;
      switch (len)
      {
        case 1: p[0] = c[2]; break;
        case 2: p[0] = c[1]; p[1] = c[2]; break;
        case 3: p[0] = c[0]; p[1] = c[1]; p[2] = c[2]; break;
      }
    }

    /// <summary>
    /// setzt fix die nächsten 9 Ziffern mit Tausender-Trennzeichen (3) in die Zeichenfolge
    /// </summary>
    /// <param name="p">Zeiger der Zeichenfolge, wohin die Zeichen gesetzt werden sollen</param>
    /// <param name="limb">Limb, welches verwendet werden soll</param>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll</param>
    static unsafe void Mark12CharsTh(char* p, int limb, char sepChar)
    {
      int l3 = limb / 1000000; limb -= l3 * 1000000;
      var c3 = DirectChars1K + l3 * 3;
      int l2 = limb / 1000; limb -= l2 * 1000;
      var c2 = DirectChars1K + l2 * 3;
      var c1 = DirectChars1K + limb * 3;
      *(ulong*)p = *(ulong*)c3 << 16 | sepChar;
      *(ulong*)(p + 4) = *(ulong*)c2 << 16 | sepChar;
      *(ulong*)(p + 8) = *(ulong*)c1 << 16 | sepChar;
    }

    /// <summary>
    /// markiert die ersten Zeichen (1-9) in der Zeichenfolge und gibt die Anzahl gesetzt Zeichen zurück
    /// </summary>
    /// <param name="p">Zeiger der Zeichenfolge, wohin die Zeichen gesetzt werden sollen</param>
    /// <param name="limb">Limb, welches verwendet werden soll</param>
    /// <returns>Anzahl der markierten Zeichen</returns>
    static unsafe int MarkFirstChars(char* p, int limb)
    {
      int len = GetStrLen(limb);

      switch (len)
      {
        case 1: Mark3Chars(p, limb, 1); break;
        case 2: Mark3Chars(p, limb, 2); break;
        case 3: Mark3Chars(p, limb); break;
        case 4: Mark3Chars(p, limb / 1000, 1); Mark3Chars(p + 1, limb % 1000); break;
        case 5: Mark3Chars(p, limb / 1000, 2); Mark3Chars(p + 2, limb % 1000); break;
        case 6: Mark3Chars(p, limb / 1000); Mark3Chars(p + 3, limb % 1000); break;
        case 7: Mark3Chars(p, limb / 1000000, 1); Mark3Chars(p + 1, limb / 1000 % 1000); Mark3Chars(p + 4, limb % 1000); break;
        case 8: Mark3Chars(p, limb / 1000000, 2); Mark3Chars(p + 2, limb / 1000 % 1000); Mark3Chars(p + 5, limb % 1000); break;
        case 9: Mark3Chars(p, limb / 1000000); Mark3Chars(p + 3, limb / 1000 % 1000); Mark3Chars(p + 6, limb % 1000); break;
        default: throw new Exception();
      }

      return len;
    }

    /// <summary>
    /// markiert die ersten Zeichen (1-9) in der Zeichenfolge mit Tausenderpunkt und gibt die Anzahl gesetzt Zeichen zurück
    /// </summary>
    /// <param name="p">Zeiger der Zeichenfolge, wohin die Zeichen gesetzt werden sollen</param>
    /// <param name="limb">Limb, welches verwendet werden soll</param>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll</param>
    /// <returns>Anzahl der markierten Zeichen</returns>
    static unsafe int MarkFirstThChars(char* p, int limb, char sepChar)
    {
      int len = GetStrLen(limb);

      switch (len)
      {
        case 1: Mark3Chars(p, limb, 1); return 1;
        case 2: Mark3Chars(p, limb, 2); return 2;
        case 3: Mark3Chars(p, limb); return 3;
        case 4: Mark3Chars(p, limb / 1000, 1); p[1] = sepChar; Mark3Chars(p + 2, limb % 1000); return 5;
        case 5: Mark3Chars(p, limb / 1000, 2); p[2] = sepChar; Mark3Chars(p + 3, limb % 1000); return 6;
        case 6: Mark3Chars(p, limb / 1000); p[3] = sepChar; Mark3Chars(p + 4, limb % 1000); return 7;
        case 7: Mark3Chars(p, limb / 1000000, 1); p[1] = sepChar; Mark3Chars(p + 2, limb / 1000 % 1000); p[5] = sepChar; Mark3Chars(p + 6, limb % 1000); return 9;
        case 8: Mark3Chars(p, limb / 1000000, 2); p[2] = sepChar; Mark3Chars(p + 3, limb / 1000 % 1000); p[6] = sepChar; Mark3Chars(p + 7, limb % 1000); return 10;
        case 9: Mark3Chars(p, limb / 1000000); p[3] = sepChar; Mark3Chars(p + 4, limb / 1000 % 1000); p[7] = sepChar; Mark3Chars(p + 8, limb % 1000); return 11;
        default: throw new Exception();
      }
    }

    /// <summary>
    /// gibt die Zahl als Zeichenfolge zurück (schnelle Berechnung)
    /// </summary>
    /// <param name="limbs">Limbs, welche verwendet werden sollen</param>
    /// <returns>fertige Zahl als Zeichenkette</returns>
    static unsafe string GetStringFast(int[] limbs)
    {
      int len = GetStrLen(limbs);
      string tmp = UnsafeHelper.FastAllocateString(len);
      fixed (char* tmpP = tmp)
      {
        var p = tmpP + MarkFirstChars(tmpP, limbs[limbs.Length - 1]);
        if (limbs.Length > 1) FastMark9Steps(limbs, p);
      }
      return tmp;
    }

    private static unsafe void FastMark9Steps(int[] limbs, char* p)
    {
      fixed (int* limbsP = limbs)
      {
        for (int i = limbs.Length - 2; i > 0; i--)
        {
          int limb = limbsP[i];
          int l3 = UnsafeHelper.Div1000000(limb); limb -= l3 * 1000000;
          var c3 = DirectChars1K + l3 * 3;
          *(ulong*)p = *(ulong*)c3;

          int l2 = UnsafeHelper.Div1000(limb); limb -= l2 * 1000;
          var c2 = DirectChars1K + l2 * 3;
          *(ulong*)(p + 3) = *(ulong*)c2;

          var c1 = DirectChars1K + limb * 3;
          *(ulong*)(p + 6) = *(ulong*)c1;
          p += 9;
        }
        // --- Last-Limb ---
        {
          int limb = limbsP[0];
          int l3 = UnsafeHelper.Div1000000(limb); limb -= l3 * 1000000;
          var c3 = DirectChars1K + l3 * 3;
          *(ulong*)p = *(ulong*)c3;

          int l2 = UnsafeHelper.Div1000(limb); limb -= l2 * 1000;
          var c2 = DirectChars1K + l2 * 3;
          *(ulong*)(p + 3) = *(ulong*)c2 & 0xffffffffffff;

          var c1 = DirectChars1K + limb * 3;
          *(ulong*)(p + 6) |= *(ulong*)c1 & 0xffffffffffff;
          p += 9;
        }
      }
    }

    /// <summary>
    /// gibt die Zahl als Zeichenfolge zurück (schnelle Berechnung)
    /// </summary>
    /// <param name="limbs">Limbs, welche verwendet werden sollen</param>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll</param>
    /// <returns>fertige Zahl als Zeichenkette</returns>
    static unsafe string GetStringThFast(int[] limbs, char sepChar)
    {
      int len = GetStrLen(limbs);
      len += (len - 1) / 3;
      string tmp = UnsafeHelper.FastAllocateString(len);
      fixed (char* tmpP = tmp)
      {
        var p = tmpP + MarkFirstThChars(tmpP, limbs[limbs.Length - 1], sepChar);
        for (int i = limbs.Length - 2; i >= 0; i--)
        {
          Mark12CharsTh(p, limbs[i], sepChar); p += 12;
        }
      }
      return tmp;
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette zurück
    /// </summary>
    /// <returns>lesbare Zeichenkette</returns>
    public override string ToString()
    {
      string result = GetStringFast(limbs);

      Debug.Assert(result == GetStringSlow(limbs));

      return result;
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette mit Tausender-Punkten zurück
    /// </summary>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll (default: ',')</param>
    /// <returns>lesbare Zeichenkette</returns>
    public string ToStringThousandsSep(char sepChar = ',')
    {
      string result = GetStringThFast(limbs, sepChar);

      Debug.Assert(result == GetStringThSlow(limbs, sepChar));

      return result;
    }
  }
}
