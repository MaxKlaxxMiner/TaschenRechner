﻿#region # using *.*
using System;
using System.Diagnostics;

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
    /// subtrahiert zwei Zahlen-Arrays mit einem Multiplkator und gibt ein eventuell vorhandenen Borrow-Wert zurück
    /// </summary>
    /// <param name="target">Basis-Wert, wovon der zweite Wert subtrahiert werden soll</param>
    /// <param name="sub">zweite Wert, welcher subtrahiert werden soll</param>
    /// <param name="subOffset">zusätzlicher Offset</param>
    /// <param name="mul">Muliplikator, welcher verwendet werden soll</param>
    /// <returns>1 = wenn Borrow-Flag gesetzt</returns>
    static int SubMul(int[] target, int[] sub, int subOffset, int mul)
    {
      if (sub.Length + subOffset > target.Length) throw new InvalidCalcException();
      switch (mul)
      {
        case 0: return 0;
        case 1: return Sub(target, sub, subOffset);
      }

      int borrow = 0;

      // --- normale Subtraction ---
      for (int i = 0; i < sub.Length; i++)
      {
        var r = target[i + subOffset] - (long)sub[i] * mul - borrow;
        borrow = (int)((LimbSize - 1 - r) / LimbSize);
        target[i + subOffset] = (int)(r + (long)borrow * LimbSize);
      }

      // --- borrow-flag von den restlichen Zahlen subtrahieren (sofern notwendig) ---
      for (int i = sub.Length + subOffset; borrow != 0 && i < target.Length; i++)
      {
        var r = target[i] - borrow;
        borrow = (LimbSize - 1 - r) / LimbSize;
        target[i] = (int)(r + (long)borrow * LimbSize);
      }

      return borrow;
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
        int sb = SubMul(rem, div, digit, count);
        if (sb != 0) throw new NotImplementedException();
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

      if (SubCheck(val, valOfs, div, counter))
      {
        Debug.Assert(SubCheck(val, valOfs, div, counter + 1) == false);
        return counter;
      }
      else
      {
        //int jump = 1;
        //while (!SubCheck(val, valOfs, div, counter - jump))
        //{
        //  counter -= jump;
        //  jump *= 20;
        //}

        //int startPos = counter - jump;
        int startPos = (int)(chk / (div[div.Length - 1] + 1));
        int endePos = counter;
        do
        {
          int mittePos = (startPos + endePos) >> 1;
          if (!SubCheck(val, valOfs, div, mittePos)) endePos = mittePos; else startPos = mittePos;
        } while (endePos - startPos > 1);

        while (SubCheck(val, valOfs, div, startPos + 1)) startPos++;
        return startPos;
      }
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
