using System;
// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib
{
  public partial struct UIntXs
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
    public UIntXs(sbyte val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXs(short val) : this((long)val) { }

    /// <summary>
    /// Konstruktor mit einem signierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXs(int val) : this((uint)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem signierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll (darf nicht kleiner als 0 sein)</param>
    public UIntXs(long val) : this((ulong)val) { if (val < 0) throw new ArgumentOutOfRangeException("val"); }

    /// <summary>
    /// Konstruktor mit einem unsignierten 8-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXs(byte val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 16-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXs(ushort val) : this((ulong)val) { }

    /// <summary>
    /// Konstruktor mit einem unsignierten 32-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXs(uint val)
      : this((ulong)val)
    {
      limbs = new[] { val };
      limbCount = 1;
    }

    /// <summary>
    /// Konstruktor mit einem unsignierten 64-Bit Wert
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntXs(ulong val)
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
    public UIntXs(string val) : this(ParseInternal(val)) { }

    /// <summary>
    /// direkter Konstruktor mit den einzelnen Zahlen
    /// </summary>
    /// <param name="limbs">Bit-Kette, welche direkt verwendet werden soll</param>
    public UIntXs(uint[] limbs)
    {
      if (limbs == null) throw new ArgumentNullException("limbs");
      this.limbs = limbs;
      limbCount = limbs.Length;
    }

    /// <summary>
    /// direkter Konstruktor mit den einzelnen Zahlen
    /// </summary>
    /// <param name="limbs">Bit-Kette, welche direkt verwendet werden soll</param>
    /// <param name="limbCount">Anzahl der gesetzten Limbs</param>
    internal UIntXs(uint[] limbs, long limbCount)
    {
      if (limbs == null) throw new ArgumentNullException("limbs");
      this.limbs = limbs;
      this.limbCount = limbCount;
    }

    /// <summary>
    /// direkter Konstruktor mit Verwendung eines Byte-Arrays (BigInteger.ToByteArray() kompatibel)
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public unsafe UIntXs(byte[] val)
    {
      if (val == null || val.Length == 0) throw new ArgumentNullException("val");
      if (val[val.Length - 1] > 0x7f) throw new ArgumentException("val"); // marker für negative Zahl gesetzt?

      limbCount = (val.Length + 3) / 4;
      limbs = new uint[limbCount];

      if (val.Length >= 4) // komplette Limbs direkt kopieren (schneller)
      {
        fixed (uint* target = limbs)
        fixed (byte* valP = val)
        {
          Xtr.CopyLimbs((uint*)valP, target, val.Length / 4);
        }
      }

      // --- restliche Bytes einzeln anfügen ---
      for (int i = val.Length / 4 * 4; i < val.Length; i++)
      {
        limbs[i >> 2] |= (uint)val[i] << (i & 3) * 8;
      }
    }
  }
}
