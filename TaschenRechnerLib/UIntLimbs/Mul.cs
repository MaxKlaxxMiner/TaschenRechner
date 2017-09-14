using System;

namespace TaschenRechnerLib
{
  public partial struct UIntLimbs
  {
    /// <summary>
    /// Operator zum multiplizieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntLimbs operator *(UIntLimbs val1, UIntLimbs val2)
    {
      return Mul(val1, val2);
    }

    /// <summary>
    /// Methode zum multiplizieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static UIntLimbs Mul(UIntLimbs val1, UIntLimbs val2)
    {
      var limbs1 = val1.limbs;
      var limbs2 = val2.limbs;
      if (limbs1.Length > limbs2.Length) { var tmp = limbs1; limbs1 = limbs2; limbs2 = tmp; }

      if (limbs2.Length == 1) // Einstellige Sonderfälle abfangen
      {
        switch (limbs2[0])
        {
          case 0: return Zero; // x * 0 = 0
          case 1: return new UIntLimbs(limbs1); // x * 1 = x
          case 2: return new UIntLimbs(limbs1) + new UIntLimbs(limbs1); // x * 2 = x + x
        }
      }

      // --- multiplizieren nach Schulmethode ---
      var result = new int[limbs1.Length + limbs2.Length];
      for (int y = 0; y < limbs2.Length; y++)
      {
        long carry = 0;
        for (int x = 0; x < limbs1.Length; x++)
        {
          long r = result[x + y] + carry + limbs1[x] * (long)limbs2[y];
          carry = r / LimbSize;
          result[x + y] = (int)(r - carry * LimbSize);
        }
        for (int c = limbs1.Length + y; carry != 0; c++)
        {
          long r = result[c] + carry;
          carry = r / LimbSize;
          result[c] = (int)(r - carry * LimbSize);
        }
      }

      return new UIntLimbs(SubNormalize(result));
    }
  }
}
