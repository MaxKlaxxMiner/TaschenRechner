using System;

namespace TaschenRechnerLib
{
  public partial struct UIntLimbs
  {
    /// <summary>
    /// Operator zum subtrahieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntLimbs operator -(UIntLimbs val1, UIntLimbs val2)
    {
      return Sub(val1, val2);
    }

    /// <summary>
    /// dekrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche dekrementiert werden soll</param>
    /// <returns>fertig dekrementierte Zahl</returns>
    public static UIntLimbs operator --(UIntLimbs val)
    {
      return Sub(val, One);
    }

    /// <summary>
    /// Methode zum subtrahieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static UIntLimbs Sub(UIntLimbs val1, UIntLimbs val2)
    {
      var limbs1 = val1.limbs;
      var limbs2 = val2.limbs;
      if (limbs2.Length == 1 && limbs2[0] == 0) return val1; // kein Wert zum Subtrahieren vorhanden
      if (limbs2.Length > limbs1.Length) throw new InvalidCalcException();

      var result = new int[limbs1.Length];
      Array.Copy(limbs1, result, limbs1.Length);

      if (Sub(result, limbs2) != 0) throw new InvalidCalcException();

      return new UIntLimbs(SubNormalize(result));
    }

    /// <summary>
    /// kürzt einen Subtrahierten Wert (sofern notwendig)
    /// </summary>
    /// <param name="val">Wert, welcher normalisiert werden soll</param>
    /// <returns>fertiger normalisierter Wert</returns>
    static int[] SubNormalize(int[] val)
    {
      int last = val.Length - 1;
      while (last > 0 && val[last] == 0) last--;
      if (last == val.Length - 1) return val; // Wert kann beibehalten werden

      var result = new int[last + 1];
      Array.Copy(val, result, result.Length);
      return result;
    }

    /// <summary>
    /// subtrahiert zwei Zahlen-Arrays und ein eventuell vorhandenen Borrow-Wert zurück
    /// </summary>
    /// <param name="target">Basis-Wert, wovon der zweite Wert subtrahiert werden soll</param>
    /// <param name="sub">zweite Wert, welcher subtrahiert werden soll</param>
    /// <returns>1 = wenn Borrow-Flag gesetzt</returns>
    static int Sub(int[] target, int[] sub)
    {
      if (sub.Length > target.Length) throw new InvalidCalcException();

      int borrow = 0;

      // --- normale Subtraction ---
      for (int i = 0; i < sub.Length; i++)
      {
        var r = target[i] - sub[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i] = r + borrow * LimbSize;
      }

      // --- borrow-flag von den restlichen Zahlen subtrahieren (sofern notwendig) ---
      for (int i = sub.Length; borrow != 0 && i < target.Length; i++)
      {
        var r = target[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i] = r + borrow * LimbSize;
      }

      return borrow;
    }

    /// <summary>
    /// subtrahiert zwei Zahlen-Arrays und ein eventuell vorhandenen Borrow-Wert zurück
    /// </summary>
    /// <param name="target">Basis-Wert, wovon der zweite Wert subtrahiert werden soll</param>
    /// <param name="sub">zweite Wert, welcher subtrahiert werden soll</param>
    /// <param name="subOffset">zusätzlicher Offset</param>
    /// <returns>1 = wenn Borrow-Flag gesetzt</returns>
    static int Sub(int[] target, int[] sub, int subOffset)
    {
      if (sub.Length + subOffset > target.Length) throw new InvalidCalcException();

      int borrow = 0;

      // --- normale Subtraction ---
      for (int i = 0; i < sub.Length; i++)
      {
        var r = target[i + subOffset] - sub[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i + subOffset] = r + borrow * LimbSize;
      }

      // --- borrow-flag von den restlichen Zahlen subtrahieren (sofern notwendig) ---
      for (int i = sub.Length + subOffset; borrow != 0 && i < target.Length; i++)
      {
        var r = target[i] - borrow;
        borrow = (int)((uint)r >> 31);
        target[i] = r + borrow * LimbSize;
      }

      return borrow;
    }
  }
}
