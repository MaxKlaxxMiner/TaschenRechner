namespace TaschenRechnerLib
{
  public unsafe partial struct UIntXs
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntXs operator +(UIntXs val1, UIntXs val2)
    {
      if (val1.limbCount < val2.limbCount) { var tmp = val1; val1 = val2; val2 = tmp; }
      if (val2.limbs[0] == 0 && val2.limbCount == 1) return val1;

      var result = new uint[val1.limbCount + 1];

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
          addLen += Xtr.CopyLimbs(l1 + addLen, target + addLen, val1.limbCount - addLen);
        }
      }

      return new UIntXs(result, addLen);
    }

    /// <summary>
    /// inkrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche inkrementiert werden soll</param>
    /// <returns>fertig inkrementierte Zahl</returns>
    public static UIntXs operator ++(UIntXs val)
    {
      var result = new uint[val.limbCount + 1];
      fixed (uint* target = result, src = val.limbs)
      {
        Xtr.CopyLimbs(src, target, val.limbCount);
        ulong carry = 1;
        long len = 0;
        while (carry != 0)
        {
          carry = target[len] + carry;
          target[len] = (uint)carry;
          len++;
          carry >>= 32;
        }
        return new UIntXs(result, val.limbCount + target[val.limbCount]);
      }
    }
  }
}
