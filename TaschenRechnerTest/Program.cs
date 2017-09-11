using System.Diagnostics;
using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  class Program
  {
    static void Constructor()
    {
      var val01 = new UIntSimple(0);
      var val02 = new UIntSimple(1);
      var val03 = new UIntSimple(9);
      var val04 = new UIntSimple(10);
      var val05 = new UIntSimple(12345);
      var val06 = new UIntSimple(1234567890);
      var val07 = new UIntSimple(12345678901234567890L);
      var val08 = new UIntSimple(sbyte.MaxValue);
      var val09 = new UIntSimple(byte.MaxValue);
      var val10 = new UIntSimple(short.MaxValue);
      var val11 = new UIntSimple(ushort.MaxValue);
      var val12 = new UIntSimple(int.MaxValue);
      var val13 = new UIntSimple(uint.MaxValue);
      var val14 = new UIntSimple(long.MaxValue);
      var val15 = new UIntSimple(ulong.MaxValue);
      var val16 = UIntSimple.Parse("0");
      var val17 = UIntSimple.Parse("1");
      var val18 = UIntSimple.Parse("100");
      var val19 = UIntSimple.Parse("12345678901234567890");
      var val20 = UIntSimple.Parse(new string('9', 1000000));
    }

    static void Add()
    {
      var val01 = new UIntSimple(123);
      var val02 = new UIntSimple(456);
      var val03 = val01 + val02; // 579
      var val04 = new UIntSimple(99);
      var val05 = val01 + val04; // 222
      var val06 = val04 + val02; // 555
      var val07 = val01 + UIntSimple.Zero; // 123
      var val08 = val01 + UIntSimple.One;  // 124
      var val09 = val01 + UIntSimple.Two;  // 125
      var val10 = new UIntSimple(49999);
      var val11 = new UIntSimple(50005);
      var val12 = val10 + val10; // 99998
      var val13 = val10 + val11; // 100004
      var val14 = val11 + val11; // 100010
      var val15 = new UIntSimple(999999999);
      var val16 = UIntSimple.One + val15; // 1000000000
      var val17 = val15 + UIntSimple.One; // 1000000000

      var val18 = UIntSimple.One;
      for (int i = 0; i < 8; i++) val18 += val18;
      // val18 == 256
      for (int i = 8; i < 16; i++) val18 += val18;
      // val18 == 65536
      for (int i = 16; i < 32; i++) val18 += val18;
      // val18 == 4294967296
      for (int i = 32; i < 64; i++) val18 += val18;
      // val18 == 18446744073709551616
      for (int i = 64; i < 128; i++) val18 += val18;
      // val18 == 340282366920938463463374607431768211456
      Debug.Assert(val18.ToString() == "340282366920938463463374607431768211456");
    }

    static void Main(string[] args)
    {
      // Constructor();
      Add();
    }
  }
}
