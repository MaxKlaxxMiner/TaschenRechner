using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckDec()
    {
      const int RetryCount = 10;
      const int TestCount = 100000;
      const string StartValue = "1000000000";
      const string StartValueLong = "1000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
      string startValueBorrow = (new BigInteger(1) << 4096).ToString();

      Console.WriteLine();
      Console.WriteLine("  --- Test Dec() ---");

      //Console.WriteLine();
      //Console.WriteLine("  - BigInteger.Dec() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = BigInteger.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount * 100; i++)
      //  {
      //    val--;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().GetHashCode();
      //  Console.WriteLine("    " + sum.ToString("N0").Replace("-827.242.760", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - BigInteger.Dec() - Long -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = BigInteger.Parse(StartValueLong);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount * 100; i++)
      //  {
      //    val--;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().GetHashCode();
      //  Console.WriteLine("    " + sum.ToString("N0").Replace("-20.299.442", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - BigInteger.Dec() - Borrow -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = BigInteger.Parse(startValueBorrow);
      //  var tmp = val;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount * 20; i++)
      //  {
      //    tmp = val;
      //    tmp--;
      //  }
      //  m.Stop();
      //  long sum = tmp.ToString().GetHashCode();
      //  Console.WriteLine("    " + sum.ToString("N0").Replace("-756.363.346", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - UIntXb.Dec() -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntXb.Parse(StartValue);
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount * 100; i++)
      //  {
      //    val--;
      //  }
      //  m.Stop();
      //  long sum = val.ToString().GetHashCode();
      //  Console.WriteLine("    " + sum.ToString("N0").Replace("-827.242.760", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      Console.WriteLine();
      Console.WriteLine("  - UIntXb.Dec() - Long -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntXb.Parse(StartValueLong);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount * 100; i++)
        {
          val--;
        }
        m.Stop();
        long sum = val.ToString().GetHashCode();
        Console.WriteLine("    " + sum.ToString("N0").Replace("-20.299.442", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      //Console.WriteLine();
      //Console.WriteLine("  - UIntXb.Dec() - Borrow -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  var val = UIntXb.Parse(startValueBorrow);
      //  var tmp = val;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount * 20; i++)
      //  {
      //    tmp = val;
      //    tmp--;
      //  }
      //  m.Stop();
      //  long sum = tmp.ToString().GetHashCode();
      //  Console.WriteLine("    " + sum.ToString("N0").Replace("-756.363.346", "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      Console.WriteLine();
    }
  }
}
