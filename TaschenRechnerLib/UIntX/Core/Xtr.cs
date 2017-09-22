
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
      return carry >> 32;
    }

    /// <summary>
    /// führt eine schnelle Subtraktion durch und gibt das Borrow-Flag zurück
    /// </summary>
    /// <param name="target">Zieladresse, wo das Ergebnis gespeichert werden soll</param>
    /// <param name="val1">Adresse auf den ersten Wert</param>
    /// <param name="val2">Adresse auf den zweiten Wert</param>
    /// <param name="count">Anzahl der Limbs, welche subtrahiert werden sollen</param>
    /// <returns>Borrow-Flag</returns>
    public static ulong Sub(uint* target, uint* val1, uint* val2, long count)
    {
      long i;
      ulong borrow = 0;
      for (i = 0; i < count - 1; i += 2)
      {
        ulong v1 = *(ulong*)(val1 + i);
        ulong v2 = *(ulong*)(val2 + i);
        borrow = (ulong)(uint)v1 - (uint)v2 - (borrow >> 63);
        target[i] = (uint)borrow;
        borrow = (v1 >> 32) - (v2 >> 32) - (borrow >> 63);
        target[i + 1] = (uint)borrow;
      }
      for (; i < count; i++)
      {
        borrow = (ulong)val1[i] - val2[i] - (borrow >> 63);
        target[i] = (uint)borrow;
      }
      return borrow >> 63;
    }

    /// <summary>
    /// kopiert eine bestimmte Anzahl von Limbs
    /// </summary>
    /// <param name="src">Quell-Adresse, der Limbs, welche kopiert werden sollen</param>
    /// <param name="dst">Ziel-Adresse, wohin die Limbs geschriebene werden sollen</param>
    /// <param name="count">Anzahl der Limbs, welche kopiert werden sollen</param>
    public static long CopyLimbs(uint* src, uint* dst, long count)
    {
      long i;

      // --- Limbs Blöcke-Weise übertragen (16 Limbs / 128 Bytes pro Schritt kopieren) ---
      for (i = 0; i < count - 15; i += 16)
      {
        ulong t1 = *(ulong*)(src + i);
        ulong t2 = *(ulong*)(src + i + 2);
        *(ulong*)(dst + i) = t1;
        *(ulong*)(dst + i + 2) = t2;
        t1 = *(ulong*)(src + i + 4);
        t2 = *(ulong*)(src + i + 6);
        *(ulong*)(dst + i + 4) = t1;
        *(ulong*)(dst + i + 6) = t2;
        t1 = *(ulong*)(src + i + 8);
        t2 = *(ulong*)(src + i + 10);
        *(ulong*)(dst + i + 8) = t1;
        *(ulong*)(dst + i + 10) = t2;
        t1 = *(ulong*)(src + i + 12);
        t2 = *(ulong*)(src + i + 14);
        *(ulong*)(dst + i + 12) = t1;
        *(ulong*)(dst + i + 14) = t2;
      }

      // --- restlichen Limbs übertragen (0-15 Limbs) ---
      for (; i < count - 1; i += 2) *(ulong*)(dst + i) = *(ulong*)(src + i);
      for (; i < count; i++) dst[i] = src[i];

      return i;
    }

    /// <summary>
    /// kopiert eine bestimmte Anzahl von Limbs
    /// </summary>
    /// <param name="src">Quell-Adresse, der Limbs, welche kopiert werden sollen</param>
    /// <param name="dst">Ziel-Array, wohin die Limbs geschriebene werden sollen</param>
    /// <param name="count">Anzahl der Limbs, welche kopiert werden sollen</param>
    public static void CopyLimbs(uint* src, uint[] dst, long count)
    {
      fixed (uint* dstP = dst) CopyLimbs(src, dstP, count);
    }

    /// <summary>
    /// kopiert eine bestimmte Anzahl von Limbs
    /// </summary>
    /// <param name="src">Quell-Array, der Limbs, welche kopiert werden sollen</param>
    /// <param name="dst">Ziel-Adresse, wohin die Limbs geschriebene werden sollen</param>
    /// <param name="count">Anzahl der Limbs, welche kopiert werden sollen</param>
    public static void CopyLimbs(uint[] src, uint* dst, long count)
    {
      fixed (uint* srcP = src) CopyLimbs(srcP, dst, count);
    }

    /// <summary>
    /// vergleicht zwei Limb-Ketten und gibt die Länge der unterschiedlichen Limbs zurück (0 = keine Unterschiede gefunden)
    /// </summary>
    /// <param name="val1">Zeiger auf die erste Limb-Kette</param>
    /// <param name="val2">zeiger auf die zweite Limb-Kette</param>
    /// <param name="len">Länge der beiden Limb-Ketten</param>
    /// <returns>Länge der unterschiedlichen Limb-Elemente (0 = keine Unterschiede gefunden)</returns>
    public static long DiffLen(uint* val1, uint* val2, long len)
    {
      while (len > 7
        && *(ulong*)(val1 + len - 8) == *(ulong*)(val2 + len - 8)
        && *(ulong*)(val1 + len - 6) == *(ulong*)(val2 + len - 6)
        && *(ulong*)(val1 + len - 4) == *(ulong*)(val2 + len - 4)
        && *(ulong*)(val1 + len - 2) == *(ulong*)(val2 + len - 2)) len -= 8;

      while (len > 0 && val1[len - 1] == val2[len - 1]) len--;

      return len;
    }
  }
}
