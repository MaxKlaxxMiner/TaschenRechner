using System;

namespace TaschenRechnerLib
{
  public partial struct UIntSimple
  {
    /// <summary>
    /// Operator zum subtrahieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntSimple operator -(UIntSimple val1, UIntSimple val2)
    {
      return Sub(val1, val2);
    }

    /// <summary>
    /// dekrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche dekrementiert werden soll</param>
    /// <returns>fertig dekrementierte Zahl</returns>
    public static UIntSimple operator --(UIntSimple val)
    {
      return Sub(val, One);
    }

    /// <summary>
    /// Methode zum subtrahieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static UIntSimple Sub(UIntSimple val1, UIntSimple val2)
    {
      var digits1 = val1.digits;
      var digits2 = val2.digits;
      if (digits2.Length == 1 && digits2[0] == 0) return val1; // kein Wert zum Subtrahieren vorhanden
      if (digits2.Length > digits1.Length) throw new InvalidCalcException();

      var result = new byte[digits1.Length];
      Array.Copy(digits1, result, digits1.Length);

      if (Sub(result, digits2) != 0) throw new InvalidCalcException();

      return new UIntSimple(SubNormalize(result));
    }

    /// <summary>
    /// kürzt einen Subtrahierten Wert (sofern notwendig)
    /// </summary>
    /// <param name="val">Wert, welcher normalisiert werden soll</param>
    /// <returns>fertiger normalisierter Wert</returns>
    static byte[] SubNormalize(byte[] val)
    {
      int last = val.Length - 1;
      while (last > 0 && val[last] == 0) last--;
      if (last == val.Length - 1) return val; // Wert kann beibehalten werden

      var result = new byte[last + 1];
      Array.Copy(val, result, result.Length);
      return result;
    }

    /// <summary>
    /// subtrahiert zwei Zahlen-Arrays und ein eventuell vorhandenen Borrow-Wert zurück
    /// </summary>
    /// <param name="target">Basis-Wert, wovon der zweite Wert subtrahiert werden soll</param>
    /// <param name="sub">zweite Wert, welcher subtrahiert werden soll</param>
    /// <returns>1 = wenn Borrow-Flag gesetzt</returns>
    static int Sub(byte[] target, byte[] sub)
    {
      if (sub.Length > target.Length) throw new InvalidCalcException();

      int borrow = 0;

      // --- normale Subtraction ---
      for (int i = 0; i < sub.Length; i++)
      {
        var r = target[i] - sub[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i] = (byte)(r + borrow * 10);
      }

      // --- borrow-flag von den restlichen Zahlen subtrahieren (sofern notwendig) ---
      for (int i = sub.Length; borrow != 0 && i < target.Length; i++)
      {
        var r = target[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i] = (byte)(r + borrow * 10);
      }

      return borrow;
    }

    /// <summary>
    /// subtrahiert zwei Zahlen-Arrays und gibt ein eventuell vorhandenen Borrow-Wert zurück
    /// </summary>
    /// <param name="target">Basis-Wert, wovon der zweite Wert subtrahiert werden soll</param>
    /// <param name="sub">zweite Wert, welcher subtrahiert werden soll</param>
    /// <param name="subOffset">zusätzlicher Offset</param>
    /// <returns>1 = wenn Borrow-Flag gesetzt</returns>
    static int Sub(byte[] target, byte[] sub, int subOffset)
    {
      if (sub.Length + subOffset > target.Length) throw new InvalidCalcException();

      int borrow = 0;

      // --- normale Subtraction ---
      for (int i = 0; i < sub.Length; i++)
      {
        var r = target[i + subOffset] - sub[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i + subOffset] = (byte)(r + borrow * 10);
      }

      // --- borrow-flag von den restlichen Zahlen subtrahieren (sofern notwendig) ---
      for (int i = sub.Length + subOffset; borrow != 0 && i < target.Length; i++)
      {
        var r = target[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i] = (byte)(r + borrow * 10);
      }

      return borrow;
    }
  }
}
