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
    static uint* AllocLimbs(long limbs)
    {
      return (uint*)MemMgr.AllocUnsafe((int)(limbs << 2));
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
  }
}
