using System;
using System.Linq;

namespace TaschenRechnerLib
{
  public partial struct UIntSimple
  {
    /// <summary>
    /// liest eine Zeichenkette als Zahl ein
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Zahl</returns>
    public static UIntSimple Parse(string number)
    {
      return new UIntSimple(number);
    }

    /// <summary>
    /// parst intern eine Zeichenkette als Zahl
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Nummern (Rückwärts gespeichert)</returns>
    static byte[] ParseInternal(string number)
    {
      if (number == null) throw new ArgumentNullException();
      var chars = number.Where(char.IsDigit).ToArray();
      if (chars.Length == 0 || number.Any(c => !char.IsDigit(c) && !char.IsWhiteSpace(c))) throw new FormatException();
      int start = 0;
      while (start < chars.Length - 1 && chars[start] == '0') start++;
      return ParseInternalChars(chars, start);
    }

    /// <summary>
    /// parst intern die einzelnen Nummer-Zeichen und gibt ein fertigen Byte-Array der Zahlen zurück
    /// </summary>
    /// <param name="chars">Zeichenkette mit den Zahlen</param>
    /// <param name="start">Startposition innerhalb der Zeichenkette</param>
    /// <returns>fertig eingelesene Nummern (Rückwärts gespeichert)</returns>
    static unsafe byte[] ParseInternalChars(char[] chars, int start)
    {
      var result = new byte[chars.Length - start];
      fixed (byte* dg = &result[result.Length - 1])
      {
        for (int i = start; i < chars.Length; i++)
        {
          dg[-i + start] = (byte)(chars[i] - '0');
        }
      }
      return result;
    }
  }
}
