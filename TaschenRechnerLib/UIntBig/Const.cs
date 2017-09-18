// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// merkt sich die ersten 10 Zahlen als Konstanten (0 bis 9)
    /// </summary>
    public static readonly UIntBig[] FirstNumbers10 =
    {
      new UIntBig(0),
      new UIntBig(1),
      new UIntBig(2),
      new UIntBig(3),
      new UIntBig(4),
      new UIntBig(5),
      new UIntBig(6),
      new UIntBig(7),
      new UIntBig(8),
      new UIntBig(9)
    };

    /// <summary>
    /// Konstante Zahl 0 (Null)
    /// </summary>
    public static readonly UIntBig Zero = FirstNumbers10[0];

    /// <summary>
    /// Konstante Zahl 1 (Eins)
    /// </summary>
    public static readonly UIntBig One = FirstNumbers10[1];

    /// <summary>
    /// Konstante Zahl 2 (Zwei)
    /// </summary>
    public static readonly UIntBig Two = FirstNumbers10[2];
  }
}
