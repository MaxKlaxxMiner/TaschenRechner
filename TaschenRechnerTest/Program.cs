#region # using *.*
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;
using System.Collections.Generic;
// ReSharper disable RedundantAssignment
// ReSharper disable UnusedVariable
// ReSharper disable UnusedMember.Local
#endregion

//using ui = TaschenRechnerLib.UIntSimple;
using ui = TaschenRechnerLib.UIntLimbs;

namespace TaschenRechnerTest
{
  static partial class Program
  {
    static void Constructor()
    {
      var val01 = new ui(0);
      Debug.Assert(val01.ToString() == "0");
      var val02 = new ui(1);
      Debug.Assert(val02.ToString() == "1");
      var val03 = new ui(9);
      Debug.Assert(val03.ToString() == "9");
      var val04 = new ui(10);
      Debug.Assert(val04.ToString() == "10");
      var val05 = new ui(12345);
      Debug.Assert(val05.ToString() == "12345");
      var val06 = new ui(1234567890);
      Debug.Assert(val06.ToString() == "1234567890");
      var val07 = new ui(12345678901234567890L);
      Debug.Assert(val07.ToString() == "12345678901234567890");
      var val08 = new ui(sbyte.MaxValue);
      Debug.Assert(val08.ToString() == sbyte.MaxValue.ToString());
      var val09 = new ui(byte.MaxValue);
      Debug.Assert(val09.ToString() == byte.MaxValue.ToString());
      var val10 = new ui(short.MaxValue);
      Debug.Assert(val10.ToString() == short.MaxValue.ToString());
      var val11 = new ui(ushort.MaxValue);
      Debug.Assert(val11.ToString() == ushort.MaxValue.ToString());
      var val12 = new ui(int.MaxValue);
      Debug.Assert(val12.ToString() == int.MaxValue.ToString());
      var val13 = new ui(uint.MaxValue);
      Debug.Assert(val13.ToString() == uint.MaxValue.ToString());
      var val14 = new ui(long.MaxValue);
      Debug.Assert(val14.ToString() == long.MaxValue.ToString());
      var val15 = new ui(ulong.MaxValue);
      Debug.Assert(val15.ToString() == ulong.MaxValue.ToString());
      var val16 = ui.Parse("0");
      Debug.Assert(val16.ToString() == "0");
      var val17 = ui.Parse("1");
      Debug.Assert(val17.ToString() == "1");
      var val18 = ui.Parse("100");
      Debug.Assert(val18.ToString() == "100");
      var val19 = ui.Parse("12345678901234567890");
      Debug.Assert(val19.ToString() == "12345678901234567890");
      var val20 = ui.Parse(new string('9', 1000000));
      Debug.Assert(val20.ToString() == new string('9', 1000000));
      for (int i = 1; i < 100; i++)
      {
        string num = string.Concat(Enumerable.Range(1, i));
        var val21 = ui.Parse("        " + num + "       ");
        Debug.Assert(val21.ToString() == num);
      }
    }

    static void Add()
    {
      var val01 = new ui(123);
      Debug.Assert(val01.ToString() == "123");
      var val02 = new ui(456);
      Debug.Assert(val02.ToString() == "456");
      var val03 = val01 + val02;
      Debug.Assert(val03.ToString() == "579");
      var val04 = new ui(99);
      Debug.Assert(val04.ToString() == "99");
      var val05 = val01 + val04;
      Debug.Assert(val05.ToString() == "222");
      var val06 = val04 + val02;
      Debug.Assert(val06.ToString() == "555");
      var val07 = val01 + ui.Zero;
      Debug.Assert(val07.ToString() == "123");
      var val08 = val01 + ui.One;
      Debug.Assert(val08.ToString() == "124");
      var val09 = val01 + ui.Two;
      Debug.Assert(val09.ToString() == "125");
      var val10 = new ui(49999);
      Debug.Assert(val10.ToString() == "49999");
      var val11 = new ui(50005);
      Debug.Assert(val11.ToString() == "50005");
      var val12 = val10 + val10; // 99998
      Debug.Assert(val12.ToString() == "99998");
      var val13 = val10 + val11; // 100004
      Debug.Assert(val13.ToString() == "100004");
      var val14 = val11 + val11; // 100010
      Debug.Assert(val14.ToString() == "100010");
      var val15 = new ui(999999999);
      Debug.Assert(val15.ToString() == "999999999");
      var val16 = ui.One + val15; // 1000000000
      Debug.Assert(val16.ToString() == "1000000000");
      var val17 = val15 + ui.One; // 1000000000
      Debug.Assert(val17.ToString() == "1000000000");

      var val18 = ui.One;
      for (int i = 0; i < 8; i++) val18 += val18;
      Debug.Assert(val18.ToString() == "256");
      for (int i = 8; i < 16; i++) val18 += val18;
      Debug.Assert(val18.ToString() == "65536");
      for (int i = 16; i < 32; i++) val18 += val18;
      Debug.Assert(val18.ToString() == "4294967296");
      for (int i = 32; i < 64; i++) val18 += val18;
      Debug.Assert(val18.ToString() == "18446744073709551616");
      for (int i = 64; i < 128; i++) val18 += val18;
      Debug.Assert(val18.ToString() == "340282366920938463463374607431768211456");

      Debug.Assert(val04.ToString() == "99");
      val04++; // 100
      Debug.Assert(val04.ToString() == "100");
      val04++; // 101
      Debug.Assert(val04.ToString() == "101");

      Debug.Assert(val15.ToString() == "999999999");
      val15++; // 1000000000
      Debug.Assert(val15.ToString() == "1000000000");
    }

