using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace TaschenRechnerLib.BigIntegerExtras
{
  /// <summary>
  /// weitgehender Clone vom original BitInteger (interne Klasse)
  /// </summary>
  internal unsafe struct BigIntegerBuilder
  {
    int iuLast;
    uint uSmall;
    uint[] rgu;
    bool fWritable;

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="bn">BitInteger mit Bit-Kette, welche verwendet werden soll</param>
    public BigIntegerBuilder(UIntBig bn)
    {
      fWritable = false;
      rgu = bn.bits;
      int last = rgu.Length - 1;
      uSmall = rgu[0];
      while (last > 0 && rgu[last] == 0) last--;
      iuLast = last;
    }

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="reg">Referenz auf einen weiteren BigIntegerBuilder</param>
    public BigIntegerBuilder(ref BigIntegerBuilder reg)
    {
      this = reg;
      if (!fWritable) return;
      fWritable = false;
      if (iuLast == 0)
      {
        rgu = null;
      }
      else
      {
        reg.fWritable = false;
      }
    }

    /// <summary>
    /// lädt einen direkten Wert
    /// </summary>
    /// <param name="reg">BigIntegerBuilder, wovon der Wert kopiert werden soll</param>
    /// <param name="cuExtra">optional gewünschte Extra-Größe</param>
    public void Load(ref BigIntegerBuilder reg, int cuExtra = 0)
    {
      if (reg.iuLast == 0)
      {
        uSmall = reg.uSmall;
        iuLast = 0;
      }
      else
      {
        if (!fWritable || rgu.Length <= reg.iuLast)
        {
          rgu = new uint[reg.iuLast + 1 + cuExtra];
          fWritable = true;
        }
        iuLast = reg.iuLast;
        Array.Copy(reg.rgu, rgu, iuLast + 1);
      }
    }

    /// <summary>
    /// gibt die fertigen Bits des Wertes zurück
    /// </summary>
    /// <returns>fertige Bits</returns>
    uint[] GetIntegerParts()
    {
      if (iuLast == 0)
      {
        if (uSmall <= int.MaxValue) return new[] { uSmall };
        if (rgu == null) rgu = new[] { uSmall };
        else if (fWritable) rgu[0] = uSmall;
        else if (rgu[0] != uSmall) rgu = new[] { uSmall };
      }
      int len = rgu.Length - iuLast - 1;
      if (len <= 1)
      {
        if (len == 0 || (int)rgu[iuLast + 1] == 0)
        {
          fWritable = false;
          return rgu;
        }
        if (fWritable)
        {
          rgu[iuLast + 1] = 0;
          fWritable = false;
          return rgu;
        }
      }
      var bits = rgu;
      Array.Resize(ref bits, iuLast + 1);
      if (!fWritable) rgu = bits;
      return bits;
    }

    /// <summary>
    /// gibt den Wert wieder als neu erstelltes UBigInt zurück
    /// </summary>
    /// <returns></returns>
    public UIntBig GetInteger()
    {
      return new UIntBig(GetIntegerParts());
    }

    /// <summary>
    /// setzt die Größe des internen Buffers
    /// </summary>
    /// <param name="cu">Größe, welche eingestellt werden soll</param>
    void SetSizeLazy(int cu)
    {
      if (cu <= 1)
      {
        iuLast = 0;
      }
      else
      {
        if (!fWritable || rgu.Length < cu)
        {
          rgu = new uint[cu];
          fWritable = true;
        }
        iuLast = cu - 1;
      }
    }

    /// <summary>
    /// setzt die Größe des internen Buffers
    /// </summary>
    /// <param name="cu">Größe, welche eingestellt werden soll</param>
    /// <param name="cuExtra">zusätzliche optionale Größe</param>
    void SetSizeKeep(int cu, int cuExtra)
    {
      if (cu <= 1)
      {
        if (iuLast > 0) uSmall = rgu[0];
        iuLast = 0;
      }
      else
      {
        if (!fWritable || rgu.Length < cu)
        {
          var numArray = new uint[cu + cuExtra];
          if (iuLast == 0) numArray[0] = uSmall; else Array.Copy(rgu, numArray, Math.Min(cu, iuLast + 1));
          rgu = numArray;
          fWritable = true;
        }
        else if (iuLast + 1 < cu)
        {
          Array.Clear(rgu, iuLast + 1, cu - iuLast - 1);
          if (iuLast == 0) rgu[0] = uSmall;
        }
        iuLast = cu - 1;
      }
    }

    /// <summary>
    /// garantiert ein Schreibzugriff für weitere Operationen mit gewünschter Mindestgröße
    /// </summary>
    /// <param name="cu">gewünschte (wird garantiert)</param>
    /// <param name="cuExtra">optional zusätzlich gewünschte Größe (wird nicht garantiert)</param>
    public void EnsureWritable(int cu, int cuExtra)
    {
      if (fWritable && rgu.Length >= cu) return;
      var newArray = new uint[cu + cuExtra];
      if (iuLast > 0)
      {
        if (iuLast >= cu) iuLast = cu - 1;
        Array.Copy(rgu, newArray, iuLast + 1);
      }
      rgu = newArray;
      fWritable = true;
    }

    /// <summary>
    /// garantiert ein Schreibzugriff für weitere Operationen
    /// </summary>
    /// <param name="cuExtra">optional zusätzlich gewünschte Größe (wird nicht garantiert)</param>
    public void EnsureWritable(int cuExtra = 0)
    {
      if (fWritable) return;
      var newArray = new uint[iuLast + 1 + cuExtra];
      Array.Copy(rgu, newArray, iuLast + 1);
      rgu = newArray;
      fWritable = true;
    }

    /// <summary>
    /// kürz automatisch 0-Stellen weg
    /// </summary>
    void Trim()
    {
      if (iuLast > 0 && rgu[iuLast] == 0)
      {
        uSmall = rgu[0];
        while (--iuLast > 0 && rgu[iuLast] == 0) { }
      }
    }
    /// <summary>
    /// fügt ein Carry-Flag hinzu
    /// </summary>
    /// <param name="iu">Carryflag, welches hinzugefügt werden soll</param>
    void ApplyCarry(int iu)
    {
      for (; ; iu++)
      {
        if (iu > iuLast)
        {
          if (iuLast + 1 == rgu.Length) Array.Resize(ref rgu, iuLast + 2);
          rgu[++iuLast] = 1;
          break;
        }
        if (++rgu[iu] > 0) break;
      }
    }

    /// <summary>
    /// addiert einen bestimmten Wert inkl. Carry-Flag
    /// </summary>
    /// <param name="u1">Referenz auf den Ziel-Wert</param>
    /// <param name="u2">der zu addiernde Wert</param>
    /// <param name="uCarry">zusätzliches Carry-Flag</param>
    /// <returns>neues Carry-Flag</returns>
    static uint AddCarry(ref uint u1, uint u2, uint uCarry)
    {
      ulong r = (ulong)u1 + u2 + uCarry;
      u1 = (uint)r;
      return (uint)(r >> 32);
    }

    /// <summary>
    /// addiert einen bestimmten Wert
    /// </summary>
    /// <param name="u">Wert, welcher direkt addiert werden soll</param>
    public void Add(uint u)
    {
      if (iuLast == 0)
      {
        if ((uSmall = uSmall + u) >= u) return;
        SetSizeLazy(2);
        rgu[0] = uSmall;
        rgu[1] = 1;
      }
      else
      {
        if (u == 0) return;
        uint r = rgu[0] + u;
        if (r < u)
        {
          EnsureWritable(1);
          ApplyCarry(1);
        }
        else
        {
          if (!fWritable) EnsureWritable();
        }
        rgu[0] = r;
      }
    }

    /// <summary>
    /// addiert einen bestimmten Wert
    /// </summary>
    /// <param name="reg">weiterer BigIntegerBuilder, welcher als Wert benutzt werden soll</param>
    public void Add(ref BigIntegerBuilder reg)
    {
      if (reg.iuLast == 0)
      {
        Add(reg.uSmall);
      }
      else if (iuLast == 0)
      {
        uint u = uSmall;
        if ((int)u == 0)
        {
          this = new BigIntegerBuilder(ref reg);
        }
        else
        {
          Load(ref reg, 1);
          Add(u);
        }
      }
      else
      {
        EnsureWritable(Math.Max(iuLast, reg.iuLast) + 1, 1);
        int iu = reg.iuLast + 1;
        if (iuLast < reg.iuLast)
        {
          iu = iuLast + 1;
          Array.Copy(reg.rgu, iuLast + 1, rgu, iuLast + 1, reg.iuLast - iuLast);
          iuLast = reg.iuLast;
        }
        uint uCarry;

        // slow: for (int i = 0; i < iu; i++) uCarry = AddCarry(ref rgu[i], reg.rgu[i], uCarry);
        fixed (uint* targetP = rgu, srcP = reg.rgu)
        {
          uCarry = XtrAddCarry(targetP, srcP, iu);
        }

        if (uCarry == 0) return;
        ApplyCarry(iu);
      }
    }

    static uint XtrAddCarry(uint* target, uint* src, long count)
    {
      ulong r = 0;
      for (long i = 0; i < count; i++)
      {
        r = (ulong)target[i] + src[i] + (r >> 32);
        target[i] = (uint)r;
      }
      return (uint)(r >> 32);
    }

    /// <summary>
    /// zieht ein Borrow-Flag an einer bestimmten Position ab
    /// </summary>
    /// <param name="iuMin">Position, wo das Borrow-Flag abgezogen werden soll</param>
    void ApplyBorrow(int iuMin)
    {
      for (int iu = iuMin; iu <= iuLast; iu++)
      {
        uint u = rgu[iu]--;
        if (u > 0) return;
      }
    }

    /// <summary>
    /// subtrahiert einen bestimmten Wert
    /// </summary>
    /// <param name="u">Wert, welcher subtrahiert werden soll</param>
    public void Sub(uint u)
    {
      if (iuLast == 0)
      {
        if (u <= uSmall) uSmall -= u;
        else throw new InvalidCalcException();
      }

      if (u == 0) return;

      EnsureWritable();

      uint uTmp = rgu[0];
      rgu[0] = uTmp - u;
      if (uTmp < u)
      {
        ApplyBorrow(1);
        Trim();
      }
    }

    /// <summary>
    /// subtrahiert einen bestimmten Wert inkl. Borrow-Flag
    /// </summary>
    /// <param name="u1">erster Wert, wovon subtrahiert werden soll</param>
    /// <param name="u2">der Wert, welcher subtrahiert werden soll</param>
    /// <param name="uBorrow">zusätzliches Borrow-Flag</param>
    /// <returns>neues Borrow-Flag</returns>
    static uint SubBorrow(ref uint u1, uint u2, uint uBorrow)
    {
      ulong uu = (ulong)u1 - u2 - uBorrow;
      u1 = (uint)uu;
      return (uint)-(int)(uu >> 32);
    }

    /// <summary>
    /// subtrahiert einen bestimmten Wert
    /// </summary>
    /// <param name="reg">Wert, welcher subtrahiert werden soll</param>
    public void Sub(ref BigIntegerBuilder reg)
    {
      if (reg.iuLast == 0)
      {
        Sub(reg.uSmall);
        return;
      }
      if (iuLast < reg.iuLast) throw new InvalidCalcException();

      int cuSub = reg.iuLast + 1;
      if (iuLast == reg.iuLast)
      {
        // Determine which is larger.
        iuLast = BigIntegerHelpers.GetDiffLength(rgu, reg.rgu, iuLast + 1) - 1;
        if (iuLast < 0)
        {
          iuLast = 0;
          uSmall = 0;
          return;
        }
        uint u1 = rgu[iuLast];
        uint u2 = reg.rgu[iuLast];
        if (u1 < u2) throw new InvalidCalcException();
        if (iuLast == 0)
        {
          uSmall = u1 - u2;
          return;
        }

        cuSub = iuLast + 1;
      }

      EnsureWritable();

      // Subtract, tracking borrow.
      uint uBorrow = 0;
      for (int iu = 0; iu < cuSub; iu++)
      {
        uBorrow = SubBorrow(ref rgu[iu], reg.rgu[iu], uBorrow);
      }
      if (uBorrow != 0)
      {
        ApplyBorrow(cuSub);
      }
      Trim();
    }

    /// <summary>
    /// Multipliziert und Addiert direkt einen Wert
    /// </summary>
    /// <param name="uAdd">Wert, welcher geändert werden soll</param>
    /// <param name="uMul1">erster Faktor</param>
    /// <param name="uMul2">zweiter Faktor</param>
    /// <param name="uCarry">vorheriges Carry-Flag</param>
    /// <returns>neues Carry-Flag</returns>
    static uint AddMulCarry(ref uint uAdd, uint uMul1, uint uMul2, uint uCarry)
    {
      ulong num = (ulong)uMul1 * uMul2 + uAdd + uCarry;
      uAdd = (uint)num;
      return (uint)(num >> 32);
    }

    /// <summary>
    /// setzt direkt einen bestimmten uint-Wert
    /// </summary>
    /// <param name="u">Wert, welcher gesetzt werden soll</param>
    public void Set(uint u)
    {
      uSmall = u;
      iuLast = 0;
    }

    /// <summary>
    /// setzt direkt einen bestimmten ulong-Wert
    /// </summary>
    /// <param name="uu">Wert, welcher gesetzt werden soll</param>
    public void Set(ulong uu)
    {
      uint hi = (uint)(uu >> 32);
      if ((int)hi == 0)
      {
        uSmall = (uint)uu;
        iuLast = 0;
      }
      else
      {
        SetSizeLazy(2);
        rgu[0] = (uint)uu;
        rgu[1] = hi;
      }
    }

    /// <summary>
    /// multipliziert einen direkten Wert inkl Carry-Flag
    /// </summary>
    /// <param name="u1">Wert, welcher geändert werden soll</param>
    /// <param name="u2">der zu multipliziernde Faktor</param>
    /// <param name="uCarry">zusätzliches Carry-Flag</param>
    /// <returns>neues Carry-Flag</returns>
    static uint MulCarry(ref uint u1, uint u2, uint uCarry)
    {
      ulong num = (ulong)u1 * u2 + uCarry;
      u1 = (uint)num;
      return (uint)(num >> 32);
    }

    /// <summary>
    /// multipiziert einen direkten Wert
    /// </summary>
    /// <param name="u">Wert, welcher multipliziert werden soll</param>
    public void Mul(uint u)
    {
      if (u == 0)
      {
        Set(0u);
      }
      else
      {
        if (u == 1) return;
        if (iuLast == 0)
        {
          Set(uSmall * (ulong)u);
        }
        else
        {
          EnsureWritable(1);
          uint uCarry = 0U;
          for (int index = 0; index <= iuLast; ++index) uCarry = MulCarry(ref rgu[index], u, uCarry);
          if ((int)uCarry == 0) return;
          SetSizeKeep(iuLast + 2, 0);
          rgu[iuLast] = uCarry;
        }
      }
    }

    /// <summary>
    /// multipliziert diesen Wert
    /// </summary>
    /// <param name="regMul">Wert, welcher für die Multiplikation verwendet werden soll</param>
    public void Mul(ref BigIntegerBuilder regMul)
    {
      if (regMul.iuLast == 0) Mul(regMul.uSmall);
      else if (iuLast == 0)
      {
        uint u = uSmall;
        switch (u)
        {
          case 1U: this = new BigIntegerBuilder(ref regMul); break;
          case 0U: break;
          default: Load(ref regMul, 1); Mul(u); break;
        }
      }
      else
      {
        int num = iuLast + 1;
        SetSizeKeep(num + regMul.iuLast, 1);
        int index1 = num;
        while (--index1 >= 0)
        {
          uint uMul2 = rgu[index1];
          rgu[index1] = 0;
          uint uCarry = 0U;
          for (int index2 = 0; index2 <= regMul.iuLast; ++index2)
          {
            uCarry = AddMulCarry(ref rgu[index1 + index2], regMul.rgu[index2], uMul2, uCarry);
          }
          if ((int)uCarry != 0)
          {
            for (int index2 = index1 + regMul.iuLast + 1; (int)uCarry != 0 && index2 <= iuLast; ++index2)
            {
              uCarry = AddCarry(ref rgu[index2], 0U, uCarry);
            }
            if ((int)uCarry != 0)
            {
              SetSizeKeep(iuLast + 2, 0);
              rgu[iuLast] = uCarry;
            }
          }
        }
      }
    }

    /// <summary>
    /// führt eine direkte Division mit Rest durch
    /// </summary>
    /// <param name="uDen">divisor</param>
    /// <returns>Rest</returns>
    public uint DivMod(uint uDen)
    {
      if (uDen == 1) return 0;
      if (iuLast == 0)
      {
        uint r = uSmall;
        uSmall = r / uDen;
        return r % uDen;
      }
      EnsureWritable();
      ulong carry = 0UL;
      for (int i = iuLast; i >= 0; i--)
      {
        ulong r = carry << 32 | rgu[i];
        rgu[i] = (uint)(r / uDen);
        carry = r % uDen;
      }
      Trim();
      return (uint)carry;
    }

    /// <summary>
    /// führt ein direktes Modulo durch
    /// </summary>
    /// <param name="regNum">Wert, welcher berechnet werden soll</param>
    /// <param name="uDen">der dividend</param>
    /// <returns>fertiger rest</returns>
    public static uint Mod(ref BigIntegerBuilder regNum, uint uDen)
    {
      if (uDen == 1) return 0;
      if (regNum.iuLast == 0) return regNum.uSmall % uDen;
      ulong r = 0;
      for (int index = regNum.iuLast; index >= 0; --index)
      {
        r = (r << 32 | regNum.rgu[index]) % uDen;
      }
      return (uint)r;
    }

    /// <summary>
    /// führt die interne Division von großen Zahlen durch
    /// </summary>
    /// <param name="regNum">Dividend, welcher zum Rest wird</param>
    /// <param name="regDen">Divisor</param>
    /// <param name="fQuo"></param>
    /// <param name="regQuo"></param>
    static void ModDivCore(ref BigIntegerBuilder regNum, ref BigIntegerBuilder regDen, bool fQuo, ref BigIntegerBuilder regQuo)
    {
      regQuo.Set(0);
      if (regNum.iuLast < regDen.iuLast) return;

      int cuDen = regDen.iuLast + 1;
      int cuDiff = regNum.iuLast - regDen.iuLast;

      // Determine whether the result will have cuDiff "digits" or cuDiff+1 "digits".
      int cuQuo = cuDiff;
      for (int iu = regNum.iuLast; ; iu--)
      {
        if (iu < cuDiff)
        {
          cuQuo++;
          break;
        }
        if (regDen.rgu[iu - cuDiff] != regNum.rgu[iu])
        {
          if (regDen.rgu[iu - cuDiff] < regNum.rgu[iu]) cuQuo++;
          break;
        }
      }

      if (cuQuo == 0) return;

      if (fQuo) regQuo.SetSizeLazy(cuQuo);

      // Get the uint to use for the trial divisions. We normalize so the high bit is set.
      uint uDen = regDen.rgu[cuDen - 1];
      uint uDenNext = regDen.rgu[cuDen - 2];
      int cbitShiftLeft = BigIntegerHelpers.CbitHighZero(uDen);
      int cbitShiftRight = 32 - cbitShiftLeft;
      if (cbitShiftLeft > 0)
      {
        uDen = (uDen << cbitShiftLeft) | (uDenNext >> cbitShiftRight);
        uDenNext <<= cbitShiftLeft;
        if (cuDen > 2) uDenNext |= regDen.rgu[cuDen - 3] >> cbitShiftRight;
      }

      // Allocate and initialize working space.
      regNum.EnsureWritable();

      for (int iu = cuQuo; --iu >= 0; )
      {
        // Get the high (normalized) bits of regNum.
        uint uNumHi = (iu + cuDen <= regNum.iuLast) ? regNum.rgu[iu + cuDen] : 0;

        ulong uuNum = (ulong)uNumHi << 32 | regNum.rgu[iu + cuDen - 1];
        uint uNumNext = regNum.rgu[iu + cuDen - 2];
        if (cbitShiftLeft > 0)
        {
          uuNum = (uuNum << cbitShiftLeft) | (uNumNext >> cbitShiftRight);
          uNumNext <<= cbitShiftLeft;
          if (iu + cuDen >= 3) uNumNext |= regNum.rgu[iu + cuDen - 3] >> cbitShiftRight;
        }

        // Divide to get the quotient digit.
        ulong uuQuo = uuNum / uDen;
        ulong uuRem = (uint)(uuNum % uDen);
        if (uuQuo > uint.MaxValue)
        {
          uuRem += uDen * (uuQuo - uint.MaxValue);
          uuQuo = uint.MaxValue;
        }
        while (uuRem <= uint.MaxValue && uuQuo * uDenNext > (uuRem << 32 | uNumNext))
        {
          uuQuo--;
          uuRem += uDen;
        }

        // Multiply and subtract. Note that uuQuo may be 1 too large. If we have a borrow
        // at the end, we'll add the denominator back on and decrement uuQuo.
        if (uuQuo > 0)
        {
          ulong uuBorrow = 0;
          for (int iu2 = 0; iu2 < cuDen; iu2++)
          {
            uuBorrow += regDen.rgu[iu2] * uuQuo;
            uint uSub = (uint)uuBorrow;
            uuBorrow >>= 32;
            if (regNum.rgu[iu + iu2] < uSub) uuBorrow++;
            regNum.rgu[iu + iu2] -= uSub;
          }

          if (uNumHi < uuBorrow)
          {
            // Add, tracking carry.
            uint uCarry = 0;
            for (int iu2 = 0; iu2 < cuDen; iu2++)
            {
              uCarry = AddCarry(ref regNum.rgu[iu + iu2], regDen.rgu[iu2], uCarry);
            }
            uuQuo--;
          }
          regNum.iuLast = iu + cuDen - 1;
        }

        if (fQuo)
        {
          if (cuQuo == 1) regQuo.uSmall = (uint)uuQuo;
          else regQuo.rgu[iu] = (uint)uuQuo;
        }
      }

      regNum.iuLast = cuDen - 1;
      regNum.Trim();
    }

    /// <summary>
    /// führt eine Division durch
    /// </summary>
    /// <param name="dividend">der zu dividiernde Wert</param>
    public void Div(ref BigIntegerBuilder dividend)
    {
      if (dividend.iuLast == 0)
      {
        DivMod(dividend.uSmall);
      }
      else if (iuLast == 0)
      {
        uSmall = 0;
      }
      else
      {
        var bb = new BigIntegerBuilder();
        ModDivCore(ref this, ref dividend, true, ref bb);
        this = bb;
      }
    }

    /// <summary>
    /// führt einen Modulo-Berechnung durch
    /// </summary>
    /// <param name="regDen">der zu dividerende Wert</param>
    public void Mod(ref BigIntegerBuilder regDen)
    {
      if (regDen.iuLast == 0)
      {
        Set(Mod(ref this, regDen.uSmall));
      }
      else
      {
        if (iuLast == 0) return;
        var regQuo = new BigIntegerBuilder();
        ModDivCore(ref this, ref regDen, false, ref regQuo);
      }
    }
  }
}
