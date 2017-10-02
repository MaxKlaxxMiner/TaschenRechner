#region # using *.*
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantAssignment
#endregion

namespace TaschenRechnerLib
{
  public unsafe partial struct UIntX
  {
    /// <summary>
    /// merkt sich die eigentlichen Daten der Zahl
    /// </summary>
    readonly ulong[] limbs;
    /// <summary>
    /// merkt sich Anzahl der Limbs, welche benutzt werden
    /// </summary>
    readonly long limbCount;

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
      limbs = new[] { val };
      limbCount = 1;
    }

    /// <summary>
    /// Konstruktor mit einer Zeichenkette, welche aus einer unsignierten Integer-Zahl besteht
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntX(string val) : this(ParseInternal(val)) { }

    /// <summary>
    /// direkter Konstruktor mit den einzelnen Werten
    /// </summary>
    /// <param name="limbs">Limb-Array, welches direkt verwendet werden soll</param>
    /// <param name="limbCount">Anzahl der benutzten Limbs</param>
    internal UIntX(ulong[] limbs, long limbCount)
    {
      if (limbs == null) throw new ArgumentNullException("limbs");
      Debug.Assert(limbCount > 0 && limbCount <= limbs.Length);
      this.limbs = limbs;
      this.limbCount = limbCount;
    }

    /// <summary>
    /// direkter Konstruktor mit den einzelnen Werten
    /// </summary>
    /// <param name="limbs">Limb-Array, welches direkt verwendet werden soll</param>
    internal UIntX(ulong[] limbs)
    {
      if (limbs == null) throw new ArgumentNullException("limbs");
      Debug.Assert(limbs.Length > 0);
      this.limbs = limbs;
      limbCount = limbs.Length;
    }

    /// <summary>
    /// direkter Konstruktor mit Verwendung eines Byte-Arrays (BigInteger.ToByteArray() kompatibel)
    /// </summary>
    /// <param name="val">Wert, welcher verwendet werden soll</param>
    public UIntX(byte[] val)
    {
      if (val == null || val.Length == 0) throw new ArgumentNullException("val");
      if (val[val.Length - 1] > 0x7f) throw new ArgumentException("val"); // marker für negative Zahl gesetzt?

      limbCount = (val.Length + 7) / 8;
      limbs = new ulong[limbCount];

      if (val.Length >= 8) // komplette Limbs direkt kopieren (schneller)
      {
        fixed (ulong* target = limbs)
        {
          Marshal.Copy(val, 0, (IntPtr)target, val.Length / 8 * 8);
        }
      }

      // --- restliche Bytes einzeln anfügen ---
      for (int i = val.Length / 8 * 8; i < val.Length; i++)
      {
        limbs[i >> 3] |= (ulong)val[i] << (i & 7) * 8;
      }
    }
  }
}
