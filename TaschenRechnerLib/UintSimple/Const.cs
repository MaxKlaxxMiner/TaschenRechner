// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib
{
  public sealed partial class UIntSimple
  {
    /// <summary>
    /// merkt sich die ersten 10 Zahlen als Konstanten (0 bis 9)
    /// </summary>
    public static readonly UIntSimple[] FirstNumbers10 =
    {
      new UIntSimple(0),
      new UIntSimple(1),
      new UIntSimple(2),
      new UIntSimple(3),
      new UIntSimple(4),
      new UIntSimple(5),
      new UIntSimple(6),
      new UIntSimple(7),
      new UIntSimple(8),
      new UIntSimple(9)
    };

    /// <summary>
    /// Konstante Zahl 0 (Null)
    /// </summary>
    public static readonly UIntSimple Zero = FirstNumbers10[0];

    /// <summary>
    /// Konstante Zahl 1 (Eins)
    /// </summary>
    public static readonly UIntSimple One = FirstNumbers10[1];

    /// <summary>
    /// Konstante Zahl 2 (Zwei)
    /// </summary>
    public static readonly UIntSimple Two = FirstNumbers10[2];
  }
}
