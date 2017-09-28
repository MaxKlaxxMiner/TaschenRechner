#region # using *.*
using System;
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
  public partial struct UIntXs
  {
    /// <summary>
    /// Operator zum multiplizieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntXs operator *(UIntXs val1, UIntXs val2)
    {
      var bb = new BigIntegerBuilder(val1);
      var regMul = new BigIntegerBuilder(val2);
      bb.Mul(ref regMul);
      return bb.GetUIntX();
    }
  }
}
