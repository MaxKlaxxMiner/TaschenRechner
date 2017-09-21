// ReSharper disable MemberCanBePrivate.Global

namespace TaschenRechnerLib
{
  public partial struct UIntXb
  {
    /// <summary>
    /// gibt an, ob der Wert die Zahl 0 entspricht
    /// </summary>
    public bool IsZero
    {
      get
      {
        return limbCount == 1 && limbs[0] == 0;
      }
    }
  }
}
