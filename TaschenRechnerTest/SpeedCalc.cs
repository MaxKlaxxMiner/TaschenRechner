#region # using *.*
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
// ReSharper disable ConvertToConstant.Local
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming
#pragma warning disable 169
#pragma warning disable 414
#endregion

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static uint CreateValue(uint val) { return Environment.TickCount > 0 ? val : 0; } // return same value -> disable compiler optimizations

    static uint v0 = CreateValue(0);
    static uint v1 = CreateValue(1);
    static uint vf = CreateValue(uint.MaxValue);

    static void SpeedCalcNotNullBranched()
    {
      const int RetryCount = 5;
      const int TestCount = 100000000;

      Console.WriteLine();
      Console.WriteLine("  - 0 - isNotNull - branched - ternary - ");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += (v0 == 0 ? 0u : 13u);
        }
        m.Stop();
        Console.WriteLine("    " + (l == (v0 == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - 1 - isNotNull - branched - ternary - ");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += (v1 == 0 ? 0u : 13u);
        }
        m.Stop();
        Console.WriteLine("    " + (l == (v1 == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - f - isNotNull - branched - ternary - ");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += (vf == 0 ? 0u : 13u);
        }
        m.Stop();
        Console.WriteLine("    " + (l == (vf == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - 0 - isNotNull - branched - if - ");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          if (v0 != 0) l += 13;
        }
        m.Stop();
        Console.WriteLine("    " + (l == (v0 == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - 1 - isNotNull - branched - if - ");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          if (v1 != 0) l += 13;
        }
        m.Stop();
        Console.WriteLine("    " + (l == (v1 == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - f - isNotNull - branched - if - ");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          if (vf != 0) l += 13;
        }
        m.Stop();
        Console.WriteLine("    " + (l == (vf == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static void SpeedCalcNotNullBrancheless()
    {
      const int RetryCount = 5;
      const int TestCount = 100000000;

      Console.WriteLine();
      Console.WriteLine("  - 0 - isNotNull - branchless - mul-shift -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += (uint)(v0 * ulong.MaxValue >> 63) * 13;
        }
        m.Stop();
        Console.WriteLine("    " + (l == (v0 == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - 1 - isNotNull - branchless - mul-shift -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += (uint)(v1 * ulong.MaxValue >> 63) * 13;
        }
        m.Stop();
        Console.WriteLine("    " + (l == (v1 == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - f - isNotNull - branchless - mul-shift -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += (uint)(vf * ulong.MaxValue >> 63) * 13;
        }
        m.Stop();
        Console.WriteLine("    " + (l == (vf == 0 ? 0 : 13) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static void SpeedCalc()
    {
      Console.WriteLine();
      Console.WriteLine("  --- Test Calc ---");

      SpeedCalcNotNullBranched();
      SpeedCalcNotNullBrancheless();
    }
  }
}
