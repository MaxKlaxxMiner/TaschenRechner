﻿using System;
using System.Diagnostics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void AssertArray(byte[] b1, byte[] b2)
    {
      if (b1.Length != b2.Length) throw new Exception();
      for (int i = 0; i < b1.Length; i++) if (b1[i] != b2[i]) throw new Exception();
    }

    static void SpeedDiv()
    {
      for (int r = 0; r < 5; r++)
      {
        long sum = 0;
        Stopwatch mess = Stopwatch.StartNew();
        for (int i = 0; i < int.MaxValue; i++)
        {
          sum += UnsafeHelper.Div10(i);
          //sum += i / 10;
          //sum += i / (uint)10;
        }
        mess.Stop();

        Console.WriteLine(sum.ToString().Replace("230584299632879208", "ok") + " (" + mess.ElapsedMilliseconds.ToString("N0") + " ms)");
      }

      // --- Validate ---
      for (int i = 0; i < int.MaxValue; i++)
      {
        int d1 = UnsafeHelper.Div10(i);
        int d2 = i / 10;
        if (d1 != d2) throw new Exception(i.ToString("N0"));
      }
    }
  }
}
