using System;
using System.Diagnostics;
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantAssignment

namespace TaschenRechnerLib
{
  public sealed unsafe partial class UIntX
  {
    /// <summary>
    /// merkt sich die eigentlichen Daten der Zahl
    /// </summary>
    readonly uint* limbs;
    /// <summary>
    /// merkt sich Anzahl der Limbs, welche benutzt werden
    /// </summary>
    readonly long limbsCount;

    /// <summary>
    /// Konstruktor mit einem signierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntX(sbyte val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntX(short val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntX(int val) : this((uint)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem signierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntX(long val) : this((ulong)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem unsignierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntX(byte val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntX(ushort val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntX(uint val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntX(ulong val)
    {
      limbs = AllocLimbs(2);
      *(ulong*)limbs = val;
      limbsCount = val <= uint.MaxValue ? 1 : 2;
    }

    ///// <summary>
    ///// Konstruktor mit einer Zeichenkette, welche aus einer unsignierten Integer-Zahl besteht
    ///// </summary>
    ///// <param name="val">Wert, welcher verwendet werden soll</param>
    //public UIntBig(string val) : this(ParseInternal(val)) { }

    /// <summary>
    /// direkter Konstruktor mit direkter Speicheradresse
    /// </summary>
    /// <param name="limbs">Limbs, welche direkt verwendet werden sollen</param>
    /// <param name="limbsCount">Anzahl der Limbs, welche benutzt werden</param>
    internal UIntX(uint* limbs, long limbsCount)
    {
      this.limbs = limbs;
      this.limbsCount = limbsCount;
    }

    /// <summary>
    /// Destructor zum freigeben des Speicherbereiches
    /// </summary>
    ~UIntX()
    {
      bool ok = FreeLimbs(limbs);
      Debug.Assert(ok);
    }
  }
}
