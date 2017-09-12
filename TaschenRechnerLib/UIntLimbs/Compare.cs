using System;

namespace TaschenRechnerLib
{
  public partial struct UIntLimbs : IComparable<UIntLimbs>
  {
    /// <summary>
    /// Operator zum vergleichen zweier Werte (gleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator ==(UIntLimbs val1, UIntLimbs val2)
    {
      return val1.CompareTo(val2) == 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (ungleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator !=(UIntLimbs val1, UIntLimbs val2)
    {
      return val1.CompareTo(val2) != 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (kleinerals)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator <(UIntLimbs val1, UIntLimbs val2)
    {
      return val1.CompareTo(val2) < 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (kleinergleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator <=(UIntLimbs val1, UIntLimbs val2)
    {
      return val1.CompareTo(val2) <= 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (größerals)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator >(UIntLimbs val1, UIntLimbs val2)
    {
      return val1.CompareTo(val2) > 0;
    }

    /// <summary>
    /// Operator zum vergleichen zweier Werte (größergleich)
    /// </summary>
    /// <param name="val1">Wert1</param>
    /// <param name="val2">Wert2</param>
    /// <returns>Vergleichsergebnis</returns>
    public static bool operator >=(UIntLimbs val1, UIntLimbs val2)
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
      return obj is UIntLimbs && CompareTo((UIntLimbs)obj) == 0;
    }

    /// <summary>
    /// berechnet eine einfach Hashsumme
    /// </summary>
    /// <returns>fertige Hashsumme</returns>
    public override int GetHashCode()
    {
      int hash = unchecked((int)2166136261);

      foreach (var v in limbs) hash = (hash * 16777619) ^ v;

      return hash;
    }

    /// <summary>
    /// vergleicht den Wert mit einen anderen Wert
    /// </summary>
    /// <param name="other">Wert, welcher verglichen werden soll</param>
    /// <returns>Vergleichsergebnis</returns>
    public int CompareTo(UIntLimbs other)
    {
      if (limbs.Length == other.limbs.Length)
      {
        for (int i = limbs.Length - 1; i >= 0; i--)
        {
          if (limbs[i] == other.limbs[i]) continue;

          return limbs[i] > other.limbs[i] ? 1 : -1;
        }
        return 0; // beide Werte sind gleich
      }

      return limbs.Length > other.limbs.Length ? 1 : -1;
    }
  }
}
