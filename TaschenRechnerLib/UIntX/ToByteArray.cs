using System;
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerLib
{
  public partial struct UIntX
  {
    /// <summary>
    /// gibt den Wert als Byte-Array zurück (kompatibel mit BigInteger.ToByteArray())
    /// </summary>
    /// <returns>fertiges Byte-Array</returns>
    public unsafe byte[] ToByteArray()
    {
      int byteCount = (int)(limbCount * 8 - 7);
      if (limbs[limbCount - 1] > 0x7ful)
      {
        byteCount++;
        if (limbs[limbCount - 1] > 0x7ffful)
        {
          byteCount++;
          if (limbs[limbCount - 1] > 0x7ffffful)
          {
            byteCount++;
            if (limbs[limbCount - 1] > 0x7ffffffful)
            {
              byteCount++;
              if (limbs[limbCount - 1] > 0x7ffffffffful)
              {
                byteCount++;
                if (limbs[limbCount - 1] > 0x7ffffffffffful)
                {
                  byteCount++;
                  if (limbs[limbCount - 1] > 0x7ffffffffffffful)
                  {
                    byteCount++;
                    if (limbs[limbCount - 1] > 0x7ffffffffffffffful)
                    {
                      byteCount++;
                    }
                  }
                }
              }
            }
          }
        }
      }

      var result = new byte[byteCount];

      int limbFastCopy = Math.Min((int)limbCount, byteCount / 8);
      if (limbFastCopy > 0)
      {
        fixed (byte* target = result)
        fixed (ulong* limbsP = limbs)
        {
          AsmWrapper.UIntX_Copy((ulong*)target, limbsP, (long)limbFastCopy);
        }
      }

      // --- rest kopieren ---
      for (int i = limbFastCopy * 8; i < result.Length; i++)
      {
        if (i >> 3 < limbCount) result[i] = (byte)(limbs[i >> 3] >> (i & 7) * 8);
      }

      return result;
    }
  }
}
