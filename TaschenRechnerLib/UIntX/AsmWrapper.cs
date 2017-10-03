
// --- benutzt Assembler-Routinen für höchste Geschwindigkeit ---
#define USE_ASM

// --- benutzt Referenz-Methoden für bessere Zuverlässigkeit ---
//#define USE_REF

#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
#endregion

namespace TaschenRechnerLib
{
  /// <summary>
  /// Klasse mit Methoden, welche vollständig in Assembler geschrieben wurden
  /// </summary>
  public static unsafe class AsmWrapper
  {
    #region # ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n) // Addiert zwei gleichlange Zahlen-Arrays und gibt das Carry-Flag zurück (1 oder 0)
    /// <summary>
    /// Addiert zwei gleichlange Zahlen-Arrays und gibt das Carry-Flag zurück (1 oder 0)
    /// </summary>
    /// <param name="rp">Zeiger auf das zu schreibende Ergebnis</param>
    /// <param name="up">Zeiger auf den ersten Wert</param>
    /// <param name="vp">Zeiger auf den zweiten Wert</param>
    /// <param name="n">Anzahl der Limbs, welche addiert werden sollen (min: 1)</param>
    /// <returns>Carry-Flag, welches übrig geblieben ist (1 oder 0)</returns>
#if USE_ASM
    //
    [DllImport("TaschenRechnerAsm.dll"), SuppressUnmanagedCodeSecurity]
    //[DllImport("libgmp-10_5.0.1.dll", EntryPoint = "__gmpn_add_n"), SuppressUnmanagedCodeSecurity]
    public static extern ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n);
#elif USE_REF
    public static ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n)
    {
      if (n <= 0) throw new ArgumentOutOfRangeException("n");

      var rpByte = (byte*)rp;
      var upByte = (byte*)up;
      var vpByte = (byte*)vp;
      n *= sizeof(ulong);

      int carry = 0;
      for (long i = 0; i < n; i++)
      {
        int r = upByte[i] + vpByte[i] + carry;
        rpByte[i] = (byte)r;
        carry = r >> 8;
      }
      return (ulong)carry;
    }
#else
    public static ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n)
    {
      Debug.Assert(n > 0);

      ulong cy = 0;
      for (long i = 0; i < n; i++)
      {
        ulong ul = up[i];
        ulong vl = vp[i];
        ulong sl = ul + vl;
        ulong rl = sl + cy;
        rp[i] = rl;
        cy = sl < ul || rl < sl ? 1UL : 0UL;
      }
      return cy;
    }
#endif
    #endregion

    #region # long UIntX_Copy(ulong* rp, ulong* sp, long n) // kopiert mehrere Limbs und gibt die entsprechende Anzahl zurück
    /// <summary>
    /// kopiert mehrere Limbs und gibt die entsprechende Anzahl zurück
    /// </summary>
    /// <param name="rp">Ziel-Adresse, wohin die Limbs geschrieben werden sollen</param>
    /// <param name="sp">Quell-Adresse, der Limbs, welche gelesen werden sollen</param>
    /// <param name="n">Anzahl der Limbs, welche kopiert werden sollen (min: 1)</param>
    /// <returns>Anzahl der kopierten Limbs</returns>
#if USE_ASM
    public static long UIntX_Copy(ulong* rp, ulong* sp, long n)
    {
      // todo: implement Assembler
      long i = 0;
      for (; i < n - 7; i += 8)
      {
        var t0 = sp[i + 0];
        var t1 = sp[i + 1];
        var t2 = sp[i + 2];
        var t3 = sp[i + 3];
        var t4 = sp[i + 4];
        var t5 = sp[i + 5];
        var t6 = sp[i + 6];
        var t7 = sp[i + 7];
        rp[i + 0] = t0;
        rp[i + 1] = t1;
        rp[i + 2] = t2;
        rp[i + 3] = t3;
        rp[i + 4] = t4;
        rp[i + 5] = t5;
        rp[i + 6] = t6;
        rp[i + 7] = t7;
      }

      for (; i < n; i++) rp[i] = sp[i];

      return i;
    }
#elif USE_REF
    public static long UIntX_Copy(ulong* rp, ulong* sp, long n)
    {
      if (n <= 0) throw new ArgumentOutOfRangeException("n");

      long i = 0;

      for (; i < n; i++) rp[i] = sp[i];

      return i;
    }
#else
    public static long UIntX_Copy(ulong* rp, ulong* sp, long n)
    {
      Debug.Assert(n > 0);

      long i = 0;
      for (; i < n - 7; i += 8)
      {
        var t0 = sp[i + 0];
        var t1 = sp[i + 1];
        var t2 = sp[i + 2];
        var t3 = sp[i + 3];
        var t4 = sp[i + 4];
        var t5 = sp[i + 5];
        var t6 = sp[i + 6];
        var t7 = sp[i + 7];
        rp[i + 0] = t0;
        rp[i + 1] = t1;
        rp[i + 2] = t2;
        rp[i + 3] = t3;
        rp[i + 4] = t4;
        rp[i + 5] = t5;
        rp[i + 6] = t6;
        rp[i + 7] = t7;
      }

      for (; i < n; i++) rp[i] = sp[i];

      return i;
    }
#endif
    #endregion
  }
}
