// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public sealed partial class UIntXm
  {
    /// <summary>
    /// merkt sich die ersten 10 Zahlen als Konstanten (0 bis 9)
    /// </summary>
    public static readonly UIntXm[] FirstNumbers10 =
    {
      new UIntXm(0),
      new UIntXm(1),
      new UIntXm(2),
      new UIntXm(3),
      new UIntXm(4),
      new UIntXm(5),
      new UIntXm(6),
      new UIntXm(7),
      new UIntXm(8),
      new UIntXm(9)
    };

    /// <summary>
    /// Konstante Zahl 0 (Null)
    /// </summary>
    public static readonly UIntXm Zero = FirstNumbers10[0];

    /// <summary>
    /// Konstante Zahl 1 (Eins)
    /// </summary>
    public static readonly UIntXm One = FirstNumbers10[1];

    /// <summary>
    /// Konstante Zahl 2 (Zwei)
    /// </summary>
    public static readonly UIntXm Two = FirstNumbers10[2];
  }
}
