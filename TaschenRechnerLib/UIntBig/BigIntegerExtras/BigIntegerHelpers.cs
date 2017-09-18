using System;
using System.Diagnostics;
using System.Numerics;

namespace TaschenRechnerLib.BigIntegerExtras
{
  internal static class BigIntegerHelpers
  {
    /// <summary>
    /// wandelt eine Bit-Kette in das alte BigInteger-Format um
    /// </summary>
    /// <param name="bits">Bit-Kette, weleche verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static BigInteger GetOldFromBits(uint[] bits)
    {
      var tmp = new byte[bits.Length * sizeof(uint) + 1];
      Buffer.BlockCopy(bits, 0, tmp, 0, bits.Length * sizeof(uint));
      return new BigInteger(tmp);
    }

    /// <summary>
    /// wandelt das alte BigInteger-Format in eine Bit-Kette um
    /// </summary>
    /// <param name="number">BigInteger-Nummer, welche umgewandelt werden soll (muss positiv oder 0 sein)</param>
    /// <returns>fertige Bit-Kette</returns>
    public static uint[] GetBitsFromOld(BigInteger number)
    {
      Debug.Assert(number.Sign >= 0);
      var val = number.ToByteArray();
      var tmp = new uint[(val.Length + sizeof(uint) - 1) / sizeof(uint)];
      Buffer.BlockCopy(val, 0, tmp, 0, val.Length);
      while (tmp.Length > 1 && tmp[tmp.Length - 1] == 0) Array.Resize(ref tmp, tmp.Length - 1); // normalisieren
      return tmp;
    }

    /// <summary>
    /// gibt die Position von zwei Bit-Ketten zurück, wo die sich unterscheiden (-1 = beide sind gleich)
    /// </summary>
    /// <param name="rgu1">erste Bit-Kette</param>
    /// <param name="rgu2">zweite Bit-Kette</param>
    /// <param name="cu">Start-Position</param>
    /// <returns></returns>
    public static int GetDiffLength(uint[] rgu1, uint[] rgu2, int cu)
    {
      for (int iv = cu; --iv >= 0; )
      {
        if (rgu1[iv] != rgu2[iv]) return iv + 1;
      }
      return 0;
    }

    /// <summary>
    /// gibt die Anzahl der freien 0-Bits zurück
    /// </summary>
    /// <param name="u">Wert, welcher berechnet werden soll</param>
    /// <returns>Anzahl der 0-Bits</returns>
    public static int CbitHighZero(uint u)
    {
      if ((int)u == 0) return 32;
      int num = 0;
      if (((int)u & -65536) == 0)
      {
        num += 16;
        u <<= 16;
      }
      if (((int)u & -16777216) == 0)
      {
        num += 8;
        u <<= 8;
      }
      if (((int)u & -268435456) == 0)
      {
        num += 4;
        u <<= 4;
      }
      if (((int)u & -1073741824) == 0)
      {
        num += 2;
        u <<= 2;
      }
      if (((int)u & int.MinValue) == 0) ++num;
      return num;
    }
  }
}
