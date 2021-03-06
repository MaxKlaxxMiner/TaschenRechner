﻿#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using TaschenRechnerLib;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable UnreachableCode
// ReSharper disable ConvertToConstant.Local
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable IntroduceOptionalParameters.Local
#pragma warning disable 162
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

    static uint MinBranched(uint val1, uint val2)
    {
      return val1 < val2 ? val1 : val2;
    }
    static void SpeedCalcMinBranched(uint val1, uint val2)
    {
      const int RetryCount = 10;
      const int TestCount = 100000000;

      Console.WriteLine();
      Console.WriteLine("  - " + val1.ToString("x").Substring(0, 1) + "," + val2.ToString("x").Substring(0, 1) + " - min - branched - ternary -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += MinBranched(val1, val2);
        }
        m.Stop();
        Console.WriteLine("    " + (l == Math.Min(val1, val2) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static uint MaxBranched(uint val1, uint val2)
    {
      return val1 < val2 ? val2 : val1;
    }
    static void SpeedCalcMaxBranched(uint val1, uint val2)
    {
      const int RetryCount = 10;
      const int TestCount = 100000000;

      Console.WriteLine();
      Console.WriteLine("  - " + val1.ToString("x").Substring(0, 1) + "," + val2.ToString("x").Substring(0, 1) + " - max - branched - ternary -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        uint l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += MaxBranched(val1, val2);
        }
        m.Stop();
        Console.WriteLine("    " + (l == Math.Max(val1, val2) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static long MinBranchless(long x, long y)
    {
      return x + ((y - x) >> 63 & (y - x));
    }
    static void SpeedCalcMinBranchless(long val1, long val2)
    {
      const int RetryCount = 10;
      const int TestCount = 100000000;

      Console.WriteLine();
      Console.WriteLine("  - " + val1.ToString("x").Substring(0, 1) + "," + val2.ToString("x").Substring(0, 1) + " - min - branchless -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += MinBranchless(val1, val2);
        }
        m.Stop();
        Console.WriteLine("    " + (l == Math.Min(val1, val2) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static long MaxBranchless(long x, long y)
    {
      return x - ((x - y) >> 63 & (x - y));
    }
    static void SpeedCalcMaxBranchless(long val1, long val2)
    {
      const int RetryCount = 10;
      const int TestCount = 100000000;

      Console.WriteLine();
      Console.WriteLine("  - " + val1.ToString("x").Substring(0, 1) + "," + val2.ToString("x").Substring(0, 1) + " - max - branchless -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long l = 0;
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += MaxBranchless(val1, val2);
        }
        m.Stop();
        Console.WriteLine("    " + (l == Math.Max(val1, val2) * TestCount) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static void SpeedCalcMinMaxBranched()
    {
      //SpeedCalcMinBranched(v0, v0);
      //SpeedCalcMinBranched(v0, v1);
      //SpeedCalcMinBranched(v0, vf);
      //SpeedCalcMinBranched(v1, v0);
      //SpeedCalcMinBranched(v1, v1);
      //SpeedCalcMinBranched(v1, vf);
      //SpeedCalcMinBranched(vf, v0);
      //SpeedCalcMinBranched(vf, v1);
      //SpeedCalcMinBranched(vf, vf);

      //SpeedCalcMaxBranched(v0, v0);
      //SpeedCalcMaxBranched(v0, v1);
      //SpeedCalcMaxBranched(v0, vf);
      //SpeedCalcMaxBranched(v1, v0);
      //SpeedCalcMaxBranched(v1, v1);
      //SpeedCalcMaxBranched(v1, vf);
      //SpeedCalcMaxBranched(vf, v0);
      //SpeedCalcMaxBranched(vf, v1);
      //SpeedCalcMaxBranched(vf, vf);

      SpeedCalcMinBranchless(v0, v0);
      SpeedCalcMinBranchless(v0, v1);
      SpeedCalcMinBranchless(v0, vf);
      SpeedCalcMinBranchless(v1, v0);
      SpeedCalcMinBranchless(v1, v1);
      SpeedCalcMinBranchless(v1, vf);
      SpeedCalcMinBranchless(vf, v0);
      SpeedCalcMinBranchless(vf, v1);
      SpeedCalcMinBranchless(vf, vf);

      SpeedCalcMaxBranchless(v0, v0);
      SpeedCalcMaxBranchless(v0, v1);
      SpeedCalcMaxBranchless(v0, vf);
      SpeedCalcMaxBranchless(v1, v0);
      SpeedCalcMaxBranchless(v1, v1);
      SpeedCalcMaxBranchless(v1, vf);
      SpeedCalcMaxBranchless(vf, v0);
      SpeedCalcMaxBranchless(vf, v1);
      SpeedCalcMaxBranchless(vf, vf);
    }

    static unsafe class Adder
    {
      public static int AddRef(byte* rp, byte* up, byte* vp, long n)
      {
        int carry = 0;
        for (long i = 0; i < n; i++)
        {
          int r = up[i] + vp[i] + carry;
          rp[i] = (byte)r;
          carry = r >> 8;
        }
        return carry;
      }

      public static ulong AddGmpLong5(ulong* rp, ulong* up, ulong* vp, long n)
      {
        ulong cy = 0;
        for (long i = 0; i < n; i++)
        {
          ulong ul = up[i];
          ulong vl = vp[i];
          ulong sl = ul + vl;
          ulong rl = sl + cy;
          rp[i] = rl;
          cy = sl < ul || rl < sl ? 1UL : 0UL;
        }
        return cy;
      }

      [DllImport("TaschenRechnerAsm.dll"), SuppressUnmanagedCodeSecurity]
      public static extern int GetAlignPointers(long[] pointers);

      [DllImport("TaschenRechnerAsm.dll"), SuppressUnmanagedCodeSecurity]
      public static extern ulong mpn_add_n_coreisbr(ulong* rp, ulong* up, ulong* vp, long n);

      [DllImport("TaschenRechnerAsm.dll"), SuppressUnmanagedCodeSecurity]
      public static extern long UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n);
    }

    //const int BitCount = 64;
    //const int RefResult = -1235051981;

    //const int BitCount = 64 * 2;
    //const int RefResult = -1826782628;

    //const int BitCount = 64 * 3;
    //const int RefResult = -2036864205;

    //const int BitCount = 64 * 4;
    //const int RefResult = -453152164;

    //const int BitCount = 64 * 5;
    //const int RefResult = -1163198925;

    //const int BitCount = 64 * 6;
    //const int RefResult = -2118735780;

    //const int BitCount = 64 * 7;
    //const int RefResult = -47983821;

    //const int BitCount = 64 * 8;
    //const int RefResult = 52304988;

    //const int BitCount = 64 * 31;
    //const int RefResult = -1175202428;

    //const int BitCount = 1024;
    //const int RefResult = 1995198812;

    const int BitCount = 65536;
    const int RefResult = 951296797;

    //const int BitCount = 1048576;
    //const int RefResult = -240413923;

    //const int BitCount = 10000000;
    //const int RefResult = -1524706991;

    //const int BitCount = 5;
    //const int RefResult = 505297501;

    const int ByteCount = BitCount >= 64 ? BitCount / 8 : 1024 + 8;

    static byte[] GetBytes(int addType = 0)
    {
      return Enumerable.Range(0, ByteCount).Select(x => (byte)x).ToArray();
    }
    static ulong[] BytesToUlong(byte[] values)
    {
      var result = new ulong[values.Length / sizeof(ulong)];
      Buffer.BlockCopy(values, 0, result, 0, values.Length);
      return result;
    }
    static byte[] UlongToBytes(ulong[] values)
    {
      var result = new byte[values.Length * sizeof(ulong)];
      Buffer.BlockCopy(values, 0, result, 0, result.Length);
      return result;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static unsafe void CalcSpeedCounter(ulong* rp, ulong* up, ulong* vp)
    {
      for (long i = 0; i < (BitCount >= 64 ? 10000000 / BitCount * 1024 : 5000000); i++)
      {
        //   full       1     31    1024   65536  10 M
        // 130,05  597,83  81,70   97,83   52,65  116,19
        // 133,46  551,99  85,57  106,44   52,75  116,97
        // 
        Adder.UIntX_Add(rp, up, vp, BitCount >= 64 ? ByteCount / sizeof(ulong) : (i & (ByteCount - 8) / sizeof(ulong) - 1) + 1);

        // 143,35  672,49  80,47  102,71   61,93  108,66
        //Adder.mpn_add_n_coreisbr(rp, up, vp, BitCount >= 64 ? ByteCount / sizeof(ulong) : (i & (ByteCount - 8) / sizeof(ulong) - 1) + 1);
      }
    }

    static unsafe void SpeedCalcAddArray()
    {
      const int RetryCount = 40;

      Console.WriteLine();
      Console.WriteLine("  - add " + (GetBytes().Length * 8) + " bits - ref -");
      Console.WriteLine();
      double bestTime = double.MaxValue;
      for (int r = 0; r < RetryCount; r++)
      {
        var res = BytesToUlong(GetBytes());
        var u = BytesToUlong(GetBytes(1));
        var v = BytesToUlong(GetBytes(2));
        if (res == null || u == null || v == null || res.Length < ByteCount / sizeof(ulong) || u.Length < ByteCount / sizeof(ulong) || v.Length < ByteCount / sizeof(ulong)) throw new IndexOutOfRangeException();
        Stopwatch m;
        fixed (ulong* rp = res, up = u, vp = v)
        {
          m = Stopwatch.StartNew();

          //CallSpeedCounter();
          CalcSpeedCounter(rp, up, vp);

          m.Stop();
        }
        double time = m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency;
        if (time < bestTime)
        {
          bestTime = time;
          Console.ForegroundColor = ConsoleColor.Yellow;
        }
        Console.WriteLine("    " + string.Concat(UlongToBytes(res).Select(c => c.ToString("x"))).GetHashCode().ToString().Replace(RefResult.ToString(), "ok") + ": " + (time).ToString("N2") + " ms");
        Console.ForegroundColor = ConsoleColor.Gray;
      }
    }

    static void SpeedCalcMemoryAllocation()
    {
      const int RetryCount = 10;
      const int TestCount = 20000000;
      const int len = 6;


      Console.WriteLine();
      Console.WriteLine("  - memory limb-allocation " + len + " limbs -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        ulong sum = 0;
        for (int i = 0; i < TestCount; i++)
        {
          var limbs = new uint[len];
          limbs[0] = 123;
          limbs[limbs.Length - 1] = 456;
          sum += limbs[0] + limbs[limbs.Length - 1];
        }
        //    ulong | uint
        // 1: 111   | 111
        // 2: 118   | 111
        // 3: 144   | 118
        // 4: 134   | 118
        // 5: 145   | 127
        // 6: 147   | 127
        // 7: 155
        // 8: 162
        // 9: 172
        // 10: 177
        // 11: 188
        // 12: 190
        // 13: 196
        // 14: 201
        // 15: 211
        // 16: 219

        m.Stop();
        Console.WriteLine("    " + sum + " : " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static int PointerAlign(long pointer)
    {
      int result = 1;
      while ((pointer & 1) == 0 && pointer != 0)
      {
        result <<= 1;
        pointer >>= 1;
      }
      return result;
    }

    static void PointerCheck()
    {
      var p = new long[256];
      int count = Adder.GetAlignPointers(p);
      for (int i = 0; i < count; i++)
      {
        //Console.WriteLine("  0x" + p[i].ToString("x").PadLeft(16, '0') + " (" + (-p[i] & 15) + " | " + (-p[i] & 31) + " | " + (-p[i] & 63) + ")");
        long pp = p[i];
        Console.Write("  0x" + pp.ToString("x").PadLeft(16, '0') + " -");
        int pmask = (int)(pp & 63);
        pmask = 36;
        for (int m = 0; m < 64; m++)
        {
          if ((m & 15) == 0) Console.Write(' ');
          Console.ForegroundColor = ConsoleColor.DarkGray;
          if (m >= pmask)
          {
            switch (pmask)
            {
              case 32:
              case 33:
              case 34:
              case 35:
              case 36: Console.ForegroundColor = ConsoleColor.White; break;
              case 37:
              case 38:
              case 39:
              case 40: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
              case 41:
              case 42:
              case 43:
              case 44: Console.ForegroundColor = ConsoleColor.DarkRed; break;
              case 45:
              case 46:
              case 47: Console.ForegroundColor = ConsoleColor.Red; break;

              case 48:
              case 49:
              case 50:
              case 51:
              case 52: Console.ForegroundColor = ConsoleColor.Yellow; break;
              case 53:
              case 54:
              case 55:
              case 56: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
              case 57:
              case 58:
              case 59:
              case 60: Console.ForegroundColor = ConsoleColor.DarkRed; break;
              case 61:
              case 62:
              case 63: Console.ForegroundColor = ConsoleColor.Red; break;
            }
          }
          Console.Write('#');
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();
      }
    }

    static unsafe void SpeedCalcMemCpy(long limbCount)
    {
      const int RetryCount = 20;

      long limit = 500000000L / limbCount;
      if (limbCount < 256) limit >>= 1;
      if (limbCount < 16) limit >>= 1;
      Console.WriteLine();
      Console.WriteLine("  - memcpy " + limbCount.ToString("N0") + " limbs, " + (limbCount * sizeof(ulong) / 1024.0).ToString("N2") + " KByte - (" + (limbCount * limit * sizeof(ulong) / 1048576.0).ToString("N0") + " MB)");
      Console.WriteLine();
      double bestTime = double.MaxValue;
      var bufDst = new ulong[limbCount];
      var bufSrc = new ulong[limbCount];

      for (int r = 0; r < RetryCount; r++)
      {
        Stopwatch m;
        fixed (ulong* dst = bufDst, src = bufSrc)
        {
          m = Stopwatch.StartNew();

          SpeedCalcMemCpy(dst, src, limbCount, limit);

          m.Stop();
        }
        double time = m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency;
        if (time < bestTime)
        {
          bestTime = time;
          Console.ForegroundColor = ConsoleColor.Yellow;
        }
        Console.WriteLine("   " + time.ToString("N2") + " ms");
        Console.ForegroundColor = ConsoleColor.Gray;
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static unsafe void SpeedCalcMemCpy(ulong* dst, ulong* src, long limbCount, long limit)
    {
      for (long i = 0; i < limit; i++)
      {
        // AsmWrapper.UIntX_Copy_Ref(dst, src, limbCount);
        // AsmWrapper.UIntX_Copy_Opti(dst, src, limbCount);
        AsmWrapper.UIntX_Copy(dst, src, limbCount);
        // AsmWrapper.mpn_copyd(dst, src, limbCount);
        // AsmWrapper.mpn_copyd_sse(dst, src, limbCount);
      }
    }

    static void SpeedCalcMemCpy()
    {
      // for (int i = 1; i < 16; i++) SpeedCalcMemCpy(i);
      // SpeedCalcMemCpy(1);
      // SpeedCalcMemCpy(15);
      // SpeedCalcMemCpy(16);
      // SpeedCalcMemCpy(256);
      // SpeedCalcMemCpy(1024);
      // SpeedCalcMemCpy(4096);
      // SpeedCalcMemCpy(16384);
      // SpeedCalcMemCpy(65536);
      // SpeedCalcMemCpy(262144);
      // SpeedCalcMemCpy(1048576);
    }

    static void SpeedCalc()
    {
      PointerCheck();

      Console.WriteLine();
      Console.WriteLine("  --- Test Calc ---");

      //SpeedCalcNotNullBranched();
      //SpeedCalcNotNullBrancheless();

      //SpeedCalcMinMaxBranched();

      SpeedCalcAddArray();

      //SpeedCalcMemCpy();

      //SpeedCalcMemoryAllocation();
    }
  }
}
