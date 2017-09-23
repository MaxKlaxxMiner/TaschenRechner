// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib
{
  public sealed unsafe partial class UIntXm
  {
    /// <summary>
    /// gibt an, ob der Wert die Zahl 0 entspricht
    /// </summary>
    public bool IsZero
    {
      get
      {
        return limbsCount == 1 && *limbs == 0;
      }
    }
  }
}
