#region # using *.*
using System;
using System.Diagnostics;
using TaschenRechnerLib.BigIntegerExtras;

// ReSharper disable TailRecursiveCall
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
// ReSharper disable RedundantIfElseBlock
// ReSharper disable RedundantLogicalConditionalExpressionOperand
#pragma warning disable 162
#endregion

namespace TaschenRechnerLib
{
  public partial struct UIntSimple
  {
    /// <summary>
    /// Operator zum berechnen des Modulus zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntSimple operator %(UIntSimple val1, UIntSimple val2)
    {
      return Mod(val1, val2);
    }

    /// <summary>
    /// führt eine Modulo-Berechnung durch
    /// </summary>
    /// <param name="val">Dividend, welcher verwendet werden soll</param>
    /// <param name="div">Divisor, welcher zum teilen verwendet werden soll</param>
    /// <returns>Ergebnis: Rest</returns>
    static UIntSimple Mod(UIntSimple val, UIntSimple div)
    {
      if (div.digits.Length == 1)
      {
        switch (div.digits[0])
        {
          case 0: throw new DivideByZeroException();
          case 1: return Zero; // x % 1 = 0
          case 2: return new UIntSimple(new[] { (byte)(val.digits[0] & 1) }); // x % 2 = x & 1
          case 5: return new UIntSimple(new[] { (byte)(val.digits[0] % 5) }); // x % 5 = x % 5
        }
      }
      if (div.digits.Length > val.digits.Length) return val; // zu kleines Ergebnis (gesamter Dividend bleibt als Rest übrig)

      var remainder = new byte[val.digits.Length];
      Array.Copy(val.digits, remainder, remainder.Length);
      var quotient = new byte[val.digits.Length - div.digits.Length + 1];

      DivModInternal(remainder, div.digits, quotient);

      return new UIntSimple(SubNormalize(remainder));
    }
  }
}
