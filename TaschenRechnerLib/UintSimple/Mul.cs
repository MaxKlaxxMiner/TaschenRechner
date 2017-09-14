using System;

namespace TaschenRechnerLib
{
  public partial struct UIntSimple
  {
    /// <summary>
    /// Operator zum multiplizieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntSimple operator *(UIntSimple val1, UIntSimple val2)
    {
      return Mul(val1, val2);
    }

    /// <summary>
    /// Methode zum multiplizieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static unsafe UIntSimple Mul(UIntSimple val1, UIntSimple val2)
    {
      var digits1 = val1.digits;
      var digits2 = val2.digits;
      if (digits1.Length > digits2.Length) { var tmp = digits1; digits1 = digits2; digits2 = tmp; }

      if (digits2.Length == 1) // Einstellige Sonderfälle abfangen
      {
        switch (digits2[0])
        {
          case 0: return Zero; // x * 0 = 0
          case 1: return new UIntSimple(digits1); // x * 1 = x
          case 2: return new UIntSimple(digits1) + new UIntSimple(digits1); // x * 2 = x + x
        }
      }

      // --- multiplizieren nach Schulmethode ---
      var sum = new int[digits1.Length + digits2.Length];
      fixed (int* sumP = sum)
      {
        MulInternal(digits1, digits2, sumP);
      }

      // --- Ergebnisse zusammenrechnen ---
      var result = new byte[sum.Length];
      int carry = 0;
      for (int i = 0; i < result.Length; i++)
      {
        int r = sum[i] + carry;
        carry = UnsafeHelper.Div10(r);
        result[i] = (byte)(r - carry * 10);
      }
      if (carry != 0) throw new InvalidCalcException();

      return new UIntSimple(SubNormalize(result));
    }

    static unsafe void MulInternal(byte[] digits1, byte[] digits2, int* sum)
    {
      for (int y = 0; y < digits2.Length; y++)
      {
        var sumP = sum + y;
        int mul = digits2[y];
        switch (mul)
        {
          case 0: break;
          case 1: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x]; break;
          case 2: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] << 1; break;
          case 3: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] * mul; break;
          case 4: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] << 2; break;
          case 5: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] * mul; break;
          case 6: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] * mul; break;
          case 7: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] * mul; break;
          case 8: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] << 3; break;
          case 9: for (int x = 0; x < digits1.Length; x++) sumP[x] += digits1[x] * mul; break;
          default: throw new Exception();
        }
      }
    }
  }
}
