using System;
using System.Diagnostics;
using System.Numerics;
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// wandelt eine Bit-Kette in das alte BigInteger-Format um
    /// </summary>
    /// <param name="bits">Bit-Kette, weleche verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static BigInteger GetOldFromBits(uint[] bits)
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
    static uint[] GetBitsFromOld(BigInteger number)
    {
      Debug.Assert(number.Sign >= 0);
      var val = number.ToByteArray();
      var tmp = new uint[(val.Length + sizeof(uint) - 1) / sizeof(uint)];
      Buffer.BlockCopy(val, 0, tmp, 0, val.Length);
      return tmp;
    }
  }
}
