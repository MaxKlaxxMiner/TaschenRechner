using System;
using System.Diagnostics;

// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantAssignment

namespace TaschenRechnerLib
{
  public sealed unsafe partial class UIntXm
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
    public UIntXm(sbyte val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXm(short val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXm(int val) : this((uint)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem signierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXm(long val) : this((ulong)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem unsignierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXm(byte val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXm(ushort val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXm(uint val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXm(ulong val)
    {
      limbs = AllocLimbs(2);
      *(ulong*)limbs = val;
      limbsCount = val <= uint.MaxValue ? 1 : 2;
    }

    /// <summary>
    /// Konstruktor mit einer Zeichenkette, welche aus einer unsignierten Integer-Zahl besteht
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXm(string val) : this(ParseInternal(val)) { }

    /// <summary>
    /// direkter Konstruktor mit direkter Speicheradresse
    /// </summary>
    /// <param name="limbs">Limbs, welche direkt verwendet werden sollen</param>
    /// <param name="limbsCount">Anzahl der Limbs, welche benutzt werden</param>
    internal UIntXm(uint* limbs, long limbsCount)
    {
      this.limbs = limbs;
      this.limbsCount = limbsCount;
    }

    /// <summary>
    /// Konstruktor mit Limbs, welche direkt verwendet werden sollen
    /// </summary>
    /// <param name="limbs">Array mit den Limbs, welche verwendet werden sollen</param>
    internal UIntXm(uint[] limbs)
    {
      this.limbs = AllocLimbs(limbs.Length);
      limbsCount = limbs.Length;
      Xtr.CopyLimbs(limbs, this.limbs, limbsCount);
    }

    /// <summary>
    /// Destructor zum freigeben des Speicherbereiches
    /// </summary>
    ~UIntXm()
    {
      bool ok = FreeLimbs(limbs);
      Debug.Assert(ok);
    }
  }
}
