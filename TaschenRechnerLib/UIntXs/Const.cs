// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntXs
  {
    /// <summary>
    /// merkt sich die ersten 10 Zahlen als Konstanten (0 bis 9)
    /// </summary>
    public static readonly UIntXs[] FirstNumbers10 =
    {
      new UIntXs(0),
      new UIntXs(1),
      new UIntXs(2),
      new UIntXs(3),
      new UIntXs(4),
      new UIntXs(5),
      new UIntXs(6),
      new UIntXs(7),
      new UIntXs(8),
      new UIntXs(9)
    };

    /// <summary>
    /// Konstante Zahl 0 (Null)
    /// </summary>
    public static readonly UIntXs Zero = FirstNumbers10[0];

    /// <summary>
    /// Konstante Zahl 1 (Eins)
    /// </summary>
    public static readonly UIntXs One = FirstNumbers10[1];

    /// <summary>
    /// Konstante Zahl 2 (Zwei)
    /// </summary>
    public static readonly UIntXs Two = FirstNumbers10[2];
  }
}
