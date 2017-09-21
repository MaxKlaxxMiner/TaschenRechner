
namespace TaschenRechnerLib.Core
{
  /// <summary>
  /// Klasse mit Hochgeschwindikeits-Funktionen
  /// </summary>
  internal static unsafe class Xtr
  {
    /// <summary>
    /// führt eine schnelle Addition durch und gibt das Carry-Flag zurück
    /// </summary>
    /// <param name="target">Zieladresse, wo das Ergebnis gespeichert werden soll</param>
    /// <param name="val1">Adresse auf den ersten Wert</param>
    /// <param name="val2">Adresse auf den zweiten Wert</param>
    /// <param name="count">Anzahl der Limbs, welche addiert werden sollen</param>
    /// <returns>Carry-Flag</returns>
    public static ulong Add(uint* target, uint* val1, uint* val2, long count)
    {
      long i;
      ulong carry = 0;
      for (i = 0; i < count - 1; i += 2)
      {
        ulong v1 = *(ulong*)(val1 + i);
        ulong v2 = *(ulong*)(val2 + i);
        carry = (ulong)(uint)v1 + (uint)v2 + (carry >> 32);
        target[i] = (uint)carry;
        carry = (v1 >> 32) + (v2 >> 32) + (carry >> 32);
        target[i + 1] = (uint)carry;
      }
      for (; i < count; i++)
      {
        carry = (ulong)val1[i] + val2[i] + (carry >> 32);
        target[i] = (uint)carry;
      }
      return (uint)(carry >> 32);
    }
  }
}
