using System;
using System.Diagnostics;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckInc()
    {
      const int RetryCount = 10;
      const int TestCount = 100000;
      const string StartValue = "1";
      const string StartValueLong = "1000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
      string startValueCarry = ((new BigInteger(1) << 4096) - 1).ToString();

      Console.WriteLine();
      Console.WriteLine("  --- Test Inc() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Inc() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(StartValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount * 100; i++)
        {
          val++;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("-1.704.021.064", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Inc() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntX.Parse(StartValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount * 100; i++)
        {
          val++;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("-1.704.021.064", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Inc() - Long -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(StartValueLong);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount * 50; i++)
        {
          val++;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("866.435.908", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Inc() - Long -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntX.Parse(StartValueLong);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount * 50; i++)
        {
          val++;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("866.435.908", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Inc() - Carry -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(startValueCarry);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount * 20; i++)
        {
          val++;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("1.188.235.812", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Inc() - Carry -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntX.Parse(startValueCarry);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount * 20; i++)
        {
          val++;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("1.188.235.812", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
    }
  }
}
