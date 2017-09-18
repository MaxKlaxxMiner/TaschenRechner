using System;
using TaschenRechnerLib.BigIntegerExtras;

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntBig operator +(UIntBig val1, UIntBig val2)
    {
      if (val2.IsZero) return val1;
      if (val1.IsZero) return val2;

      var bb = new BigIntegerBuilder(val1);
      var reg = new BigIntegerBuilder(val2);
      bb.Add(ref reg);

      return bb.GetInteger();
    }

    /// <summary>
    /// inkrementiert eine Zahl
    /// </summary>
    /// <param name="val">Zahl, welche inkrementiert werden soll</param>
    /// <returns>fertig inkrementierte Zahl</returns>
    public static UIntBig operator ++(UIntBig val)
    {
      var bb = new BigIntegerBuilder(val);
      bb.Add(1);
      return bb.GetInteger();
    }

    /// <summary>
    /// Methode zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static UIntBig Add(UIntBig val1, UIntBig val2)
    {
      throw new NotImplementedException();
    }
  }
}
