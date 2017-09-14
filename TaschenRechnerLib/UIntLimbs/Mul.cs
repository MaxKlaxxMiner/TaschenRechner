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
    /// gibt an, ob der Karatsuba-Algorythmus benutzt werden soll (bei aufwendigen Multiplikationen)
    /// </summary>
    const bool UseKaratsuba = true;

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
      if (limbs2.Length > limbs1.Length) { var tmp = limbs1; limbs1 = limbs2; limbs2 = tmp; }

      if (limbs2.Length == 1) // Einstellige Sonderfälle abfangen
      {
        switch (limbs2[0])
        {
          case 0: return Zero; // x * 0 = 0
          case 1: return new UIntLimbs(limbs1); // x * 1 = x
          case 2: return new UIntLimbs(limbs1) + new UIntLimbs(limbs1); // x * 2 = x + x
        }
      }

      if (UseKaratsuba && limbs2.Length > 16)
      {
        // --- multiplizieren nach Karatsuba ---
        return new UIntLimbs(SubNormalize(MulKaratsuba(limbs1, limbs2)));
      }
      else
      {
        // --- multiplizieren nach Schulmethode ---
        var resultOld = new int[limbs1.Length + limbs2.Length];
        fixed (int* resultP = resultOld, limbs1P = limbs1, limbs2P = limbs2)
        {
          MulInternal(resultP, limbs1P, limbs1.Length, limbs2P, limbs2.Length);
        }
        return new UIntLimbs(SubNormalize(resultOld));
      }
    }

    static int[] MulKaratsuba(int[] limbs1, int[] limbs2)
    {
      int kLen = (Math.Max(limbs1.Length, limbs2.Length) + 1) / 2;
      Array.Resize(ref limbs1, kLen * 2);
      Array.Resize(ref limbs2, kLen * 2);

      var result = new int[kLen * 4];

      MulKaratsubaInternal(result, limbs1, limbs2, kLen);

      return result;
    }

    static unsafe void MulKaratsubaInternal(int[] result, int[] limbs1, int[] limbs2, int kLen)
    {
      if (result.Length < 64) // zu klein für Karatsuba? -> Schulmethode verwendet
      {
        fixed (int* resultP = result, limbs1P = limbs1, limbs2P = limbs2)
        {
          MulInternal(resultP, limbs1P, limbs1.Length, limbs2P, limbs2.Length);
        }
        return;
      }

      int kLenNext = (kLen + 2) / 2;
      var xh = new int[kLenNext * 2];
      Array.Copy(limbs1, kLen, xh, 0, kLen);
      var yh = new int[kLenNext * 2];
      Array.Copy(limbs2, kLen, yh, 0, kLen);
      var xl = new int[kLenNext * 2];
      Array.Copy(limbs1, 0, xl, 0, kLen);
      var yl = new int[kLenNext * 2];
      Array.Copy(limbs2, 0, yl, 0, kLen);
      var p1 = new int[kLenNext * 4];
      MulKaratsubaInternal(p1, xh, yh, kLenNext);
      var p2 = new int[kLenNext * 4];
      MulKaratsubaInternal(p2, xl, yl, kLenNext);
      if (Add(xh, xl) != 0) throw new InvalidCalcException();
      if (Add(yh, yl) != 0) throw new InvalidCalcException();
      var p3 = new int[kLenNext * 4];
      MulKaratsubaInternal(p3, xh, yh, kLenNext);

      Array.Copy(p2, result, p2.Length);
      Array.Copy(p1, 0, result, kLen * 2, kLen * 2);
      if (Add(result, p3, kLen) != 0) throw new InvalidCalcException();
      if (Add(p1, p2) != 0) throw new InvalidCalcException();
      if (Sub(result, p1, kLen) != 0) throw new InvalidCalcException();
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
