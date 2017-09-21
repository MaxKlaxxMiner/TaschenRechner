using System;
using TaschenRechnerLib.BigIntegerExtras;
using TaschenRechnerLib.Core;

namespace TaschenRechnerLib
{
  public unsafe partial struct UIntXb
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntXb operator +(UIntXb val1, UIntXb val2)
    {
      if (val2.IsZero) return val1;
      if (val1.IsZero) return val2;

      if (val1.limbCount < val2.limbCount) { var tmp = val1; val1 = val2; val2 = tmp; }

      //var bb = new BigIntegerBuilder(val1);
      //var reg = new BigIntegerBuilder(val2);
      //bb.Add(ref reg);

      //return bb.GetUIntXb();

      //int preCarry = val1.limbCount == val2.limbCount && (ulong)val1.limbs[val1.limbCount - 1] + val2.limbs[val2.limbCount - 1] + 1 > uint.MaxValue ? 1 : (ulong)val1.limbs[val1.limbCount - 1] >= uint.MaxValue ? 1 : 0;
      int preCarry = 1;
      var result = new uint[val1.limbCount + preCarry];

      long addLen = val2.limbCount;
      fixed (uint* target = result, l1 = val1.limbs, l2 = val2.limbs)
      {
        var carry = Xtr.Add(target, l1, l2, addLen);
        while (carry != 0)
        {
          carry = l1[addLen] + carry;
          target[addLen] = (uint)carry;
          addLen++;
          carry >>= 32;
        }
        if (val1.limbCount - addLen > 0)
        {
          Xtr.CopyLimbs(l1 + addLen, target + addLen, val1.limbCount - addLen);
          addLen = val1.limbCount;
        }

        //if (carry == 0)
        //{
        //  Xtr.CopyLimbs(l1 + addLen, target + addLen, val1.limbCount - addLen);
        //  addLen = val1.limbCount;
        //}
        //else
        //{
        //  addLen += AddCarry(target + addLen, l1 + addLen, val1.limbCount - addLen, carry);
        //}
      }

      return new UIntXb(result, addLen);
    }

    static long AddCarry(uint* target, uint* val, long minlen, ulong carry)
    {
      for (long count = 0; count < minlen; count++)
      {
        carry = val[count] + carry;
        target[count] = (uint)carry;
        carry >>= 32;
      }
      target[minlen] = (uint)carry;
      return minlen + (long)carry;
    }

    /// <summary>
    /// inkrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche inkrementiert werden soll</param>
    /// <returns>fertig inkrementierte Zahl</returns>
    public static UIntXb operator ++(UIntXb val)
    {
      var bb = new BigIntegerBuilder(val);
      bb.Add(1);
      return bb.GetUIntXb();
    }
  }
}
