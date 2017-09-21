using System;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib.BigIntegerExtras;

namespace TaschenRechnerLib
{
  public sealed unsafe partial class UIntX
  {
    /// <summary>
    /// liest eine Zeichenkette als Zahl ein
    /// </summary>
    /// <param name="number">Zeichenkette, welche als Zahl eingelesen werden soll</param>
    /// <returns>fertig eingelesene Zahl</returns>
    public static UIntX Parse(string number)
    {
      return new UIntX(number);
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

      return BigIntegerHelpers.GetBitsFromOld(BigInteger.Parse(number));
    }
  }
}
