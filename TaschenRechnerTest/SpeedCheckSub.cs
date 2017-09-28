using System;
using System.Diagnostics;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckSub()
    {
      const int RetryCount = 10;
      const int TestCount = 200000;
      string startValue = new string('1', 10000);
      string subValue33 = new string('3', 9998);
      string subValueMax = new string('1', 9999) + "0";

      Console.WriteLine();
      Console.WriteLine("  --- Test Sub() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Sub() - Long Result -");
      Console.WriteLine();
      {
        var val = BigInteger.Parse(startValue);
        var sub = BigInteger.Parse(subValue33);
        for (int r = 0; r < RetryCount; r++)
        {
          var res = new BigInteger(0);
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            res = val - sub;
          }
          m.Stop();
          long sum = res.ToString().GetHashCode();
          Console.WriteLine("    " + sum.ToString("N0").Replace("-322.311.868", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Sub() - Long Result -");
      Console.WriteLine();
      {
        var val = UIntXs.Parse(startValue);
        var sub = UIntXs.Parse(subValue33);
        for (int r = 0; r < RetryCount; r++)
        {
          var res = new UIntXs(0);
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            res = val - sub;
          }
          m.Stop();
          long sum = res.ToString().GetHashCode();
          Console.WriteLine("    " + sum.ToString("N0").Replace("-322.311.868", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Sub() - Short Result - ");
      Console.WriteLine();
      {
        var val = BigInteger.Parse(startValue);
        var sub = BigInteger.Parse(subValueMax);
        for (int r = 0; r < RetryCount; r++)
        {
          var res = new BigInteger(0);
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            res = val - sub;
          }
          m.Stop();
          long sum = res.ToString().GetHashCode();
          Console.WriteLine("    " + sum.ToString("N0").Replace("372.029.325", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Sub() - Short Result - ");
      Console.WriteLine();
      {
        var val = UIntXs.Parse(startValue);
        var sub = UIntXs.Parse(subValueMax);
        for (int r = 0; r < RetryCount; r++)
        {
          var res = new UIntXs(0);
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            res = val - sub;
          }
          m.Stop();
          long sum = res.ToString().GetHashCode();
          Console.WriteLine("    " + sum.ToString("N0").Replace("372.029.325", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
    }
  }
}
