using System;
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
      const int TestCount = 100000;
      string startValue = new string('1', 10000);
      string subValue1 = new string('1', 9999);
      string subValue2 = new string('3', 9998);
      string subValue3 = new string('1', 9999) + "0";

      Console.WriteLine();
      Console.WriteLine("  --- Test Sub() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Dec() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(startValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          val--;
        }
        m.Stop();
        long sum = val.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Sub() - Type: 1111 - 111 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(startValue);
        var sub = BigInteger.Parse(subValue1);
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

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Sub() - Type: 1111 - 33 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(startValue);
        var sub = BigInteger.Parse(subValue2);
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

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Sub() - Type: 1111 - 1110 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = BigInteger.Parse(startValue);
        var sub = BigInteger.Parse(subValue3);
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

      Console.WriteLine();
      Console.WriteLine("  - UIntSimple.Dec() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntSimple.Parse(startValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          val--;
        }
        m.Stop();
        long sum = val.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntSimple.Sub() - Type: 1111 - 111 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntSimple.Parse(startValue);
        var sub = UIntSimple.Parse(subValue1);
        var res = new UIntSimple(0);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val - sub;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntSimple.Sub() - Type: 1111 - 33 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntSimple.Parse(startValue);
        var sub = UIntSimple.Parse(subValue2);
        var res = new UIntSimple(0);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val - sub;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntSimple.Sub() - Type: 1111 - 1110 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntSimple.Parse(startValue);
        var sub = UIntSimple.Parse(subValue3);
        var res = new UIntSimple(0);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val - sub;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Dec() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntLimbs.Parse(startValue);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          val--;
        }
        m.Stop();
        long sum = val.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Sub() - Type: 1111 - 111 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntLimbs.Parse(startValue);
        var sub = UIntLimbs.Parse(subValue1);
        var res = new UIntLimbs(0);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val - sub;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Sub() - Type: 1111 - 33 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntLimbs.Parse(startValue);
        var sub = UIntLimbs.Parse(subValue2);
        var res = new UIntLimbs(0);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val - sub;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Sub() - Type: 1111 - 1110 -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var val = UIntLimbs.Parse(startValue);
        var sub = UIntLimbs.Parse(subValue3);
        var res = new UIntLimbs(0);
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          res = val - sub;
        }
        m.Stop();
        long sum = res.ToString().Sum(c => (long)(c - '/'));
        Console.WriteLine("    " + sum.ToString("N0") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
    }
  }
}
