using System;
using System.Diagnostics;
using TaschenRechnerLib;
using TaschenRechnerLib.UIntX.Core;
// ReSharper disable UnusedMember.Local

namespace TaschenRechnerTest
{
  static unsafe partial class Program
  {
    static void MemTestSingles()
    {
      var p1 = MemMgr.Alloc(11);
      Debug.Assert(MemMgr.GetSize(p1) == 32);
      var p2 = MemMgr.Alloc(11);
      Debug.Assert(MemMgr.GetSize(p2) == 32);
      if (p1 + 32 != p2) throw new Exception("?");
      var p3 = MemMgr.Alloc(32);
      Debug.Assert(MemMgr.GetSize(p3) == 32);
      if (p2 + 32 != p3) throw new Exception("?");

      var p4 = MemMgr.Alloc(33); // need new internal block
      Debug.Assert(MemMgr.GetSize(p4) == 64);
      if (p3 + 32 == p4) throw new Exception("?");

      var p5 = MemMgr.Alloc(32);
      Debug.Assert(MemMgr.GetSize(p5) == 32);
      if (p3 + 32 != p5) throw new Exception("?");

      var p6 = MemMgr.Alloc(10000);
      Debug.Assert(MemMgr.GetSize(p6) == 16384);
      Debug.Assert(MemMgr.GetSize(p6 - 1) == 0);
      Debug.Assert(MemMgr.GetSize(p6 + 1) == 0);
      var p7 = MemMgr.Alloc(10000);
      Debug.Assert(MemMgr.GetSize(p7) == 16384);
      if (p6 + 16384 != p7) throw new Exception("?");
      Debug.Assert(MemMgr.GetSize(p6 + 16383) == 0);
      Debug.Assert(MemMgr.GetSize(p6 + 16384) == 16384);

      bool c1 = MemMgr.Free(p6);
      bool c2 = MemMgr.Free(p6);
      Debug.Assert(c1);
      Debug.Assert(!c2);
      Debug.Assert(MemMgr.GetSize(p6) == 0);
      Debug.Assert(MemMgr.GetSize(p7) == 16384);

      Debug.Assert(MemMgr.Free(p2));
      Debug.Assert(!MemMgr.Free(p2));

      Debug.Assert(MemMgr.GetSize(p1) == 32);
      Debug.Assert(MemMgr.GetSize(p2) == 0);
      Debug.Assert(MemMgr.GetSize(p3) == 32);
      Debug.Assert(MemMgr.GetSize(p4) == 64);
      Debug.Assert(MemMgr.GetSize(p5) == 32);

      Debug.Assert(MemMgr.Free(p1));
      Debug.Assert(!MemMgr.Free(p2));
      Debug.Assert(MemMgr.Free(p3));
      Debug.Assert(MemMgr.Free(p4));
      Debug.Assert(MemMgr.Free(p5));
      Debug.Assert(!MemMgr.Free(p6));
      Debug.Assert(MemMgr.Free(p7));
    }

    static void MemTestMulti(int len, int count = 1000)
    {
      var ps = new byte*[count];

      // --- 1000 Elemente reservieren ---
      for (int i = 0; i < ps.Length; i++) ps[i] = MemMgr.AllocUnsafe(len);

      // --- alle Elemente prüfen ---
      foreach (var p in ps) if (MemMgr.GetSize(p) == 0) throw new Exception();

      // --- jedes neunte Element freigeben ---
      for (int i = 0; i < ps.Length; i += 9) if (!MemMgr.Free(ps[i])) throw new Exception();

      // --- jedes neunte Element neu reservieren ---
      for (int i = 0; i < ps.Length; i += 9) ps[i] = MemMgr.AllocUnsafe(len);

      // --- alle Elemente wieder frei geben ---
      foreach (var p in ps) if (!MemMgr.Free(p)) throw new Exception();
    }

    static void MemTestMaxMem()
    {
      MemTestMulti(10);
      MemTestMulti(100);
      MemTestMulti(1000);
      MemTestMulti(10000);
      MemTestMulti(100000);
      // --- x64 4 GB ---
      MemTestMulti(1000000);
      // --- x64 6 GB ---
      MemTestMulti(10000000, 100);
      // --- x64 8 GB ---
      MemTestMulti(100000000, 10);
      // --- x64 12 GB ---
      MemTestMulti(1000000000, 5);
    }

    static void MemTestMaxElements()
    {
      MemTestMulti(32, 20);
      MemTestMulti(32, 200);
      MemTestMulti(32, 2000);
      MemTestMulti(32, 20000);
      MemTestMulti(32, 200000);
      MemTestMulti(32, 2000000);
      // --- x64 4 GB ---
      MemTestMulti(32, 20000000);
      // --- x64 12 GB ---
      MemTestMulti(32, 200000000);
    }

    static void MemTestSpeed()
    {
      var m = new Stopwatch();
      m.Start();
      MemTestMulti(32, 20);
      MemTestMulti(32, 200);
      MemTestMulti(32, 2000);
      MemTestMulti(32, 20000);
      MemTestMulti(32, 200000);
      MemTestMulti(32, 2000000);
      MemTestMulti(32, 20000000);
      m.Stop();
      Console.WriteLine("Time: " + m.ElapsedMilliseconds.ToString("N0"));
    }

    static void MemTest()
    {
      // MemTestSingles();
      // MemTestMaxMem();
      // MemTestMaxElements();
      for (int r = 0; r < 5; r++) MemTestSpeed();
    }
  }
}
