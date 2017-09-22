﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckSub()
    {
      const int RetryCount = 5;
      const int TestCount = 200000;
      string startValue = new string('1', 10000);
      string subValue33 = new string('3', 9998);
      string subValueMax = new string('1', 9999) + "0";

      Console.WriteLine();
      Console.WriteLine("  --- Test Sub() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Dec() -");
      Console.WriteLine();
      {
        var val = BigInteger.Parse(startValue);
        for (int r = 0; r < RetryCount; r++)
        {
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            val--;
          }
          m.Stop();
          long sum = val.ToString().Sum(c => (long)(c - '/'));
          Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Sub() - Long -");
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
          long sum = res.ToString().Sum(c => (long)(c - '/'));
          Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Sub() - Short - ");
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
          long sum = res.ToString().Sum(c => (long)(c - '/'));
          Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntXb.Dec() -");
      Console.WriteLine();
      {
        var val = UIntXb.Parse(startValue);
        for (int r = 0; r < RetryCount; r++)
        {
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            val--;
          }
          m.Stop();
          long sum = val.ToString().Sum(c => (long)(c - '/'));
          Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntXb.Sub() - Long -");
      Console.WriteLine();
      {
        var val = UIntXb.Parse(startValue);
        var sub = UIntXb.Parse(subValue33);
        for (int r = 0; r < RetryCount; r++)
        {
          var res = new UIntXb(0);
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            res = val - sub;
          }
          m.Stop();
          long sum = res.ToString().Sum(c => (long)(c - '/'));
          Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntXb.Sub() - Short - ");
      Console.WriteLine();
      {
        var val = UIntXb.Parse(startValue);
        var sub = UIntXb.Parse(subValueMax);
        for (int r = 0; r < RetryCount; r++)
        {
          var res = new UIntXb(0);
          var m = Stopwatch.StartNew();
          for (int i = 0; i < TestCount; i++)
          {
            res = val - sub;
          }
          m.Stop();
          long sum = res.ToString().Sum(c => (long)(c - '/'));
          Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
        }
      }

      Console.WriteLine();
    }
  }
}
