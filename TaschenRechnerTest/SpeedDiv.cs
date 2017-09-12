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
        for (int i = 0; i < int.MaxValue - 1; i++)
        {
          sum += UnsafeHelper.Div1000000(i);
          //sum += i / 1000000;
          //sum += i / (uint)1000000;
        }
        mess.Stop();

        Console.WriteLine(sum.ToString().Replace("2304769387962", "ok") + " (" + mess.ElapsedMilliseconds.ToString("N0") + " ms)");
      }

      // --- Validate ---
      for (int i = 0; i < int.MaxValue - 1; i++)
      {
        int d1 = UnsafeHelper.Div1000000(i);
        int d2 = i / 1000000;
        if (d1 != d2) throw new Exception(i.ToString("N0"));
      }
    }
  }
}
