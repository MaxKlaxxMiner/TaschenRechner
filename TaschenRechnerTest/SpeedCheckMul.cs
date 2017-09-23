using System;
using System.Diagnostics;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckMul()
    {
      const int RetryCount = 10;
      const int TestCount = 18;
      const string StartValue = "2";

      Console.WriteLine();
      Console.WriteLine("  --- Test Sub() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Mul() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var start = BigInteger.Parse(StartValue);
        var val = start;
        var m = Stopwatch.StartNew();
        for (int j = 0; j < 10; j++)
        {
          val = start;
          for (int i = 0; i < TestCount; i++)
          {
            val *= val;
          }
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("1.034.604.941", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Mul() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var start = UIntLimbs.Parse(StartValue);
        var val = start;
        var m = Stopwatch.StartNew();
        for (int j = 0; j < 10; j++)
        {
          val = start;
          for (int i = 0; i < TestCount; i++)
          {
            val *= val;
          }
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("1.034.604.941", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
    }
  }
}
