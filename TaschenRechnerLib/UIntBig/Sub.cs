using TaschenRechnerLib.BigIntegerExtras;

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// Operator zum subtrahieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntBig operator -(UIntBig val1, UIntBig val2)
    {
      if (val2.IsZero) return val1;
      if (val1.IsZero) throw new InvalidCalcException();
      var bb = new BigIntegerBuilder(val1);
      var reg = new BigIntegerBuilder(val2);
      bb.Sub(ref reg);
      return bb.GetInteger();
    }

    /// <summary>
    /// dekrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche dekrementiert werden soll</param>
    /// <returns>fertig dekrementierte Zahl</returns>
    public static UIntBig operator --(UIntBig val)
    {
      if (val.IsZero) throw new InvalidCalcException();
      var bb = new BigIntegerBuilder(val);
      bb.Sub(1);
      return bb.GetInteger();
    }
  }
}
