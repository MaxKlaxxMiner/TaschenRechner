using System;
using System.Diagnostics;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckAdd()
    {
      const int RetryCount = 10;
      const int TestCount = 100000;
      const string StartValue = "1";

      Console.WriteLine();
      Console.WriteLine("  --- Test Add() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Add() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(StartValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          val += val;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("-1.108.288.130", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Add() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntXs.Parse(StartValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          val += val;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("-1.108.288.130", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Add() - +Append x100 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(StartValue);
        var append = BigInteger.Parse(StartValue + new string('0', 100));
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount / 5; i++)
        {
          val += val;
          for (int a = 0; a < 100; a++)
          {
            val += append;
          }
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("829.227.657", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Add() - +Append x100 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntXs.Parse(StartValue);
        var append = UIntXs.Parse(StartValue + new string('0', 100));
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount / 5; i++)
        {
          val += val;
          for (int a = 0; a < 100; a++)
          {
            val += append;
          }
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("829.227.657", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
    }
  }
}
