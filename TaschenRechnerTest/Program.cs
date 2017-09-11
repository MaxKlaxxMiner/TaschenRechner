using System.Diagnostics;
// ReSharper disable RedundantAssignment
// ReSharper disable UnusedVariable
// ReSharper disable UnusedMember.Local

//using ui = TaschenRechnerLib.UIntSimple;
using ui = TaschenRechnerLib.UIntLimbs;

namespace TaschenRechnerTest
{
  class Program
  {
    static void Constructor()
    {
      var val01 = new ui(0);
      var val02 = new ui(1);
      var val03 = new ui(9);
      var val04 = new ui(10);
      var val05 = new ui(12345);
      var val06 = new ui(1234567890);
      var val07 = new ui(12345678901234567890L);
      var val08 = new ui(sbyte.MaxValue);
      var val09 = new ui(byte.MaxValue);
      var val10 = new ui(short.MaxValue);
      var val11 = new ui(ushort.MaxValue);
      var val12 = new ui(int.MaxValue);
      var val13 = new ui(uint.MaxValue);
      var val14 = new ui(long.MaxValue);
      var val15 = new ui(ulong.MaxValue);
      //var val16 = ui.Parse("0");
      //var val17 = ui.Parse("1");
      //var val18 = ui.Parse("100");
      //var val19 = ui.Parse("12345678901234567890");
      //var val20 = ui.Parse(new string('9', 1000000));
    }

    static void Add()
    {
      //var val01 = new ui(123);
      //var val02 = new ui(456);
      //var val03 = val01 + val02; // 579
      //var val04 = new ui(99);
      //var val05 = val01 + val04; // 222
      //var val06 = val04 + val02; // 555
      //var val07 = val01 + ui.Zero; // 123
      //var val08 = val01 + ui.One;  // 124
      //var val09 = val01 + ui.Two;  // 125
      //var val10 = new ui(49999);
      //var val11 = new ui(50005);
      //var val12 = val10 + val10; // 99998
      //var val13 = val10 + val11; // 100004
      //var val14 = val11 + val11; // 100010
      //var val15 = new ui(999999999);
      //var val16 = ui.One + val15; // 1000000000
      //var val17 = val15 + ui.One; // 1000000000

      //var val18 = ui.One;
      //for (int i = 0; i < 8; i++) val18 += val18;
      //// val18 == 256
      //for (int i = 8; i < 16; i++) val18 += val18;
      //// val18 == 65536
      //for (int i = 16; i < 32; i++) val18 += val18;
      //// val18 == 4294967296
      //for (int i = 32; i < 64; i++) val18 += val18;
      //// val18 == 18446744073709551616
      //for (int i = 64; i < 128; i++) val18 += val18;
      //// val18 == 340282366920938463463374607431768211456
      //Debug.Assert(val18.ToString() == "340282366920938463463374607431768211456");

      //val04++; // 100
      //val04++; // 101
      //val15++; // 1000000000
    }

    static void Main(string[] args)
    {
      Constructor();
      Add();
    }
  }
}
