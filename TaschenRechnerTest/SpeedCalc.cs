#region # using *.*
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using TaschenRechnerLib;

// ReSharper disable ConvertToConstant.Local
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMethodReturnValue.Local
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
      public static ulong AddXtr(uint* rp, uint* up, uint* vp, long n)
      {
        return Xtr.Add(rp, up, vp, n);
      }

      public static ulong AddXtr2(uint* rp, uint* up, uint* vp, long n)
      {
        ulong carry = 0;
        for (long i = 0; i < n; i += 2)
        {
          ulong v1 = *(ulong*)(up + i);
          ulong v2 = *(ulong*)(vp + i);
          carry = (ulong)(uint)v1 + (uint)v2 + (carry >> 32);
          rp[i] = (uint)carry;
          carry = (v1 >> 32) + (v2 >> 32) + (carry >> 32);
          rp[i + 1] = (uint)carry;
        }
        return carry >> 32;
      }

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

      public static byte AddGmpByte(byte* rp, byte* up, byte* vp, long n)
      {
        byte cy = 0;
        do
        {
          byte ul = *up++;
          byte vl = *vp++;
          byte sl = (byte)(ul + vl);
          byte cy1 = sl < ul ? (byte)1 : (byte)0;
          byte rl = (byte)(sl + cy);
          byte cy2 = rl < sl ? (byte)1 : (byte)0;
          cy = (byte)(cy1 | cy2);
          *rp++ = rl;
        }
        while (--n != 0);
        return cy;
      }

      public static int AddGmpByteX(byte* rp, byte* up, byte* vp, long n)
      {
        int cy = 0;
        do
        {
          byte ul = *up++;
          byte vl = *vp++;
          int rl = ul + vl + cy;
          cy = rl >> 8;
          *rp++ = (byte)rl;
        }
        while (--n != 0);
        return cy;
      }

      public static ushort AddGmpShort(ushort* rp, ushort* up, ushort* vp, long n)
      {
        ushort cy = 0;
        do
        {
          ushort ul = *up++;
          ushort vl = *vp++;
          ushort sl = (ushort)(ul + vl);
          ushort cy1 = sl < ul ? (ushort)1 : (ushort)0;
          ushort rl = (ushort)(sl + cy);
          ushort cy2 = rl < sl ? (ushort)1 : (ushort)0;
          cy = (ushort)(cy1 | cy2);
          *rp++ = rl;
        }
        while (--n != 0);
        return cy;
      }

      public static int AddGmpShortX(ushort* rp, ushort* up, ushort* vp, long n)
      {
        int cy = 0;
        do
        {
          ushort ul = *up++;
          ushort vl = *vp++;
          int rl = ul + vl + cy;
          cy = rl >> 16;
          *rp++ = (ushort)rl;
        }
        while (--n != 0);
        return cy;
      }

      public static uint AddGmpInt(uint* rp, uint* up, uint* vp, long n)
      {
        uint cy = 0;
        do
        {
          uint ul = *up++;
          uint vl = *vp++;
          uint sl = ul + vl;
          uint cy1 = sl < ul ? 1u : 0u;
          uint rl = sl + cy;
          uint cy2 = rl < sl ? 1u : 0u;
          cy = cy1 | cy2;
          *rp++ = rl;
        }
        while (--n != 0);
        return cy;
      }

      public static ulong AddGmpIntX(uint* rp, uint* up, uint* vp, long n)
      {
        ulong cy = 0;
        do
        {
          uint ul = *up++;
          uint vl = *vp++;
          ulong rl = (ulong)ul + vl + cy;
          cy = rl >> 32;
          *rp++ = (uint)rl;
        }
        while (--n != 0);
        return cy;
      }

      public static ulong AddGmpLong(ulong* rp, ulong* up, ulong* vp, long n)
      {
        ulong cy = 0;
        do
        {
          ulong ul = *up++;
          ulong vl = *vp++;
          ulong sl = ul + vl;
          ulong cy1 = sl < ul ? 1UL : 0UL;
          ulong rl = sl + cy;
          ulong cy2 = rl < sl ? 1UL : 0UL;
          cy = cy1 | cy2;
          *rp++ = rl;
        }
        while (--n != 0);
        return cy;
      }

      public static ulong AddGmpLong2(ulong* rp, ulong* up, ulong* vp, long n)
      {
        var cy1 = default(HackBool);
        var cy2 = default(HackBool);
        ulong cy = 0;
        do
        {
          ulong ul = *up++;
          ulong vl = *vp++;
          ulong sl = ul + vl;
          cy1.vBool = sl < ul;
          ulong rl = sl + cy;
          cy2.vBool = rl < sl;
          cy = cy1.vULong | cy2.vULong;
          *rp++ = rl;
        }
        while (--n != 0);
        return cy;
      }

      [StructLayout(LayoutKind.Explicit, Size = 8)]
      struct HackBool
      {
        [FieldOffset(0)]
        public bool vBool;
        [FieldOffset(0)]
        public byte vByte;
        [FieldOffset(0)]
        public int vInt;
        [FieldOffset(0)]
        public uint vUInt;
        [FieldOffset(0)]
        public long vLong;
        [FieldOffset(0)]
        public ulong vULong;
      }

      public static ulong AddGmpLong3(ulong* rp, ulong* up, ulong* vp, long n)
      {
        ulong* cyb = stackalloc ulong[2];
        ulong cy = 0;
        do
        {
          ulong ul = *up++;
          ulong vl = *vp++;
          ulong sl = ul + vl;
          *(bool*)(cyb + 0) = sl < ul;
          ulong rl = sl + cy;
          *(bool*)(cyb + 1) = rl < sl;
          cy = cyb[0] | cyb[1];
          *rp++ = rl;
        }
        while (--n != 0);
        return cy;
      }

      public static ulong AddGmpLong4(ulong* rp, ulong* up, ulong* vp, long n)
      {
        ulong cy = 0;
        for (long i = 0; i < n; i++)
        {
          ulong ul = up[i];
          ulong vl = vp[i];
          ulong sl = ul + vl;
          ulong cy1 = sl < ul ? 1UL : 0UL;
          ulong rl = sl + cy;
          ulong cy2 = rl < sl ? 1UL : 0UL;
          cy = cy1 | cy2;
          rp[i] = rl;
        }
        return cy;
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
      public static extern ulong AddAsm(ulong* rp, ulong* up, ulong* vp, long n);

      [DllImport("TaschenRechnerAsm.dll"), SuppressUnmanagedCodeSecurity]
      public static extern ulong AddAsmX2(ulong* rp, ulong* up, ulong* vp, long n);

      [DllImport("TaschenRechnerAsm.dll"), SuppressUnmanagedCodeSecurity]
      public static extern ulong mpn_add_n(ulong* rp, ulong* up, ulong* vp, long n);
    }

    const int BitCount = 128;
    const int RefResult = -1826782628;

    //const int BitCount = 1024;
    //const int RefResult = 1995198812;

    //const int BitCount = 65536;
    //const int RefResult = 951296797;

    //const int BitCount = 1048576;
    //const int RefResult = -240413923;

    //const int BitCount = 10000000;
    //const int RefResult = -1524706991;

    const int ByteCount = BitCount / 8;

    static byte[] GetBytes(int addType = 0)
    {
      return Enumerable.Range(0, ByteCount).Select(x => (byte)x).ToArray();
    }

    static unsafe void SpeedCalcAddArray()
    {
      const int RetryCount = 10;
      const int TestCount = 10000000;

      Console.WriteLine();
      Console.WriteLine("  - add " + (GetBytes().Length * 8) + " bits - ref -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        var res = GetBytes();
        var u = GetBytes(1);
        var v = GetBytes(2);
        fixed (byte* rp = res, up = u, vp = v)
        {
          for (int i = 0; i < TestCount / BitCount * 1024; i++)
          {
            //                                         1024 Bits / 65536 Bits
            // Adder.AddRef(rp, up, vp, ByteCount); // 1.269,12 ms / 1.195,87 ms
            // Adder.AddGmpByte(rp, up, vp, ByteCount); // 2.824,51 ms / 2.816,84 ms
            // Adder.AddGmpByteX(rp, up, vp, ByteCount); // 1.611,47 ms / 1.551,23 ms
            // Adder.AddGmpShort((ushort*)rp, (ushort*)up, (ushort*)vp, ByteCount / sizeof(ushort)); // 1.486,16 ms / 1.451,76 ms
            // Adder.AddGmpShortX((ushort*)rp, (ushort*)up, (ushort*)vp, ByteCount / sizeof(ushort)); // 978,99 ms / 853,07 ms
            // Adder.AddGmpInt((uint*)rp, (uint*)up, (uint*)vp, ByteCount / sizeof(uint)); // 684,38 ms / 635,75 ms
            // Adder.AddGmpIntX((uint*)rp, (uint*)up, (uint*)vp, ByteCount / sizeof(uint)); // 554,34 ms / 423,39 ms
            // Adder.AddGmpLong((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 354,95 ms / 371,12 ms
            // Adder.AddGmpLong2((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 1.078,49 ms / 1.020,44 ms
            // Adder.AddGmpLong3((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 1.046,07 ms / 990,21 ms
            // Adder.AddGmpLong4((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 341,16 ms / 352,28 ms

            //                                                                                   128 Bits  / 1024 Bits / 65536 Bits / 1048576 Bits / 10000000 Bits
            //Adder.AddGmpLong5((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 310,44 ms / 239,66 ms / 193,80 ms  / 192,84 ms    / 262,96 ms

            // Adder.AddXtr((uint*)rp, (uint*)up, (uint*)vp, ByteCount / sizeof(uint)); // 405,74 ms / 216,07 ms / 188,70 ms / 197,21 ms / 257,31 ms
            // Adder.AddXtr2((uint*)rp, (uint*)up, (uint*)vp, ByteCount / sizeof(uint)); // 394,00 ms / 214,13 ms / 188,05 ms / 196,77 ms / 258,50 ms

            // Adder.AddAsm((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 319,53 ms / 110,96 ms / 82,20 ms / 119,47 ms / 176,06 ms
            // Adder.AddAsmX2((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 309,96 ms / 102,06 ms / 79,15 ms / 118,15 ms / 174,29 ms
            Adder.mpn_add_n((ulong*)rp, (ulong*)up, (ulong*)vp, ByteCount / sizeof(ulong)); // 369,67 ms / 104,81 ms / 69,25 ms / 113,34 ms / 169,54 ms
          }
        }
        m.Stop();
        Console.WriteLine("    " + string.Concat(res.Select(c => c.ToString("x"))).GetHashCode().ToString().Replace(RefResult.ToString(), "ok") + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
    }

    static void SpeedCalc()
    {
      Console.WriteLine();
      Console.WriteLine("  --- Test Calc ---");

      //SpeedCalcNotNullBranched();
      //SpeedCalcNotNullBrancheless();

      //SpeedCalcMinMaxBranched();

      SpeedCalcAddArray();
    }
  }
}
