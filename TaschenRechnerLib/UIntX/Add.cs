namespace TaschenRechnerLib
{
  public unsafe partial struct UIntX
  {
    #region # // --- Überladungen ---
    /// <summary>
    /// inkrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche inkrementiert werden soll</param>
    /// <returns>fertig inkrementierte Zahl</returns>
    public static UIntX operator ++(UIntX val)
    {
      return val + 1UL;
    }

    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntX operator +(UIntX val1, byte val2)
    {
      return val1 + (ulong)val2;
    }

    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntX operator +(UIntX val1, ushort val2)
    {
      return val1 + (ulong)val2;
    }

    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntX operator +(UIntX val1, uint val2)
    {
      return val1 + (ulong)val2;
    }
    #endregion

    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntX operator +(UIntX val1, ulong val2)
    {
      var result = new ulong[val1.limbCount + 1];
      fixed (ulong* rp = result, up = val1.limbs)
      {
        AsmWrapper.UIntX_Copy(rp, up, val1.limbCount);
        ulong cy = val2;
        long len = 0;
        while (cy != 0)
        {
          ulong sl = rp[len];
          ulong rl = sl + cy;
          rp[len] = rl;
          cy = rl < sl ? 1UL : 0UL;
          len++;
        }
        return new UIntX(result, val1.limbCount + (long)rp[val1.limbCount]);
      }
    }

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

      long len = val2.limbCount;
      fixed (ulong* rp = result, up = val1.limbs, vp = val2.limbs)
      {
        var cy = AsmWrapper.UIntX_Add(rp, up, vp, len);
        while (cy != 0)
        {
          ulong ul = up[len];
          ulong vl = rp[len];
          ulong sl = ul + vl;
          ulong rl = sl + cy;
          rp[len] = rl;
          cy = sl < ul || rl < sl ? 1UL : 0UL;
          len++;
        }
        if (val1.limbCount - len > 0)
        {
          AsmWrapper.UIntX_Copy(rp + len, up + len, val1.limbCount - len);
          len = val1.limbCount;
        }
      }

      return new UIntX(result, len);
    }
  }
}
