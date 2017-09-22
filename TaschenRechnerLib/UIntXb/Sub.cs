using TaschenRechnerLib.BigIntegerExtras;

namespace TaschenRechnerLib
{
  public partial struct UIntXb
  {
    /// <summary>
    /// Operator zum subtrahieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntXb operator -(UIntXb val1, UIntXb val2)
    {
      if (val2.limbs[0] == 0 && val2.limbCount == 1) return val1;
      if (val1.limbCount < val2.limbCount) throw new InvalidCalcException();
      if (val1.limbs[0] == 0 && val1.limbCount == 1) throw new InvalidCalcException();

      var bb = new BigIntegerBuilder(val1);
      var reg = new BigIntegerBuilder(val2);
      bb.Sub(ref reg);

      return bb.GetUIntXb();
    }

    /// <summary>
    /// dekrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche dekrementiert werden soll</param>
    /// <returns>fertig dekrementierte Zahl</returns>
    public static UIntXb operator --(UIntXb val)
    {
      if (val.limbs[0] == 0 && val.limbCount == 1) throw new InvalidCalcException();
      var bb = new BigIntegerBuilder(val);
      bb.Sub(1);
      return bb.GetUIntXb();
    }
  }
}
