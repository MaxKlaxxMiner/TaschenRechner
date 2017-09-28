using System;
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntXs
  {
    /// <summary>
    /// gibt den Wert als Byte-Array zurück (kompatibel mit BigInteger.ToByteArray())
    /// </summary>
    /// <returns>fertiges Byte-Array</returns>
    public unsafe byte[] ToByteArray()
    {
      int byteCount = (int)(limbCount * 4 - 3);
      if (limbs[limbCount - 1] > 0x7f)
      {
        byteCount++;
        if (limbs[limbCount - 1] > 0x7fff)
        {
          byteCount++;
          if (limbs[limbCount - 1] > 0x7fffff)
          {
            byteCount++;
            if (limbs[limbCount - 1] > 0x7fffffff)
            {
              byteCount++;
            }
          }
        }
      }

      var result = new byte[byteCount];

      int limbFastCopy = Math.Min((int)limbCount, byteCount / 4);
      if (limbFastCopy > 0)
      {
        fixed (byte* target = result)
        fixed (uint* limbsP = limbs)
        {
          Xtr.CopyLimbs(limbsP, (uint*)target, limbFastCopy);
        }
      }

      // --- rest kopieren ---
      for (int i = limbFastCopy * 4; i < result.Length; i++)
      {
        if (i >> 2 < limbCount) result[i] = (byte)(limbs[i >> 2] >> (i & 3) * 8);
      }

      return result;
    }
  }
}
