// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntXb
  {
    /// <summary>
    /// merkt sich die ersten 10 Zahlen als Konstanten (0 bis 9)
    /// </summary>
    public static readonly UIntXb[] FirstNumbers10 =
    {
      new UIntXb(0),
      new UIntXb(1),
      new UIntXb(2),
      new UIntXb(3),
      new UIntXb(4),
      new UIntXb(5),
      new UIntXb(6),
      new UIntXb(7),
      new UIntXb(8),
      new UIntXb(9)
    };

    /// <summary>
    /// Konstante Zahl 0 (Null)
    /// </summary>
    public static readonly UIntXb Zero = FirstNumbers10[0];

    /// <summary>
    /// Konstante Zahl 1 (Eins)
    /// </summary>
    public static readonly UIntXb One = FirstNumbers10[1];

    /// <summary>
    /// Konstante Zahl 2 (Zwei)
    /// </summary>
    public static readonly UIntXb Two = FirstNumbers10[2];
  }
}
