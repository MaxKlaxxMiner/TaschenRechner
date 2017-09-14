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
      var sum = new ulong[limbs1.Length + limbs2.Length];
      int lim = 19;
      for (int y = 0; y < limbs2.Length; y++)
      {
        for (int x = 0; x < limbs1.Length; x++)
        {
          if (--lim == 0)
          {
            ulong mc = 0;
            for (int i = 0; i < sum.Length; i++)
            {
              ulong r = sum[i] + mc;
              mc = r / LimbSize;
              sum[i] = r - mc * LimbSize;
            }
            lim = 19;
          }
          sum[x + y] += (ulong)(limbs1[x] * (long)limbs2[y]);
        }
      }

      // --- Ergebnisse zusammenrechnen ---
      var result = new int[sum.Length];
      ulong carry = 0;
      for (int i = 0; i < result.Length; i++)
      {
        ulong r = sum[i] + carry;
        carry = r / LimbSize;
        result[i] = (int)(r - carry * LimbSize);
      }
      if (carry != 0) throw new InvalidCalcException();

      return new UIntLimbs(SubNormalize(result));
    }
  }
}
