using System;
using System.Numerics;
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// wandelt 
    /// </summary>
    /// <param name="bits"></param>
    /// <returns></returns>
    static BigInteger GetOldFromBits(uint[] bits)
    {
      var tmp = new byte[bits.Length * sizeof(uint) + 1];
      Buffer.BlockCopy(bits, 0, tmp, 0, bits.Length * sizeof(uint));
      return new BigInteger(tmp);
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette zurück
    /// </summary>
    /// <returns>lesbare Zeichenkette</returns>
    public override string ToString()
    {
      return GetOldFromBits(bits).ToString(); // todo -> langsam
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette mit Tausender-Punkten zurück
    /// </summary>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll (default: ',')</param>
    /// <returns>lesbare Zeichenkette</returns>
    public unsafe string ToStringThousandsSep(char sepChar = ',')
    {
      string num = ToString();
      int len = num.Length + (num.Length - 1) / 3;
      string tmp = UnsafeHelper.FastAllocateString(len);
      fixed (char* tmpP = tmp)
      {
        var tmpPp = tmpP + len - 1;
        fixed (char* numP = num)
        {
          var dg = numP + num.Length - 1;
          for (int i = 0; i < len; i++)
          {
            tmpPp[-i] = (i & 3) == 3 ? sepChar : *dg--;
          }
        }
      }
      return tmp;
    }
  }
}
