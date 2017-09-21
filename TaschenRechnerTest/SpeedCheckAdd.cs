using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckAdd()
    {
      const int RetryCount = 25;
      const int TestCount = 100000;
      const string StartValue = "1";

      Console.WriteLine();
      Console.WriteLine("  --- Test Add() ---");

      //Console.WriteLine();
      //Console.WriteLine("  - BigInteger.Add() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = BigInteger.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    val += val;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '0'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - UIntLimbs.Add() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntLimbs.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    val += val;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '0'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - UIntBig.Add() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntBig.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    val += val;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '0'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - UIntX.Add() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntX.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    val += val;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '0'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - BigInteger.Add() - +Append x100 -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = BigInteger.Parse(StartValue);
      //  var append = BigInteger.Parse(StartValue + new string('0', 100));
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount / 5; i++)
      //  {
      //    val += val;
      //    for (int a = 0; a < 100; a++)
      //    {
      //      val += append;
      //    }
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '0'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - UIntSimple.Add() - +Append x100 -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntLimbs.Parse(StartValue);
      //  var append = UIntLimbs.Parse(StartValue + new string('0', 100));
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount / 5; i++)
      //  {
      //    val += val;
      //    for (int a = 0; a < 100; a++)
      //    {
      //      val += append;
      //    }
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '0'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - UIntBig.Add() - +Append x100 -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntBig.Parse(StartValue);
      //  var append = UIntBig.Parse(StartValue + new string('0', 100));
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount / 5; i++)
      //  {
      //    val += val;
      //    for (int a = 0; a < 100; a++)
      //    {
      //      val += append;
      //    }
      //  }
      //  m.Stop();
      //  long sum = val.ToString().Sum(c => (long)(c - '0'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      Console.WriteLine();
      Console.WriteLine("  - UIntX.Add() - +Append x100 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntX.Parse(StartValue);
        var append = UIntX.Parse(StartValue + new string('0', 100));
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
        long sum = val.ToString().Sum(c => (long)(c - '0'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
    }
  }
}
