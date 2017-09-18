using System;
// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib.BigIntegerExtras
{
  /// <summary>
  /// weitgehender Clone vom original BitInteger (interne Klasse)
  /// </summary>
  internal struct BigIntegerBuilder
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
        uint uCarry = 0U;
        for (int index = 0; index < iu; ++index) uCarry = AddCarry(ref rgu[index], reg.rgu[index], uCarry);
        if ((int)uCarry == 0) return;
        ApplyCarry(iu);
      }
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
  }
}
