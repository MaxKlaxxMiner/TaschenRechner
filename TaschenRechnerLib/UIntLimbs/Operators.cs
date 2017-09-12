
namespace TaschenRechnerLib
{
  public sealed partial class UIntLimbs
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntLimbs operator +(UIntLimbs val1, UIntLimbs val2)
    {
      return Add(val1, val2);
    }

    /// <summary>
    /// inkrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche inkrementiert werden soll</param>
    /// <returns>fertig inkrementierte Zahl</returns>
    public static UIntLimbs operator ++(UIntLimbs val)
    {
      return Add(val, One);
    }
  }
}
