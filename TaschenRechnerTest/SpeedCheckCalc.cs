using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckCalc()
    {
      const int RetryCount = 5;
      const int TestCount = 100000;
      const string StartValue = "1";

      #region # // --- ToString() Tests ---
      Console.WriteLine();
      Console.WriteLine("  --- Test Add() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Add() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        var val = BigInteger.Parse(StartValue);
        for (int i = 0; i < TestCount; i++)
        {
          val += val;
        }
        m.Stop();
        long sum = val.ToString().Sum(c => (long)(c - '0'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntSimple.Add() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        var val = new UIntSimple(1);
        for (int i = 0; i < TestCount; i++)
        {
          val += val;
        }
        m.Stop();
        long sum = val.ToString().Sum(c => (long)(c - '0'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Add() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        var val = UIntLimbs.Parse(StartValue);
        for (int i = 0; i < TestCount; i++)
        {
          val += val;
        }
        m.Stop();
        long sum = val.ToString().Sum(c => (long)(c - '0'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      #endregion
    }
  }
}
