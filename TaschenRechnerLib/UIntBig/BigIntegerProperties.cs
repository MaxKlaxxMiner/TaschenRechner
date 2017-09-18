
namespace TaschenRechnerLib
{
  public partial struct UIntBig
  {
    /// <summary>
    /// gibt an, ob der Wert die Zahl 0 entspricht
    /// </summary>
    bool IsZero
    {
      get
      {
        return bits[0] == 0 && bits.Length == 1;
      }
    }
  }
}
