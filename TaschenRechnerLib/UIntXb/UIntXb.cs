using System;
// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib
{
  public partial struct UIntXb
  {
    /// <summary>
    /// merkt sich die eigentlichen Daten der Zahl
    /// </summary>
    internal readonly uint[] limbs;
    /// <summary>
    /// merkt sich die Anzahl der belgten Limbs
    /// </summary>
    internal readonly long limbCount;

    /// <summary>
    /// Konstruktor mit einem signierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXb(sbyte val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXb(short val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXb(int val) : this((uint)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem signierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXb(long val) : this((ulong)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem unsignierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXb(byte val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXb(ushort val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXb(uint val) : this((ulong)val)
    {
      limbs = new[] { val };
      limbCount = 1;
    }

    /// <summary>
    /// Konstruktor mit einem unsignierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXb(ulong val)
    {
      if (val <= uint.MaxValue)
      {
        limbs = new[] { (uint)val };
        limbCount = 1;
      }
      else
      {
        limbs = new[] { (uint)val, (uint)(val >> 32) };
        limbCount = 2;
      }
    }

    /// <summary>
    /// Konstruktor mit einer Zeichenkette, welche aus einer unsignierten Integer-Zahl besteht
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXb(string val) : this(ParseInternal(val)) { }

    /// <summary>
    /// direkter Konstruktor mit den einzelnen Zahlen
    /// </summary>
    /// <param name="limbs">Bit-Kette, welche direkt verwendet werden soll</param>
    internal UIntXb(uint[] limbs)
    {
      if (limbs == null) throw new ArgumentNullException("limbs");
      this.limbs = limbs;
      limbCount = limbs.Length;
    }
  }
}
