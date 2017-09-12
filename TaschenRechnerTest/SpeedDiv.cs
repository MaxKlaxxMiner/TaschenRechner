using System;
using System.Diagnostics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
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
          //sum += i / (uint)1000000000;
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
