﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckMul()
    {
      const int RetryCount = 5;
      const int TestCount = 19;
      const string StartValue = "2";

      Console.WriteLine();
      Console.WriteLine("  --- Test Sub() ---");

      //Console.WriteLine();
      //Console.WriteLine("  - BigInteger.Mul() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = BigInteger.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    val *= val;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '/'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - UIntSimple.Mul() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntSimple.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    val *= val;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '/'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Mul() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntLimbs.Parse(StartValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          val *= val;
        }
        m.Stop();
        long sum = val.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
    }
  }
}
