#region # using *.*
using System;
using System.Runtime.InteropServices;
using System.Security;
// ReSharper disable UnusedMember.Global
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
    /// <param name="n">Anzahl der Limbs, welche addiert werden sollen</param>
    /// <returns>Carry-Flag, welches übrig geblieben ist (1 oder 0)</returns>
    [DllImport("TaschenRechnerAsm.dll"), SuppressUnmanagedCodeSecurity]
    public static extern ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n);

    /// <summary>
    /// gleiche Methode wie <see cref="UIntX_Add"/>, jedoch in C# geschrieben
    /// </summary>
    /// <param name="rp">Zeiger auf das zu schreibende Ergebnis</param>
    /// <param name="up">Zeiger auf den ersten Wert</param>
    /// <param name="vp">Zeiger auf den zweiten Wert</param>
    /// <param name="n">Anzahl der Limbs, welche addiert werden sollen</param>
    /// <returns>Carry-Flag, welches übrig geblieben ist (1 oder 0)</returns>
    public static ulong UIntX_Add_NoAsm(ulong* rp, ulong* up, ulong* vp, long n)
    {
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

    /// <summary>
    /// gleiche Methode wie <see cref="UIntX_Add"/>, dient jedoch als langsame Referenz-Variante
    /// </summary>
    /// <param name="rp">Zeiger auf das zu schreibende Ergebnis</param>
    /// <param name="up">Zeiger auf den ersten Wert</param>
    /// <param name="vp">Zeiger auf den zweiten Wert</param>
    /// <param name="n">Anzahl der Limbs, welche addiert werden sollen</param>
    /// <returns>Carry-Flag, welches übrig geblieben ist (1 oder 0)</returns>
    public static ulong UIntX_Add_Slow(ulong* rp, ulong* up, ulong* vp, long n)
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
    #endregion
  }
}
