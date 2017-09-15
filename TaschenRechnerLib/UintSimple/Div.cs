using System;

namespace TaschenRechnerLib
{
  public partial struct UIntSimple
  {
    /// <summary>
    /// Operator zum dividieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntSimple operator /(UIntSimple val1, UIntSimple val2)
    {
      return Div(val1, val2);
    }

    /// <summary>
    /// führt eine Division durch
    /// </summary>
    /// <param name="val">Dividend, welcher verwendet werden soll</param>
    /// <param name="div">Divisor, welcher zum teilen verwendet werden soll</param>
    /// <returns>Ergebnis: Quotient</returns>
    static UIntSimple Div(UIntSimple val, UIntSimple div)
    {
      if (div.digits.Length == 1)
      {
        switch(div.digits[0])
        {
          case 0: throw new DivideByZeroException();
          case 1: return val; // x / 1 == x
        }
      }
      if (div.digits.Length > val.digits.Length) return Zero; // zu kleines Ergebnis

      var remainder = new byte[val.digits.Length];
      Array.Copy(val.digits, remainder, remainder.Length);
      var quotient = new byte[val.digits.Length - div.digits.Length + 1];

      DivModInternal(remainder, div.digits, quotient);

      return new UIntSimple(SubNormalize(quotient));
    }

    /// <summary>
    /// berechnet eine Division mit Rest anhand der Schulmethode
    /// </summary>
    /// <param name="rem">Dividend, welcher zum Rest wird</param>
    /// <param name="div">Divisor, welcher verwendet werden soll</param>
    /// <param name="quo">Quotient, welcher als Ergebnis ausgegeben wird</param>
    static void DivModInternal(byte[] rem, byte[] div, byte[] quo)
    {

    }
  }
}
