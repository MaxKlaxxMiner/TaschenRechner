using System;

namespace TaschenRechnerLib
{
  public partial struct UIntBig : IComparable<UIntBig>
  {
    /// <summary>
    /// Operator zum vergleichen zweier Werte (gleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator ==(UIntBig val1, UIntBig val2)
    {
      return val1.CompareTo(val2) == 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (ungleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator !=(UIntBig val1, UIntBig val2)
    {
      return val1.CompareTo(val2) != 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (kleinerals)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator <(UIntBig val1, UIntBig val2)
    {
      return val1.CompareTo(val2) < 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (kleinergleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator <=(UIntBig val1, UIntBig val2)
    {
      return val1.CompareTo(val2) <= 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (größerals)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator >(UIntBig val1, UIntBig val2)
    {
      return val1.CompareTo(val2) > 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (größergleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator >=(UIntBig val1, UIntBig val2)
    {
      return val1.CompareTo(val2) >= 0;
    }

    /// <summary>
    /// direkte Vergleichsmethode mit einem Objekt
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      return obj is UIntBig && CompareTo((UIntBig)obj) == 0;
    }

    /// <summary>
    /// berechnet eine einfach Hashsumme
    /// </summary>
    /// <returns>fertige Hashsumme</returns>
    public override int GetHashCode()
    {
      int hash = unchecked((int)2166136261);

      foreach (var v in bits) hash = (int)((hash * 16777619) ^ v);

      return hash;
    }

    /// <summary>
    /// vergleicht den Wert mit einen anderen Wert
    /// </summary>
    /// <param name="other">Wert, welcher verglichen werden soll</param>
    /// <returns>Vergleichsergebnis</returns>
    public int CompareTo(UIntBig other)
    {
      if (bits.Length == other.bits.Length)
      {
        for (int i = bits.Length - 1; i >= 0; i--)
        {
          if (bits[i] == other.bits[i]) continue;

          return bits[i] > other.bits[i] ? 1 : -1;
        }
        return 0; // beide Werte sind gleich
      }

      return bits.Length > other.bits.Length ? 1 : -1;
    }
  }
}
