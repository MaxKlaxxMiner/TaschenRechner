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
    static unsafe UIntLimbs Mul(UIntLimbs val1, UIntLimbs val2)
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
      fixed (int* resultP = result, limbs1P = limbs1, limbs2P = limbs2)
      {
        MulInternal(resultP, limbs1P, limbs1.Length, limbs2P, limbs2.Length);
      }

      return new UIntLimbs(SubNormalize(result));
    }

    static unsafe void MulInternal(int* result, int* limbs1, int limbs1Len, int* limbs2, int limbs2Len)
    {
      for (int y = 0; y < limbs2Len; y++)
      {
        MulInternalLine(result + y, limbs1, limbs1Len, limbs2[y]);
      }
    }

    static unsafe void MulInternalLine(int* result, int* limbs1, int limbs1Len, int limb2)
    {
      long carry = 0;
      for (int x = 0; x < limbs1Len; x++)
      {
        long r = *result + carry + limbs1[x] * (long)limb2;
        carry = r / LimbSize;
        *result++ = (int)(r - carry * LimbSize);
      }
      for (; carry != 0; result++)
      {
        long r = *result + carry;
        carry = r / LimbSize;
        *result++ = (int)(r - carry * LimbSize);
      }
    }
  }
}
