#region # using *.*
using System;
using System.Diagnostics;
using TaschenRechnerLib.BigIntegerExtras;

// ReSharper disable TailRecursiveCall
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
// ReSharper disable RedundantIfElseBlock
// ReSharper disable RedundantLogicalConditionalExpressionOperand
#pragma warning disable 162
#endregion

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// Operator zum dividieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntBig operator /(UIntBig val1, UIntBig val2)
    {
      var bb = new BigIntegerBuilder(val1);
      var regDen = new BigIntegerBuilder(val2);
      bb.Div(ref regDen);
      return bb.GetInteger();
    }
  }
}
