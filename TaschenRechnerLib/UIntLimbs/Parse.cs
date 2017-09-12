using System;
using System.Diagnostics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntLimbs
  {
    /// <summary>
    /// liest eine Zeichenkette als Zahl ein
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Zahl</returns>
    public static UIntLimbs Parse(string number)
    {
      return new UIntLimbs(number);
    }

    /// <summary>
    /// gleiche wie <see cref="Parse"/>, jedoch werden innerhalb einer langen Zahl keine Prüfungen durchgeführt, was Geschwindigkeit bei langen Nummern deutlich steigert (70%-80% schneller)
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Zahl</returns>
    public static UIntLimbs ParseFast(string number)
    {
      return new UIntLimbs(ParseInternalFast(number));
    }

    /// <summary>
    /// parst intern eine Zeichenkette al Zahl
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Nummern (Rückwärts gespeichert)</returns>
    static unsafe int[] ParseInternal(string number)
    {
      if (string.IsNullOrWhiteSpace(number)) throw new ArgumentNullException();

      int firstDigit = 0;

      for (; firstDigit < number.Length; firstDigit++)
      {
        char c = number[firstDigit];
        if (c > '0' && c <= '9') break; // erste Zahl gefunden
        if (!char.IsWhiteSpace(c) && c != '0') throw new FormatException();
      }

      int lastDigit = number.Length;
      for (; lastDigit > firstDigit; lastDigit--)
      {
        char c = number[lastDigit - 1];
        if (c >= '0' && c <= '9') break; // letzte Zahl gefunden
      }

      if (lastDigit == firstDigit) return Zero.limbs;

      int len = lastDigit - firstDigit;
      var result = new int[(len + 8) / 9];

      int firstDigits = len % 9;

      fixed (char* chars = number)
      {
        var p = chars + firstDigit + len - 9;
        fixed (int* resultP = result)
        {
          if (firstDigits == 0)
          {
            Parse9Steps(resultP, result.Length, p);
          }
          else
          {
            Parse9Steps(resultP, result.Length - 1, p);
            result[result.Length - 1] = ParseLimitedDigits(chars + firstDigit, firstDigits);
          }
        }
      }

      return result;
    }

    /// <summary>
    /// schneller Parser zum parsen von Ziffern in 9er Schritten
    /// </summary>
    /// <param name="results">Ausgabe-Array der Limbs-Ergebnisse</param>
    /// <param name="limbCount">Anzahl der zu parsenden Limbs</param>
    /// <param name="chars">Zeiger auf die Zeichen, welche geparst werden sollen</param>
    static unsafe void Parse9Steps(int* results, int limbCount, char* chars)
    {
      for (int i = 0; i < limbCount; i++)
      {
        long c1 = *(long*)chars - ('0' + ((long)'0' << 16) + ((long)'0' << 32) + ((long)'0' << 48));
        long c2 = (*(long*)(chars + 4) & 0x7fff7fff7fff7fff) - ('0' + ((long)'0' << 16) + ((long)'0' << 32) + ((long)'0' << 48));
        int result = chars[8] - '0';

        if ((uint)result > 9) throw new FormatException();

        if ((c1 & 0x7ff0fff0fff0fff0) != 0 || ((c1 - 0x000a000a000a000a) & 0x7ff0fff0fff0fff0) != 0x7ff0fff0fff0fff0) throw new FormatException();
        result += (int)(c1 & 0xf) * 100000000 + (int)(c1 >> 16 & 0xf) * 10000000 + (int)(c1 >> 32 & 0xf) * 1000000 + (int)(c1 >> 48 & 0xf) * 100000;

        if ((c1 & 0x7ff0fff0fff0fff0) != 0 || ((c2 - 0x000a000a000a000a) & 0x7ff0fff0fff0fff0) != 0x7ff0fff0fff0fff0) throw new FormatException();
        result += (int)(c2 & 0xf) * 10000 + (int)(c2 >> 16 & 0xf) * 1000 + (int)(c2 >> 32 & 0xf) * 100 + (int)(c2 >> 48 & 0xf) * 10;

        Debug.Assert(result == Parse9DigitsSlow(chars));

        chars -= 9;
        results[i] = result;
      }
    }

    /// <summary>
    /// parst intern eine Zeichenkette al Zahl
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Nummern (Rückwärts gespeichert)</returns>
    static unsafe int[] ParseInternalFast(string number)
    {
      if (string.IsNullOrWhiteSpace(number)) throw new ArgumentNullException();

      int firstDigit = 0;

      for (; firstDigit < number.Length; firstDigit++)
      {
        char c = number[firstDigit];
        if (c > '0' && c <= '9') break; // erste Zahl gefunden
        if (!char.IsWhiteSpace(c) && c != '0') throw new FormatException();
      }

      int lastDigit = number.Length;
      for (; lastDigit > firstDigit; lastDigit--)
      {
        char c = number[lastDigit - 1];
        if (c >= '0' && c <= '9') break; // letzte Zahl gefunden
      }

      if (lastDigit == firstDigit) return Zero.limbs;

      int len = lastDigit - firstDigit;
      var result = new int[(len + 8) / 9];

      int firstDigits = len % 9;

      fixed (char* chars = number)
      {
        var p = chars + firstDigit + len - 9;
        fixed (int* resultP = result)
        {
          if (firstDigits == 0)
          {
            Parse9StepsFast(resultP, result.Length, p);
          }
          else
          {
            Parse9StepsFast(resultP, result.Length - 1, p);
            result[result.Length - 1] = ParseLimitedDigits(chars + firstDigit, firstDigits);
          }
        }
      }

      return result;
    }

    /// <summary>
    /// schneller Parser zum parsen von Ziffern in 9er Schritten
    /// </summary>
    /// <param name="results">Ausgabe-Array der Limbs-Ergebnisse</param>
    /// <param name="limbCount">Anzahl der zu parsenden Limbs</param>
    /// <param name="chars">Zeiger auf die Zeichen, welche geparst werden sollen</param>
    static unsafe void Parse9StepsFast(int* results, int limbCount, char* chars)
    {
      const int SubZeros = unchecked('0' * 100000000 + '0' * 10000000 + '0' * 1000000 + '0' * 100000 + '0' * 10000 + '0' * 1000 + '0' * 100 + '0' * 10 + '0');

      for (int i = 0; i < limbCount; i++)
      {
        long c1 = *(long*)chars;
        long c2 = *(long*)(chars + 4);
        int result = chars[8] - SubZeros;

        chars -= 9;

        result += (byte)c1 * 100000000 + (byte)c2 * 10000 + (byte)(c1 >> 16) * 10000000 + (byte)(c2 >> 16) * 1000 +
                  (byte)(c1 >> 32) * 1000000 + (byte)(c2 >> 32) * 100 + (byte)(c1 >> 48) * 100000 + (byte)(c2 >> 48) * 10;

        results[i] = result;
      }
    }

    /// <summary>
    /// Parst die Ziffern in einer Zeichenfolge mit begrenzter Länge
    /// </summary>
    /// <param name="chars">Zeiger auf die Zeichenfolge, wo die Ziffern geparst werden sollen</param>
    /// <param name="len">Länge der zu parsenden Zeichenfolge (1-9)</param>
    /// <returns>fertig geparste Zahl</returns>
    static unsafe int ParseLimitedDigits(char* chars, int len)
    {
      int result = 0;
      for (int i = 0; i < len; i++)
      {
        int c = chars[i] - '0';
        result *= 10;
        if ((uint)c > 9) throw new FormatException();
        result += c;
      }
      return result;
    }

    /// <summary>
    /// Parst fix die 9 nächsten Ziffen in einer Zeichenfolge
    /// </summary>
    /// <param name="chars">Zeiger auf die Zeichenfolge, wo die Ziffern geparst werden sollen</param>
    /// <returns>fertig geparste Zahl</returns>
    static unsafe int Parse9DigitsSlow(char* chars)
    {
      int result = 0;
      for (int i = 0; i < 9; i++)
      {
        int c = chars[i] - '0';
        result *= 10;
        if ((uint)c > 9) throw new FormatException();
        result += c;
      }
      return result;
    }

    static unsafe int Parse9Digits(char* chars)
    {
      long c1 = *(long*)chars - ('0' + ((long)'0' << 16) + ((long)'0' << 32) + ((long)'0' << 48));
      long c2 = (*(long*)(chars + 4) & 0x7fff7fff7fff7fff) - ('0' + ((long)'0' << 16) + ((long)'0' << 32) + ((long)'0' << 48));
      int c3 = chars[8] - '0';

      if ((c1 & 0x7ff0fff0fff0fff0) != 0) throw new FormatException();
      int result = (int)(c1 & 0xf) * 100000000 + (int)(c1 >> 16 & 0xf) * 10000000 + (int)(c1 >> 32 & 0xf) * 1000000 + (int)(c1 >> 48 & 0xf) * 100000;

      if ((c2 & 0x7ff0fff0fff0fff0) != 0) throw new FormatException();
      result += (int)(c2 & 0xf) * 10000 + (int)(c2 >> 16 & 0xf) * 1000 + (int)(c2 >> 32 & 0xf) * 100 + (int)(c2 >> 48 & 0xf) * 10;

      if ((uint)c3 > 9) throw new FormatException();
      result += c3;

      Debug.Assert(result == Parse9DigitsSlow(chars));
      return result;
    }
  }
}
