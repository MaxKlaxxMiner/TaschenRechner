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
    }
  }
}
