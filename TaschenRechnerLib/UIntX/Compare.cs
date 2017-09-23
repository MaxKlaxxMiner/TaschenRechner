using System;

namespace TaschenRechnerLib
{
  public partial struct UIntX : IComparable<UIntX>
  {
    /// <summary>
    /// Operator zum vergleichen zweier Werte (gleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator ==(UIntX val1, UIntX val2)
    {
      return val1.CompareTo(val2) == 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (ungleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator !=(UIntX val1, UIntX val2)
    {
      return val1.CompareTo(val2) != 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (kleinerals)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator <(UIntX val1, UIntX val2)
    {
      return val1.CompareTo(val2) < 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (kleinergleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator <=(UIntX val1, UIntX val2)
    {
      return val1.CompareTo(val2) <= 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (größerals)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator >(UIntX val1, UIntX val2)
    {
      return val1.CompareTo(val2) > 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (größergleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator >=(UIntX val1, UIntX val2)
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
      return obj is UIntX && CompareTo((UIntX)obj) == 0;
    }

    /// <summary>
    /// berechnet eine einfach Hashsumme
    /// </summary>
    /// <returns>fertige Hashsumme</returns>
    public override int GetHashCode()
    {
      int hash = unchecked((int)2166136261);

      for (long i = 0; i < limbCount; i++)
      {
        hash = (int)((hash * 16777619) ^ limbs[i]);
      }

      return hash;
    }

    /// <summary>
    /// vergleicht den Wert mit einen anderen Wert
    /// </summary>
    /// <param name="other">Wert, welcher verglichen werden soll</param>
    /// <returns>Vergleichsergebnis</returns>
    public int CompareTo(UIntX other)
    {
      if (limbCount == other.limbCount)
      {
        for (long i = limbCount - 1; i >= 0; i--)
        {
          if (limbs[i] == other.limbs[i]) continue;

          return limbs[i] > other.limbs[i] ? 1 : -1;
        }
        return 0; // beide Werte sind gleich
      }

      return limbCount > other.limbCount ? 1 : -1;
    }
  }
}
