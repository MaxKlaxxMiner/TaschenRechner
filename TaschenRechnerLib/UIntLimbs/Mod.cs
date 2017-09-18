#region # using *.*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaschenRechnerLib.BigIntegerExtras;

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
    /// Operator zum berechnen des Modulus zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntLimbs operator %(UIntLimbs val1, UIntLimbs val2)
    {
      return Mod(val1, val2);
    }

    /// <summary>
    /// merkt sich alle gültigen Modulos von LimbSize (1.000.000.000)
    /// </summary>
    static readonly HashSet<int> DirectMod = new HashSet<int>(GenDirectMod().OrderBy(x => x));

    /// <summary>
    /// generiert alle gültigen Modulos von LimbSize (1.000.000.000)
    /// </summary>
    /// <returns>alle Modulos von LimbSize</returns>
    static IEnumerable<int> GenDirectMod()
    {
      var sended = new HashSet<int>(Enumerable.Range(0, 10).Select(i => 1 << i));

      var mul = new List<int> { 5 };
      mul.AddRange(sended);
      mul.Sort();
      for (int y = 0; y < mul.Count; y++)
      {
        for (int x = 0; x < mul.Count; x++)
        {
          long val = (long)mul[x] * mul[y];
          if (val >= LimbSize || LimbSize % (int)val != 0 || sended.Contains((int)val)) continue;
          yield return (int)val;
          mul.Add((int)val);
          sended.Add((int)val);
          if (mul.Count == 100) yield break; // alle gefunden
        }
      }
    }

    /// <summary>
    /// führt eine Modulo-Berechnung durch
    /// </summary>
    /// <param name="val">Dividend, welcher verwendet werden soll</param>
    /// <param name="div">Divisor, welcher zum teilen verwendet werden soll</param>
    /// <returns>Ergebnis: Rest</returns>
    static UIntLimbs Mod(UIntLimbs val, UIntLimbs div)
    {
      if (div.limbs.Length == 1)
      {
        switch (div.limbs[0])
        {
          case 0: throw new DivideByZeroException();
          case 1: return Zero; // x % 1 = 0
          case 2: return new UIntLimbs(new[] { val.limbs[0] & 1 }); // x % 2 = x & 1
          case 4: return new UIntLimbs(new[] { val.limbs[0] & 3 }); // x % 4 = x & 3
          case 8: return new UIntLimbs(new[] { val.limbs[0] & 7 }); // x % 8 = x & 7
          case 16: return new UIntLimbs(new[] { val.limbs[0] & 15 }); // x % 16 = x & 15
          case 32: return new UIntLimbs(new[] { val.limbs[0] & 31 }); // x % 32 = x & 31
          case 64: return new UIntLimbs(new[] { val.limbs[0] & 63 }); // x % 64 = x & 63
          case 128: return new UIntLimbs(new[] { val.limbs[0] & 127 }); // x % 128 = x & 127
          case 256: return new UIntLimbs(new[] { val.limbs[0] & 255 }); // x % 256 = x & 255
          case 512: return new UIntLimbs(new[] { val.limbs[0] & 511 }); // x % 512 = x & 511
        }
        if (DirectMod.Contains(div.limbs[0])) return new UIntLimbs(new[] { val.limbs[0] % div.limbs[0] });
      }
      if (div.limbs.Length > val.limbs.Length) return val; // zu kleines Ergebnis (gesamter Dividend bleibt als Rest übrig)

      var remainder = new int[val.limbs.Length];
      Array.Copy(val.limbs, remainder, remainder.Length);
      var quotient = new int[val.limbs.Length - div.limbs.Length + 1];

      DivModInternal(remainder, div.limbs, quotient);

      return new UIntLimbs(SubNormalize(remainder));
    }
  }
}
