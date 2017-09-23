using System;

namespace TaschenRechnerLib
{
  public sealed unsafe partial class UIntXm
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntXm operator +(UIntXm val1, UIntXm val2)
    {
      if (val1 == null || val2 == null) throw new ArgumentNullException();

      if (val2.IsZero) return val1;
      if (val1.IsZero) return val2;

      if (val1.limbsCount < val2.limbsCount) { var tmp = val1; val1 = val2; val2 = tmp; }

      var target = AllocLimbs(val1.limbsCount + 1);

      long addLen = val2.limbsCount;
      var carry = Xtr.Add(target, val1.limbs, val2.limbs, addLen);
      if (carry == 0)
      {
        Xtr.CopyLimbs(val1.limbs + addLen, target + addLen, val1.limbsCount - addLen);
        addLen = val1.limbsCount;
      }
      else
      {
        addLen += AddCarry(target + addLen, val1.limbs + addLen, val1.limbsCount - addLen, carry);
      }

      return new UIntXm(target, addLen);
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
    public static UIntXm operator ++(UIntXm val)
    {
      if (val == null) throw new ArgumentNullException();

      if (*val.limbs < uint.MaxValue) // schnelle direkte Variante möglich?
      {
        (*val.limbs)++;
        return val;
      }

      return val + One;
    }
  }
}
