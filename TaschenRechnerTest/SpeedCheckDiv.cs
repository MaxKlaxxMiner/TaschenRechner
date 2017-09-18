using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckDiv()
    {
      const int RetryCount = 5;
      const int TestCount = 100000;
      //const string StartValue = "123456789012345678901234567890123456789012345678901234567890";
      const string TestValue = "78151841";
      const string StartValue = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
      //const string TestValue = "7815184178151841781518417815184178151841781518417815184178151841781518417815184178151841781518417815184178151841781518417815184178151841781518417815184178151841781518417815184178151841781518417815184178151841781518417815184178151841781518417815184178151841";

      Console.WriteLine();
      Console.WriteLine("  --- Test Sub() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Div() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(StartValue);
        var div = BigInteger.Parse(TestValue);
        var res = val;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val / div;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      //Console.WriteLine();
      //Console.WriteLine("  - UIntSimple.Div() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntSimple.Parse(StartValue);
      //  var div = UIntSimple.Parse(TestValue);
      //  var res = val;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    res = val / div;
      //  }
      //  m.Stop();
      //  long sum = res.ToString().Sum(c => (long)(c - '/'));
      //  Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Div() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntLimbs.Parse(StartValue);
        var div = UIntLimbs.Parse(TestValue);
        var res = val;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val / div;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
    }
  }
}
