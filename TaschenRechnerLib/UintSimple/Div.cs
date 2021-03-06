﻿using System;

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
        switch (div.digits[0])
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
    /// subtrahiert zwei Zahlen-Arrays mit Multiplikator und gibt ein eventuell vorhandenen Borrow-Wert zurück
    /// </summary>
    /// <param name="target">Basis-Wert, wovon der zweite Wert subtrahiert werden soll</param>
    /// <param name="sub">zweite Wert, welcher subtrahiert werden soll</param>
    /// <param name="subOffset">zusätzlicher Offset</param>
    /// <param name="mul">Multiplikator, welcher beim subtrahieren zusätzlich verwendet werden soll</param>
    /// <returns>1 = wenn Borrow-Flag gesetzt</returns>
    static int SubMul(byte[] target, byte[] sub, int subOffset, int mul)
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
        var r = target[i + subOffset] - sub[i] * mul - borrow;
        borrow = UnsafeHelper.Div10(9 - r);
        target[i + subOffset] = (byte)(r + borrow * 10);
      }

      // --- borrow-flag von den restlichen Zahlen subtrahieren (sofern notwendig) ---
      for (int i = sub.Length + subOffset; borrow != 0 && i < target.Length; i++)
      {
        var r = target[i] - borrow;
        borrow = UnsafeHelper.Div10(9 - r);
        target[i] = (byte)(r + borrow * 10);
      }

      return borrow;
    }

    /// <summary>
    /// berechnet eine Division mit Rest anhand der Schulmethode
    /// </summary>
    /// <param name="rem">Dividend, welcher zum Rest wird</param>
    /// <param name="div">Divisor, welcher verwendet werden soll</param>
    /// <param name="quo">Quotient, welcher als Ergebnis ausgegeben wird</param>
    static void DivModInternal(byte[] rem, byte[] div, byte[] quo)
    {
      for (int digit = rem.Length - div.Length; digit >= 0; digit--)
      {
        int count = CountValue(rem, digit, div);
        int sb = SubMul(rem, div, digit, count);
        if (sb != 0) throw new NotImplementedException();
        quo[digit] = (byte)count;
      }
    }

    /// <summary>
    /// ermittelt, wie oft der Div-Wert von der gewünschten Stelle im Value-Werte abgezogen werden kann
    /// </summary>
    /// <param name="val">Value-Wert, welche benutzt werden soll</param>
    /// <param name="valOfs">Startposition im Value-Wert</param>
    /// <param name="div">Div-Wert, welcher abgezogen werden soll</param>
    /// <returns>Häufigkeit, wie oft der Div-Wert abgezogen werden kann</returns>
    static int CountValue(byte[] val, int valOfs, byte[] div)
    {
      int counter = val[valOfs + div.Length - 1];
      if (valOfs + div.Length < val.Length) counter += val[valOfs + div.Length] * 10;
      counter /= div[div.Length - 1];
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
    static bool SubCheck(byte[] val, int valOfs, byte[] sub, int subMul)
    {
      int borrow = 0;

      for (int i = 0; i < sub.Length; i++)
      {
        var r = val[i + valOfs] - sub[i] * subMul - borrow;
        borrow = (9 - r) / 10;
      }

      if (sub.Length + valOfs < val.Length) borrow -= val[sub.Length + valOfs];

      return borrow <= 0;
    }
  }
}
