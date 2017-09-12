using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void SpeedCheckStr()
    {
      string num = "     " + string.Concat(Enumerable.Range(0, 1000)) + "     ";
      string numTrimmed = num.TrimStart(' ', '0').TrimEnd();
      var big = BigInteger.Parse(num);
      var simple = UIntSimple.Parse(num);
      var limbs = UIntLimbs.Parse(num);

      if (big.ToString() != numTrimmed) throw new Exception();
      if (simple.ToString() != numTrimmed) throw new Exception();
      if (limbs.ToString() != numTrimmed) throw new Exception();

      const int RetryCount = 5;
      const int TestCountStr = 300;

      #region # // --- ToString() Tests ---
      Console.WriteLine();
      Console.WriteLine("  --- Test ToString() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.ToString() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        long n = 0;
        for (int i = 0; i < TestCountStr; i++)
        {
          n += big.ToString().Length;
        }
        m.Stop();
        Console.WriteLine("    " + (n == numTrimmed.Length * (long)TestCountStr) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntSimple.ToString() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        long n = 0;
        for (int i = 0; i < TestCountStr; i++)
        {
          n += simple.ToString().Length;
        }
        m.Stop();
        Console.WriteLine("    " + (n == numTrimmed.Length * (long)TestCountStr) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.ToString() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        long n = 0;
        for (int i = 0; i < TestCountStr; i++)
        {
          n += limbs.ToString().Length;
        }
        m.Stop();
        Console.WriteLine("    " + (n == numTrimmed.Length * (long)TestCountStr) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      #endregion

      #region # // --- Parse() Tests ---
      Console.WriteLine();
      Console.WriteLine("  --- Test Parse() ---");

      Console.WriteLine();
      Console.WriteLine("  - BigInteger.Parse() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCountStr; i++)
        {
          var val = BigInteger.Parse(num);
        }
        m.Stop();
        Console.WriteLine("    Parse: " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntSimple.Parse() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCountStr; i++)
        {
          var val = UIntSimple.Parse(num);
        }
        m.Stop();
        Console.WriteLine("    Parse: " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.Parse() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCountStr; i++)
        {
          var val = UIntLimbs.Parse(num);
        }
        m.Stop();
        Console.WriteLine("    Parse: " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - UIntLimbs.ParseFast() -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCountStr; i++)
        {
          var val = UIntLimbs.ParseFast(num);
        }
        m.Stop();
        Console.WriteLine("    Parse: " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      #endregion
    }
  }
}
