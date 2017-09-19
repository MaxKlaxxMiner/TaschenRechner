using System;
using System.Diagnostics;
using TaschenRechnerLib;
using TaschenRechnerLib.UIntX.Core;

namespace TaschenRechnerTest
{
  static unsafe partial class Program
  {
    static void MemTest()
    {
      var p1 = MemMgr.Alloc(11);
      var p2 = MemMgr.Alloc(11);
      if (p1 + 32 != p2) throw new Exception("?");
      var p3 = MemMgr.Alloc(32);
      if (p2 + 32 != p3) throw new Exception("?");

      var p4 = MemMgr.Alloc(33); // need new internal block
      if (p3 + 32 == p4) throw new Exception("?");

      var p5 = MemMgr.Alloc(32);
      if (p3 + 32 != p5) throw new Exception("?");
    }
  }
}
