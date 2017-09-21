using TaschenRechnerLib.BigIntegerExtras;
using TaschenRechnerLib.Core;

// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public unsafe partial class UIntX
  {
    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette zurück
    /// </summary>
    /// <returns>lesbare Zeichenkette</returns>
    public override string ToString()
    {
      var bits = new uint[limbsCount];
      Xtr.CopyLimbs(limbs, bits, limbsCount);
      return BigIntegerHelpers.GetOldFromBits(bits).ToString(); // todo -> langsam
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette mit Tausender-Punkten zurück
    /// </summary>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll (default: ',')</param>
    /// <returns>lesbare Zeichenkette</returns>
    public string ToStringThousandsSep(char sepChar = ',')
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
