// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib
{
  public sealed partial class UIntLimbs
  {
    /// <summary>
    /// merkt sich die ersten 10 Zahlen als Konstanten (0 bis 9)
    /// </summary>
    public static readonly UIntLimbs[] FirstNumbers10 =
    {
      new UIntLimbs(0),
      new UIntLimbs(1),
      new UIntLimbs(2),
      new UIntLimbs(3),
      new UIntLimbs(4),
      new UIntLimbs(5),
      new UIntLimbs(6),
      new UIntLimbs(7),
      new UIntLimbs(8),
      new UIntLimbs(9)
    };

    /// <summary>
    /// Konstante Zahl 0 (Null)
    /// </summary>
    public static readonly UIntLimbs Zero = FirstNumbers10[0];

    /// <summary>
    /// Konstante Zahl 1 (Eins)
    /// </summary>
    public static readonly UIntLimbs One = FirstNumbers10[1];

    /// <summary>
    /// Konstante Zahl 2 (Zwei)
    /// </summary>
    public static readonly UIntLimbs Two = FirstNumbers10[2];
  }
}
