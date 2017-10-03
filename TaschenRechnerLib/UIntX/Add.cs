namespace TaschenRechnerLib
{
  public unsafe partial struct UIntX
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntX operator +(UIntX val1, UIntX val2)
    {
      if (val1.limbCount < val2.limbCount) { var tmp = val1; val1 = val2; val2 = tmp; }
      if (val2.limbs[0] == 0 && val2.limbCount == 1) return val1;

      var result = new ulong[val1.limbCount + 1];

      long addLen = val2.limbCount;
      fixed (ulong* target = result, l1 = val1.limbs, l2 = val2.limbs)
      {
        var carry = AsmWrapper.UIntX_Add(target, l1, l2, addLen);
        while (carry != 0)
        {
          ulong ul = l1[addLen];
          ulong vl = target[addLen];
          ulong sl = ul + vl;
          ulong rl = sl + carry;
          target[addLen] = rl;
          carry = sl < ul || rl < sl ? 1UL : 0UL;
          addLen++;
        }
        if (val1.limbCount - addLen > 0)
        {
          AsmWrapper.UIntX_Copy(target + addLen, l1 + addLen, val1.limbCount - addLen);
          addLen = val1.limbCount;
        }
      }

      return new UIntX(result, addLen);
    }

    /// <summary>
    /// inkrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche inkrementiert werden soll</param>
    /// <returns>fertig inkrementierte Zahl</returns>
    public static UIntX operator ++(UIntX val)
    {
      var result = new ulong[val.limbCount + 1];
      fixed (ulong* target = result, src = val.limbs)
      {
        AsmWrapper.UIntX_Copy(target, src, val.limbCount);
        ulong carry = 1;
        long len = 0;
        while (carry != 0)
        {
          ulong sl = target[len];
          ulong rl = sl + carry;
          target[len] = rl;
          carry = rl < sl ? 1UL : 0UL;
          len++;
        }
        return new UIntX(result, val.limbCount + (long)target[val.limbCount]);
      }
    }
  }
}
