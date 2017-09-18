
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
    /// <param name="cuExtra">gewünschte Extra-Größe</param>
    public void Load(ref BigIntegerBuilder reg, int cuExtra)
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
  }
}
