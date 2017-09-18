#region # using *.*
using System;
// ReSharper disable TailRecursiveCall
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
// ReSharper disable RedundantIfElseBlock
// ReSharper disable RedundantLogicalConditionalExpressionOperand
#pragma warning disable 162
#endregion

namespace TaschenRechnerLib
{
  public partial struct UIntLimbs
  {
    /// <summary>
    /// Operator zum dividieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntLimbs operator /(UIntLimbs val1, UIntLimbs val2)
    {
      return Div(val1, val2);
    }

    /// <summary>
    /// führt eine Division durch
    /// </summary>
    /// <param name="val">Dividend, welcher verwendet werden soll</param>
    /// <param name="div">Divisor, welcher zum teilen verwendet werden soll</param>
    /// <returns>Ergebnis: Quotient</returns>
    static UIntLimbs Div(UIntLimbs val, UIntLimbs div)
    {
      if (div.limbs.Length == 1)
      {
        switch (div.limbs[0])
        {
          case 0: throw new DivideByZeroException();
          case 1: return val; // x / 1 == x
        }
      }
      if (div.limbs.Length > val.limbs.Length) return Zero; // zu kleines Ergebnis

      var remainder = new int[val.limbs.Length];
      Array.Copy(val.limbs, remainder, remainder.Length);
      var quotient = new int[val.limbs.Length - div.limbs.Length + 1];

      DivModInternal(remainder, div.limbs, quotient);

      return new UIntLimbs(SubNormalize(quotient));
    }

    /// <summary>
    /// berechnet eine Division mit Rest anhand der Schulmethode
    /// </summary>
    /// <param name="rem">Dividend, welcher zum Rest wird</param>
    /// <param name="div">Divisor, welcher verwendet werden soll</param>
    /// <param name="quo">Quotient, welcher als Ergebnis ausgegeben wird</param>
    static void DivModInternal(int[] rem, int[] div, int[] quo)
    {
      for (int digit = rem.Length - div.Length; digit >= 0; digit--)
      {
        int count = CountValue(rem, digit, div);
        for (int i = 0; i < count; i++)
        {
          int sb = Sub(rem, div, digit);
          if (sb != 0) throw new NotImplementedException();
        }
        quo[digit] = count;
      }
    }

    /// <summary>
    /// ermittelt, wie oft der Div-Wert von der gewünschten Stelle im Value-Werte abgezogen werden kann
    /// </summary>
    /// <param name="val">Value-Wert, welche benutzt werden soll</param>
    /// <param name="valOfs">Startposition im Value-Wert</param>
    /// <param name="div">Div-Wert, welcher abgezogen werden soll</param>
    /// <returns>Häufigkeit, wie oft der Div-Wert abgezogen werden kann</returns>
    static int CountValue(int[] val, int valOfs, int[] div)
    {
      long chk = val[valOfs + div.Length - 1];
      if (valOfs + div.Length < val.Length) chk += (long)val[valOfs + div.Length] * LimbSize;
      int counter = (int)(chk / div[div.Length - 1]);
      if (!SubCheck(val, valOfs, div, counter))
      {
        counter--;
        while (!SubCheck(val, valOfs, div, counter)) counter--;
        return counter;
      }
      while (SubCheck(val, valOfs, div, counter + 1)) counter++;
      return counter;
    }

    /// <summary>
    /// prüft, ob ein bestimmter Wert subtrahiert werden kann
    /// </summary>
    /// <param name="val">Wert, wovon subtrahiert werden soll</param>
    /// <param name="valOfs">Offset innerhalb des Wertes</param>
    /// <param name="sub">Wert, welcher subtrahiert werden soll</param>
    /// <param name="subMul">Multiplikator des zu subtrahierenden Wertes</param>
    /// <returns>true, wenn der Wert subtrahiert werden kann</returns>
    static bool SubCheck(int[] val, int valOfs, int[] sub, int subMul)
    {
      long borrow = 0;

      for (int i = 0; i < sub.Length; i++)
      {
        var r = val[i + valOfs] - sub[i] * (long)subMul - borrow;
        borrow = ((LimbSize - 1L) - r) / LimbSize;
      }

      if (sub.Length + valOfs < val.Length) borrow -= val[sub.Length + valOfs];

      return borrow <= 0;
    }
  }
}
