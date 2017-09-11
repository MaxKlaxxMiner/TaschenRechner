using TaschenRechnerLib;

namespace TaschenRechnerTest
{
  class Program
  {
    static void Main(string[] args)
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
  }
}
