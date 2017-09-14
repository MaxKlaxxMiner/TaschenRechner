using System;

namespace TaschenRechnerLib
{
  public partial struct UIntLimbs
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

    /// <summary>
    /// Methode zum addieren zweier Zahlen
    /// </summary>
    /// <param name="val1">erster Wert, welcher verwendet werden soll</param>
    /// <param name="val2">zweiter Wert, welcher verwendet werden soll</param>
    /// <returns>fertiges Ergebnis</returns>
    static UIntLimbs Add(UIntLimbs val1, UIntLimbs val2)
    {
      var limbs1 = val1.limbs;
      var limbs2 = val2.limbs;
      if (limbs2.Length > limbs1.Length) { var tmp = limbs1; limbs1 = limbs2; limbs2 = tmp; }
      if (limbs2.Length == 1 && limbs2[0] == 0) return new UIntLimbs(limbs1); // nichts zu addieren?

      int preCarry = limbs1.Length == limbs2.Length && limbs1[limbs1.Length - 1] + limbs2[limbs2.Length - 1] >= LimbSize ? 1 : 0;
      var result = new int[limbs1.Length + preCarry];
      Array.Copy(limbs1, result, limbs1.Length);

      int carry = Add(result, limbs2);
      return new UIntLimbs(AppendCarry(result, carry));
    }

    /// <summary>
    /// addiert zwei Zahlen-Arrays (und gibt ggf. ein Carry-Flag zurück)
    /// </summary>
    /// <param name="target">Zahlen-Array, worauf die Zahlen add-Zahlen addiert werden sollen</param>
    /// <param name="add">Zahlen-Array, welche für die Addition verwendet werden soll</param>
    /// <returns></returns>
    static int Add(int[] target, int[] add)
    {
      if (add.Length > target.Length) throw new InvalidCalcException();

      int carry = 0;

      // --- normale Addition ---
      for (int i = 0; i < add.Length; i++)
      {
        int r = target[i] + add[i] + carry;
        carry = (int)((uint)(r + (int)((1u << 31) - LimbSize)) >> 31);
        target[i] = r - carry * LimbSize;
      }

      // --- carry-flag auf die restlichen Zahlen addieren (sofern notwendig) ---
      for (int i = add.Length; carry != 0 && i < target.Length; i++)
      {
        var r = target[i] + carry;
        if (r >= LimbSize) r -= LimbSize; else carry = 0;
        target[i] = r;
      }

      return carry; // Ergebnis Carry-Flag (1 = wenn die Zahl zu lang war)
    }

    /// <summary>
    /// fügt ein Carry-Wert hinzu (nur wenn notwendig)
    /// </summary>
    /// <param name="val">Zahlen-Array, welches geprüft werden soll</param>
    /// <param name="carry">Carry-Zahl, welche hinzugefügt werden soll</param>
    /// <returns>fertiges Zahlen-Array</returns>
    static int[] AppendCarry(int[] val, int carry)
    {
      if (carry == 0) return val; // nichts zu tun

      // --- Carry-Wert hinzufügen ---
      Array.Resize(ref val, val.Length + 1);
      val[val.Length - 1] = carry;

      return val;
    }
  }
}
