namespace TaschenRechnerLib
{
  public sealed partial class UIntSimple
  {
    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette zurück
    /// </summary>
    /// <returns>lesbare Zeichenkette</returns>
    public unsafe override string ToString()
    {
      int len = digits.Length;
      string tmp = UnsafeHelper.FastAllocateString(len);
      fixed (char* tmpP = tmp)
      {
        fixed (byte* digitsP = &digits[len - 1])
        {
          for (int i = 0; i < len; i++) tmpP[i] = (char)(*(digitsP - i) + '0');
        }
      }
      return tmp;
    }

    /// <summary>
    /// gibt die Zahl als lesbare Zeichenkette mit Tausender-Punkten zurück
    /// </summary>
    /// <param name="sepChar">Zeichen, welches für den Tausenderpunkt verwendet werden soll (default: ',')</param>
    /// <returns>lesbare Zeichenkette</returns>
    public unsafe string ToStringThousandsSep(char sepChar = ',')
    {
      int len = digits.Length + (digits.Length - 1) / 3;
      string tmp = UnsafeHelper.FastAllocateString(len);
      fixed (char* tmpP = tmp)
      {
        var tmpPp = tmpP + len - 1;
        fixed (byte* digitsP = digits)
        {
          var dg = digitsP;
          for (int i = 0; i < len; i++)
          {
            tmpPp[-i] = (i & 3) == 3 ? sepChar : (char)(*dg++ + '0');
          }
        }
      }
      return tmp;
    }
  }
}
