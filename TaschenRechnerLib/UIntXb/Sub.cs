using System;
using TaschenRechnerLib.BigIntegerExtras;
using TaschenRechnerLib.Core;

namespace TaschenRechnerLib
{
  public unsafe partial struct UIntXb
  {
    /// <summary>
    /// Operator zum subtrahieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntXb operator -(UIntXb val1, UIntXb val2)
    {
      if (val2.limbs[0] == 0 && val2.limbCount == 1) return val1;
      if (val1.limbCount < val2.limbCount) throw new InvalidCalcException();
      if (val1.limbs[0] == 0 && val1.limbCount == 1) throw new InvalidCalcException();

      fixed (uint* l1 = val1.limbs, l2 = val2.limbs)
      {
        if (val1.limbCount == val2.limbCount)
        {
          long len = Xtr.DiffLen(l1, l2, val1.limbCount);
          if (len == 0) return Zero;

          var result = new uint[len];

          fixed (uint* target = result)
          {
            var borrow = Xtr.Sub(target, l1, l2, len);
            if (borrow != 0) throw new InvalidCalcException();
          }

          return new UIntXb(result, len);
        }
        else
        {
          var result = new uint[val1.limbCount];
          fixed (uint* target = result)
          {
            long subLen = val2.limbCount;
            var borrow = Xtr.Sub(target, l1, l2, subLen);
            while (borrow != 0)
            {
              borrow = l1[subLen] - borrow;
              target[subLen] = (uint)borrow;
              subLen++;
              borrow >>= 63;
            }
            if (val1.limbCount - subLen > 0)
            {
              subLen += Xtr.CopyLimbs(l1 + subLen, target + subLen, val1.limbCount - subLen);
            }
            else while (subLen > 1 && target[subLen - 1] == 0) subLen--;
            return new UIntXb(result, subLen);
          }
        }
      }
    }

    /// <summary>
    /// dekrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche dekrementiert werden soll</param>
    /// <returns>fertig dekrementierte Zahl</returns>
    public static UIntXb operator --(UIntXb val)
    {
      //if (val.limbs[0] == 0 && val.limbCount == 1) throw new InvalidCalcException();
      //var bb = new BigIntegerBuilder(val);
      //bb.Sub(1);
      //return bb.GetUIntXb();

      var result = new uint[val.limbCount];
      fixed (uint* target = result, src = val.limbs)
      {
        Xtr.CopyLimbs(src, target, val.limbCount);

        ulong borrow = 1;
        long len = 0;
        while (borrow != 0)
        {
          borrow = target[len] - borrow;
          target[len] = (uint)borrow;
          len++;
          borrow >>= 63;
          if (len == val.limbCount)
          {
            if (borrow != 0) throw new InvalidCalcException();
            return new UIntXb(result, Math.Max(1, val.limbCount - 1));
          }
        }
        return new UIntXb(result, val.limbCount);
      }
    }
  }
}
