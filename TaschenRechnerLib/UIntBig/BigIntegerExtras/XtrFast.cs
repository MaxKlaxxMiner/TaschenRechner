using System;

namespace TaschenRechnerLib.BigIntegerExtras
{
  /// <summary>
  /// statische Klasse mit mehreren Methode für sehr schnelle Berechnungen
  /// </summary>
  internal static unsafe class XtrFast
  {
    #region # public static uint AddCarry(uint* target, uint* src, long count, ulong carry) // Hochgeschwindigkeitsfunktion zum addieren von zwei Langen Bit-Ketten
    /// <summary>
    /// Hochgeschwindigkeitsfunktion zum addieren von zwei Langen Bit-Ketten
    /// </summary>
    /// <param name="target">Ziel-Adresse, wo das Ergebnis gespeichert werden soll</param>
    /// <param name="src">Quell-Adresse des zweiten Wertes</param>
    /// <param name="count">Anzahl der Berechnungen</param>
    /// <param name="carry">(optional) Carry-Flag, welches am Start verwendet werden soll</param>
    /// <returns>Carry-Flag</returns>
    public static uint AddCarry(uint* target, uint* src, long count, ulong carry)
    {
      long i;
      for (i = 0; i < count - 1; i += 2)
      {
        ulong s = *(ulong*)(src + i);
        ulong t = *(ulong*)(target + i);
        carry = (ulong)(uint)t + (uint)s + (carry >> 32);
        target[i] = (uint)carry;
        carry = (t >> 32) + (s >> 32) + (carry >> 32);
        target[i + 1] = (uint)carry;
      }
      for (; i < count; i++)
      {
        carry = (ulong)target[i] + src[i] + (carry >> 32);
        target[i] = (uint)carry;
      }
      return (uint)(carry >> 32);
    }

    /// <summary>
    /// Referenz-Funktion zum Debuggen von <see cref="AddCarry"/> (30% langsamer)
    /// </summary>
    /// <param name="target">Ziel-Adresse, wo das Ergebnis gespeichert werden soll</param>
    /// <param name="src">Quell-Adresse des zweiten Wertes</param>
    /// <param name="count">Anzahl der Berechnungen</param>
    /// <param name="carry">(optional) Carry-Flag, welches am Start verwendet werden soll</param>
    /// <returns>Carry-Flag</returns>
    public static uint AddCarryRef(uint[] target, uint[] src, long count, ulong carry)
    {
      if (count > target.Length || count > src.Length) throw new IndexOutOfRangeException();
      for (long i = 0; i < count; i++)
      {
        ulong r = (ulong)target[i] + src[i] + carry;
        target[i] = (uint)r;
        carry = r >> 32;
      }
      return (uint)carry;
    }
    #endregion

    #region # public static uint SubBorrow(uint* target, uint* src, long count, ulong borrow) // Hochgeschwindigkeitsfunktion zum subtrahieren von zwei Langen Bit-Ketten
    /// <summary>
    /// Hochgeschwindigkeitsfunktion zum subtrahieren von zwei Langen Bit-Ketten
    /// </summary>
    /// <param name="target">Ziel-Adresse, wo das Ergebnis gespeichert werden soll</param>
    /// <param name="src">Quell-Adresse des zweiten Wertes</param>
    /// <param name="count">Anzahl der Berechnungen</param>
    /// <param name="borrow">(optional) Borrow-Flag, welches am Start verwendet werden soll</param>
    /// <returns>Borrow-Flag</returns>
    public static uint SubBorrow(uint* target, uint* src, long count, ulong borrow)
    {
      long i;
      for (i = 0; i < count - 1; i += 2)
      {
        ulong s = *(ulong*)(src + i);
        ulong t = *(ulong*)(target + i);
        borrow = (ulong)(uint)t - (uint)s - (borrow >> 63);
        target[i] = (uint)borrow;
        borrow = (t >> 32) - (s >> 32) - (borrow >> 63);
        target[i + 1] = (uint)borrow;
      }
      for (; i < count; i++)
      {
        borrow = (ulong)target[i] - src[i] - (borrow >> 63);
        target[i] = (uint)borrow;
      }
      return (uint)-(int)(borrow >> 32);
    }

    /// <summary>
    /// Referenz-Funktion zum Debuggen von <see cref="SubBorrow"/>
    /// </summary>
    /// <param name="target">Ziel-Adresse, wo das Ergebnis gespeichert werden soll</param>
    /// <param name="src">Quell-Adresse des zweiten Wertes</param>
    /// <param name="count">Anzahl der Berechnungen</param>
    /// <param name="borrow">(optional) Borrow-Flag, welches am Start verwendet werden soll</param>
    /// <returns>Borrow-Flag</returns>
    static uint SubBorrowRef(uint[] target, uint[] src, long count, ulong borrow)
    {
      if (count > target.Length || count > src.Length) throw new IndexOutOfRangeException();
      for (long i = 0; i < count; i++)
      {
        ulong r = (ulong)target[i] - src[i] - borrow;
        target[i] = (uint)r;
        borrow = (ulong)-(int)(r >> 32);
      }
      return (uint)borrow;
    }
    #endregion
  }
}
