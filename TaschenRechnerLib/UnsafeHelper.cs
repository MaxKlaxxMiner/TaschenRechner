using System;
using System.Reflection;

namespace TaschenRechnerLib
{
  /// <summary>
  /// Klasse mit Hilfsmethoden
  /// </summary>
  internal class UnsafeHelper
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
  }
}