    static void Compare()
    {
      var val01 = new ui(123);
      var val02 = new ui(123);
      Debug.Assert(val01.CompareTo(val02) == 0);
      Debug.Assert(val02.CompareTo(val01) == 0);
      var val03 = new ui(ulong.MaxValue);
      var val04 = new ui(ulong.MaxValue);
      Debug.Assert(val03.CompareTo(val04) == 0);
      Debug.Assert(val04.CompareTo(val03) == 0);
      Debug.Assert(val01.CompareTo(val03) < 0);
      Debug.Assert(val03.CompareTo(val01) > 0);
      val02++;
      Debug.Assert(val01.CompareTo(val02) < 0);
      Debug.Assert(val02.CompareTo(val01) > 0);

      var list = new List<ui> { val04, val03, val02, val01 };
      list.Sort();
      Debug.Assert(list[0] == val01);
      Debug.Assert(list[1] == val02);
      Debug.Assert(list[2] == val03);
      Debug.Assert(list[3] == val04);
      Debug.Assert(val01 < val02);
      Debug.Assert(val02 < val03);
      Debug.Assert(val03 == val04);
      Debug.Assert(val04 > val01);

      int hash01 = val01.GetHashCode();
      int hash02 = val02.GetHashCode();
      Debug.Assert(hash01 != hash02);

      int hash03 = val03.GetHashCode();
      int hash04 = val04.GetHashCode();
      Debug.Assert(hash03 == hash04);
      Debug.Assert(val03.Equals((object)val04));
      Debug.Assert(!val03.Equals((object)val02));
    }

    static void Sub()
    {
      var val01 = new ui(123);
      Debug.Assert(val01.ToString() == "123");
      var val02 = new ui(456);
      Debug.Assert(val02.ToString() == "456");
      var val03 = val02 - val01;
      Debug.Assert(val03.ToString() == "333");
      var val04 = val03 - val01;
      Debug.Assert(val04.ToString() == "210");
      var val05 = val04 - val01;
      Debug.Assert(val05.ToString() == "87");
      var val06 = val02 - val05;
      Debug.Assert(val06.ToString() == "369");
      var val07 = val01 - val05;
      Debug.Assert(val07.ToString() == "36");
      var val08 = ui.Parse("1" + new string('0', 100));
      Debug.Assert(val08.ToString() == "1" + new string('0', 100));
      var val09 = val08 - ui.One;
      Debug.Assert(val09.ToString() == new string('9', 100));
      var val10 = val08 - val09;
      Debug.Assert(val10.ToString() == "1");
    }

    static void Mul()
    {
      var val01 = new ui(123);
      Debug.Assert(val01.ToString() == "123");
      var val02 = new ui(456);
      Debug.Assert(val02.ToString() == "456");
      var val03 = val02 * val02;
      Debug.Assert(val03.ToString() == "207936");
      var val04 = val01 * val01;
      Debug.Assert(val04.ToString() == "15129");
      var val05 = val01 * val02;
      Debug.Assert(val05.ToString() == "56088");
      var val06 = new ui(9999999999999999999UL);
      Debug.Assert(val06.ToString() == "9999999999999999999");
      var val07 = val06 * val06;
      Debug.Assert(val07.ToString() == "99999999999999999980000000000000000001");
      var val08 = new ui(new string('9', 1000));
      Debug.Assert(val08.ToString() == new string('9', 1000));
      var val09 = val08 * val08;
      Debug.Assert(val09.ToString() == new string('9', 999) + "8" + new string('0', 999) + "1");
      var val10 = val01 * val02 * val03 * val04 * val05;
      Debug.Assert(val10.ToString() == "9896458695813697536");
    }

    static void Main(string[] args)
    {
      //Constructor();
      //Add();
      //Compare();
      //Sub();
      //Mul();

      //SpeedDiv();
      //SpeedCheckStr();
      //SpeedCheckAdd();
      //SpeedCheckSub();
      SpeedCheckMul();
    }
  }
}
