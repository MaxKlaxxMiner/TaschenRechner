using System;
using TaschenRechnerLib.BigIntegerExtras;

namespace TaschenRechnerLib
{
  public sealed unsafe partial class UIntX
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntX operator +(UIntX val1, UIntX val2)
    {
      if (val1 == null || val2 == null) throw new ArgumentNullException();

      if (val2.IsZero) return val1;
      if (val1.IsZero) return val2;

      if (val1.limbsCount < val2.limbsCount) { var tmp = val1; val1 = val2; val2 = tmp; }

      var target = AllocLimbs(val1.limbsCount + 1);

      long addLen = val2.limbsCount;
      var carry = Add(target, val1.limbs, val2.limbs, addLen);
      while (carry != 0 && addLen < val1.limbsCount)
      {
        carry = (ulong)val1.limbs[addLen] + val2.limbs[addLen] + carry;
        target[addLen++] = (uint)carry;
        carry = carry >> 32;
      }
      if (carry == 0) addLen = val1.limbsCount; else target[addLen++] = (uint)carry;

      return new UIntX(target, addLen);
    }

    static ulong Add(uint* target, uint* val1, uint* val2, long count)
    {
      long i = 0;
      ulong carry = 0;
      for (i = 0; i < count - 1; i += 2)
      {
        ulong v1 = *(ulong*)(val1 + i);
        ulong v2 = *(ulong*)(val2 + i);
        carry = (ulong)(uint)v1 + (uint)v2 + (carry >> 32);
        target[i] = (uint)carry;
        carry = (v1 >> 32) + (v2 >> 32) + (carry >> 32);
        target[i + 1] = (uint)carry;
      }
      for (; i < count; i++)
      {
        carry = (ulong)val1[i] + val2[i] + (carry >> 32);
        target[i] = (uint)carry;
      }
      return (uint)(carry >> 32);
    }

    /// <summary>
    /// inkrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche inkrementiert werden soll</param>
    /// <returns>fertig inkrementierte Zahl</returns>
    public static UIntX operator ++(UIntX val)
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
