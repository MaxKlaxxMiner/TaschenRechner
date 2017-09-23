// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntX
  {
    /// <summary>
    /// merkt sich die ersten 10 Zahlen als Konstanten (0 bis 9)
    /// </summary>
    public static readonly UIntX[] FirstNumbers10 =
    {
      new UIntX(0),
      new UIntX(1),
      new UIntX(2),
      new UIntX(3),
      new UIntX(4),
      new UIntX(5),
      new UIntX(6),
      new UIntX(7),
      new UIntX(8),
      new UIntX(9)
    };

    /// <summary>
    /// Konstante Zahl 0 (Null)
    /// </summary>
    public static readonly UIntX Zero = FirstNumbers10[0];

    /// <summary>
    /// Konstante Zahl 1 (Eins)
    /// </summary>
    public static readonly UIntX One = FirstNumbers10[1];

    /// <summary>
    /// Konstante Zahl 2 (Zwei)
    /// </summary>
    public static readonly UIntX Two = FirstNumbers10[2];
  }
}
