using System;
// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// merkt sich die eigentlichen Daten der Zahl
    /// </summary>
    internal readonly uint[] bits;

    /// <summary>
    /// Konstruktor mit einem signierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntBig(sbyte val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntBig(short val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntBig(int val) : this((uint)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem signierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntBig(long val) : this((ulong)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem unsignierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntBig(byte val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntBig(ushort val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntBig(uint val) : this((ulong)val)
    {
      bits = new[] { val };
    }

    /// <summary>
    /// Konstruktor mit einem unsignierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntBig(ulong val)
    {
      if (val <= uint.MaxValue)
      {
        bits = new[] { (uint)val };
      }
      else
      {
        bits = new[] { (uint)val, (uint)(val >> 32) };
      }
    }

    /// <summary>
    /// Konstruktor mit einer Zeichenkette, welche aus einer unsignierten Integer-Zahl besteht
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntBig(string val) : this(ParseInternal(val)) { }

    /// <summary>
    /// direkter Konstruktor mit den einzelnen Zahlen
    /// </summary>
    /// <param name="bits">Bit-Kette, welche direkt verwendet werden soll</param>
    internal UIntBig(uint[] bits)
    {
      if (bits == null) throw new ArgumentNullException("bits");
      this.bits = bits;
    }
  }
}
