using System;

namespace TaschenRechnerLib
{
  public sealed partial class UIntSimple
  {
    /// <summary>
    /// Operator zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    public static UIntSimple operator +(UIntSimple val1, UIntSimple val2)
    {
      var digits1 = val1.digits;
      var digits2 = val2.digits;
      if (digits2.Length > digits1.Length)
      {
        var tmp = digits1;
        digits1 = digits2;
        digits2 = tmp;
      }
      if (digits2.Length == 1 && digits2[0] == 0) return new UIntSimple(digits1); // nichts zu addieren?

      int preCarry = digits1.Length == digits2.Length && digits1[digits1.Length - 1] + digits2[digits2.Length - 1] >= 10 ? 1 : 0;
      var result = new byte[digits1.Length + preCarry];
      Array.Copy(digits1, result, digits1.Length);

      int carry = Add(result, digits2);
      return new UIntSimple(AppendCarry(result, carry));
    }
  }
}
