using System;
using System.Linq;
using System.Numerics;

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// liest eine Zeichenkette als Zahl ein
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Zahl</returns>
    public static UIntBig Parse(string number)
    {
      return new UIntBig(number);
    }

    /// <summary>
    /// parst intern eine Zeichenkette als Zahl
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Nummern (Rückwärts gespeichert)</returns>
    static uint[] ParseInternal(string number)
    {
      if (number == null) throw new ArgumentNullException();
      var chars = number.Where(char.IsDigit).ToArray();
      if (chars.Length == 0 || number.Any(c => !char.IsDigit(c) && !char.IsWhiteSpace(c))) throw new FormatException();

      return GetBitsFromOld(BigInteger.Parse(number));
    }
  }
}
