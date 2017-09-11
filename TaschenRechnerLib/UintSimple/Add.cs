using System;

namespace TaschenRechnerLib
{
  public sealed partial class UIntSimple
  {
    /// <summary>
    /// Methode zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static UIntSimple Add(UIntSimple val1, UIntSimple val2)
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

    /// <summary>
    /// addiert zwei Zahlen-Arrays (und gibt ggf. ein Carry-Flag zurück)
    /// </summary>
    /// <param name="target">Zahlen-Array, worauf die Zahlen add-Zahlen addiert werden sollen</param>
    /// <param name="add">Zahlen-Array, welche für die Addition verwendet werden soll</param>
    /// <returns></returns>
    static int Add(byte[] target, byte[] add)
    {
      if (add.Length > target.Length) throw new ArgumentOutOfRangeException();

      int carry = 0;

      // --- normale Addition ---
      for (int i = 0; i < add.Length; i++)
      {
        var r = target[i] + add[i] + carry;
        if (r > 9) { r -= 10; carry = 1; } else carry = 0;
        target[i] = (byte)r;
      }

      // --- carry-flag auf die restlichen Zahlen addieren (sofern notwendig) ---
      for (int i = add.Length; carry != 0 && i < target.Length; i++)
      {
        var r = target[i] + carry;
        if (r > 9) r -= 10; else carry = 0;
        target[i] = (byte)r;
      }

      return carry; // Ergebnis Carry-Flag (1 = wenn die Zahl zu lang war)
    }

    /// <summary>
    /// fügt ein Carry-Wert hinzu (nur wenn notwendig)
    /// </summary>
    /// <param name="val">Zahlen-Array, welches geprüft werden soll</param>
    /// <param name="carry">Carry-Zahl, welche hinzugefügt werden soll</param>
    /// <returns>fertiges Zahlen-Array</returns>
    static byte[] AppendCarry(byte[] val, int carry)
    {
      if (carry == 0) return val; // nichts zu tun

      // --- Carry-Wert hinzufügen ---
      Array.Resize(ref val, val.Length + 1);
      val[val.Length - 1] = (byte)carry;

      return val;
    }
  }
}
