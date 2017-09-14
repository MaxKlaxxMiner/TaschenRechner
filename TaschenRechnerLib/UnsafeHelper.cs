using System;
using System.Reflection;

namespace TaschenRechnerLib
{
  /// <summary>
  /// Klasse mit Hilfsmethoden
  /// </summary>
  public static class UnsafeHelper
  {
    /// <summary>
    /// schnelle Methode um einen leeren String zu erstellen
    /// </summary>
    public static readonly Func<int, string> FastAllocateString = GenFastAllocateString();

    /// <summary>
    /// gibt die Methode "string.FastAllocateString" zurück (oder eine Fallback-Variante, falls die Methode nicht gefunden wurde)
    /// </summary>
    /// <returns>Delegate auf die Methode</returns>
    static Func<int, string> GenFastAllocateString()
    {
      try
      {
        return (Func<int, string>)Delegate.CreateDelegate(typeof(Func<int, string>), typeof(string).GetMethod("FastAllocateString", BindingFlags.NonPublic | BindingFlags.Static));
      }
      catch
      {
        return count => new string('\0', count); // Fallback 
      }
    }

    /// <summary>
    /// Teilt eine (positive) Integer-Zahl durch 10 (40% bis 60% schneller als eine direkt Division)
    /// </summary>
    /// <param name="val">positiver Wert, welcher durch 10 gerechnet werden soll</param>
    /// <returns>fertig geteilter Wert</returns>
    public static int Div10(int val)
    {
      const int Div = 10;
      const int Shift = 34;
      return (int)((uint)val * (((1UL << Shift) + Div - 1) / Div) >> Shift);
    }

    /// <summary>
    /// Teilt eine (positive) Integer-Zahl durch 1000 (40% bis 60% schneller als eine direkt Division)
    /// </summary>
    /// <param name="val">positiver Wert, welcher durch 1000 gerechnet werden soll</param>
    /// <returns>fertig geteilter Wert</returns>
    public static int Div1000(int val)
    {
      const int Div = 1000;
      const int Shift = 38;
      return (int)((uint)val * (((1UL << Shift) + Div - 1) / Div) >> Shift);
    }

    /// <summary>
    /// Teilt eine (positive) Integer-Zahl durch 1000000 (40% bis 60% schneller als eine direkt Division)
    /// </summary>
    /// <param name="val">positiver Wert, welcher durch 1000000 gerechnet werden soll</param>
    /// <returns>fertig geteilter Wert</returns>
    public static int Div1000000(int val)
    {
      const int Div = 1000000;
      const int Shift = 50;
      return (int)((uint)val * (((1UL << Shift) + Div - 1) / Div) >> Shift);
    }
  }
}
