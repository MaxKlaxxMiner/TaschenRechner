using TaschenRechnerLib.Core;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local

namespace TaschenRechnerLib
{
  public sealed unsafe partial class UIntX
  {
    /// <summary>
    /// reserviert den Speicherplatz von mehreren Limbs
    /// </summary>
    /// <param name="limbs">minimale Anzahl der Limbs, welche reserviert werden sollen</param>
    /// <returns>Zeiger auf den Speicherbereich</returns>
    static uint* AllocLimbs(int limbs)
    {
      return (uint*)MemMgr.AllocUnsafe(limbs << 2);
    }

    /// <summary>
    /// gibt den Speicherbereich für die Limbs wieder frei
    /// </summary>
    /// <param name="limbs">Limbs, welche freigegeben werden sollen</param>
    /// <returns>true, wenn das freigeben erfolgreich war</returns>
    static bool FreeLimbs(uint* limbs)
    {
      return MemMgr.Free((byte*)limbs);
    }

    /// <summary>
    /// kopiert eine bestimmte Anzahl von Limbs
    /// </summary>
    /// <param name="src">Quell-Adresse, der Limbs, welche kopiert werden sollen</param>
    /// <param name="dst">Ziel-Adresse, wohin die Limbs geschriebene werden sollen</param>
    /// <param name="count">Anzahl der Limbs, welche kopiert werden sollen</param>
    static void CopyLimbs(uint* src, uint* dst, long count)
    {
      long i;
      // --- Limbs Blöcke-Weise übertragen (4 Limbs / 32 Bytes pro Schritt) ---
      for (i = 0; i < count - 3; i += 4)
      {
        ulong t1 = *(ulong*)(src + i);
        ulong t2 = *(ulong*)(src + i + 2);
        *(ulong*)(dst + i) = t1;
        *(ulong*)(dst + i + 2) = t2;
      }
      // --- restlichen Limbs übertragen (sofern notwendig) ---
      for (; i < count; i++) dst[i] = src[i];
    }

    /// <summary>
    /// kopiert eine bestimmte Anzahl von Limbs
    /// </summary>
    /// <param name="src">Quell-Adresse, der Limbs, welche kopiert werden sollen</param>
    /// <param name="dst">Ziel-Array, wohin die Limbs geschriebene werden sollen</param>
    /// <param name="count">Anzahl der Limbs, welche kopiert werden sollen</param>
    static void CopyLimbs(uint* src, uint[] dst, long count)
    {
      fixed (uint* dstP = dst) CopyLimbs(src, dstP, count);
    }

    /// <summary>
    /// kopiert eine bestimmte Anzahl von Limbs
    /// </summary>
    /// <param name="src">Quell-Array, der Limbs, welche kopiert werden sollen</param>
    /// <param name="dst">Ziel-Adresse, wohin die Limbs geschriebene werden sollen</param>
    /// <param name="count">Anzahl der Limbs, welche kopiert werden sollen</param>
    static void CopyLimbs(uint[] src, uint* dst, long count)
    {
      fixed (uint* srcP = src) CopyLimbs(srcP, dst, count);
    }
  }
}
