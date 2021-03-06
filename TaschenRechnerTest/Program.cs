﻿#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using TaschenRechnerLib;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
// ReSharper disable RedundantAssignment
// ReSharper disable UnusedVariable
// ReSharper disable UnusedMember.Local
#endregion

//using ui = TaschenRechnerLib.UIntSimple;
//using ui = TaschenRechnerLib.UIntLimbs;
//using ui = TaschenRechnerLib.UIntBig;
//using ui = TaschenRechnerLib.UIntXs;
using ui = TaschenRechnerLib.UIntX;

namespace TaschenRechnerTest
{
  static unsafe partial class Program
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
      var val20 = ui.Parse(new string('9', 100000));
      Debug.Assert(val20.ToString() == new string('9', 100000));
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
      for (int i = 16; i < 31; i++) val18 += val18;
      val18 += val18;
      Debug.Assert(val18.ToString() == "4294967296");
      for (int i = 32; i < 63; i++) val18 += val18;
      val18 += val18;
      Debug.Assert(val18.ToString() == "18446744073709551616");
      for (int i = 64; i < 127; i++) val18 += val18;
      val18 += val18;
      Debug.Assert(val18.ToString() == "340282366920938463463374607431768211456");

      Debug.Assert(val04.ToString() == "99");
      val04++; // 100
      Debug.Assert(val04.ToString() == "100");
      val04++; // 101
      Debug.Assert(val04.ToString() == "101");

      Debug.Assert(val15.ToString() == "999999999");
      val15++; // 1000000000
      Debug.Assert(val15.ToString() == "1000000000");

      var val19 = new ui(uint.MaxValue);
      val19++;
      Debug.Assert(val19.ToString() == "4294967296");
      var val20 = new ui(ulong.MaxValue);
      ulong v1 = (ulong)val20;
      val20++;
      Debug.Assert(val20.ToString() == "18446744073709551616");
      val20 += 12345;
      Debug.Assert(val20.ToString() == "18446744073709563961");
      val20 += ulong.MaxValue;
      Debug.Assert(val20.ToString() == "36893488147419115576");
      val20 += ulong.MaxValue;
      Debug.Assert(val20.ToString() == "55340232221128667191");
    }

    static void Compare()
    {
      //var val01 = new ui(123);
      //var val02 = new ui(123);
      //Debug.Assert(val01.CompareTo(val02) == 0);
      //Debug.Assert(val02.CompareTo(val01) == 0);
      //var val03 = new ui(ulong.MaxValue);
      //var val04 = new ui(ulong.MaxValue);
      //Debug.Assert(val03.CompareTo(val04) == 0);
      //Debug.Assert(val04.CompareTo(val03) == 0);
      //Debug.Assert(val01.CompareTo(val03) < 0);
      //Debug.Assert(val03.CompareTo(val01) > 0);
      //val02++;
      //Debug.Assert(val01.CompareTo(val02) < 0);
      //Debug.Assert(val02.CompareTo(val01) > 0);

      //var list = new List<ui> { val04, val03, val02, val01 };
      //list.Sort();
      //Debug.Assert(list[0] == val01);
      //Debug.Assert(list[1] == val02);
      //Debug.Assert(list[2] == val03);
      //Debug.Assert(list[3] == val04);
      //Debug.Assert(val01 < val02);
      //Debug.Assert(val02 < val03);
      //Debug.Assert(val03 == val04);
      //Debug.Assert(val04 > val01);

      //int hash01 = val01.GetHashCode();
      //int hash02 = val02.GetHashCode();
      //Debug.Assert(hash01 != hash02);

      //int hash03 = val03.GetHashCode();
      //int hash04 = val04.GetHashCode();
      //Debug.Assert(hash03 == hash04);
      //// ReSharper disable once RedundantCast
      //Debug.Assert(val03.Equals((object)val04));
      //// ReSharper disable once RedundantCast
      //Debug.Assert(!val03.Equals((object)val02));
    }

    static void Sub()
    {
      //var val01 = new ui(123);
      //Debug.Assert(val01.ToString() == "123");
      //var val02 = new ui(456);
      //Debug.Assert(val02.ToString() == "456");
      //var val03 = val02 - val01;
      //Debug.Assert(val03.ToString() == "333");
      //var val04 = val03 - val01;
      //Debug.Assert(val04.ToString() == "210");
      //var val05 = val04 - val01;
      //Debug.Assert(val05.ToString() == "87");
      //var val06 = val02 - val05;
      //Debug.Assert(val06.ToString() == "369");
      //var val07 = val01 - val05;
      //Debug.Assert(val07.ToString() == "36");
      //var val08 = ui.Parse("1" + new string('0', 100));
      //Debug.Assert(val08.ToString() == "1" + new string('0', 100));
      //var val09 = val08 - ui.One;
      //Debug.Assert(val09.ToString() == new string('9', 100));
      //var val10 = val08 - val09;
      //Debug.Assert(val10.ToString() == "1");

      //var val11 = new ui(((BigInteger.One << 1024) - 1).ToString());
      //var val12 = new ui((BigInteger.One << 1024).ToString());
      //var val13 = val12 - val11;
      //Debug.Assert(val13.ToString() == "1");
      ////var val14 = val11 - val12; // error
      //val12++;
      //val12--;
      //val12--;
      //Debug.Assert(val11.ToString() == val12.ToString());

      //var val15 = new ui(3);
      //val15--;
      //val15--;
      //val15--;
      //Debug.Assert(val15.ToString() == "0");
      ////val15--; // error
    }

    static void Mul()
    {
      //var val01 = new ui(123);
      //Debug.Assert(val01.ToString() == "123");
      //var val02 = new ui(456);
      //Debug.Assert(val02.ToString() == "456");
      //var val03 = val02 * val02;
      //Debug.Assert(val03.ToString() == "207936");
      //var val04 = val01 * val01;
      //Debug.Assert(val04.ToString() == "15129");
      //var val05 = val01 * val02;
      //Debug.Assert(val05.ToString() == "56088");
      //var val06 = new ui(9999999999999999999UL);
      //Debug.Assert(val06.ToString() == "9999999999999999999");
      //var val07 = val06 * val06;
      //Debug.Assert(val07.ToString() == "99999999999999999980000000000000000001");
      //var val08 = new ui(new string('9', 1000));
      //Debug.Assert(val08.ToString() == new string('9', 1000));
      //var val09 = val08 * val08;
      //Debug.Assert(val09.ToString() == new string('9', 999) + "8" + new string('0', 999) + "1");
      //var val10 = val01 * val02 * val03 * val04 * val05;
      //Debug.Assert(val10.ToString() == "9896458695813697536");
      //val10 *= val10;
      //val10 *= val10;
      //var val11 = val10 * val10 * val09;
      //string cmp11 = (BigInteger.Parse(val10.ToString()) * BigInteger.Parse(val10.ToString()) * BigInteger.Parse(val09.ToString())).ToString();
      //Debug.Assert(val11.ToString() == cmp11);
    }

    static void Div()
    {
      //var val01 = new ui(12345);
      //Debug.Assert(val01.ToString() == "12345");
      //var val02 = new ui(15);
      //Debug.Assert(val02.ToString() == "15");
      //var val03 = val01 / val02;
      //Debug.Assert(val03.ToString() == "823");
      //var val04 = new ui(10000000000000000000ul);
      //Debug.Assert(val04.ToString() == "10000000000000000000");
      //var val05 = val04 / val03;
      //Debug.Assert(val05.ToString() == "12150668286755771");
      //var val06 = val05 / val03;
      //Debug.Assert(val06.ToString() == "14763873981477");
      //var val07 = val06 / val03;
      //Debug.Assert(val07.ToString() == "17939093537");
    }

    static void Mod()
    {
      //var val01 = new ui(12345);
      //Debug.Assert(val01.ToString() == "12345");
      //var val02 = new ui(15);
      //Debug.Assert(val02.ToString() == "15");
      //var val03 = val01 % val02;
      //Debug.Assert(val03.ToString() == "0");
      //val03 = new ui(823);
      //var val04 = new ui(10000000000000000000ul);
      //Debug.Assert(val04.ToString() == "10000000000000000000");
      //var val05 = val04 % val03;
      //Debug.Assert(val05.ToString() == "467");
      //var val06 = val04 % val05;
      //Debug.Assert(val06.ToString() == "139");
      //var val07 = new ui(1793909353712311ul);
      //Debug.Assert(val07.ToString() == "1793909353712311");
      //var val08 = val04 % val07;
      //Debug.Assert(val08.ToString() == "749262407578486");
      //var val09 = val04 + new ui(1044646946133826ul);
      //Debug.Assert(val09.ToString() == "10001044646946133826");
      //var val10 = val09 / val07;
      //Debug.Assert(val10.ToString() == "5575");
      //var val11 = val09 % val07;
      //Debug.Assert(val11.ToString() == "1");
    }

    static void Copy()
    {
      const int Len = 65536;
      var sourceBuf = Enumerable.Range(0, Len).Select(i => (ulong)i * 91UL + 19UL).ToArray();
      var targetBuf = new ulong[Len];
      fixed (ulong* target = targetBuf, source = sourceBuf)
      {
        for (long copyLen = 1; copyLen < Len - 256; copyLen += copyLen < 300 ? 1 : copyLen / 30)
        {
          Console.WriteLine(copyLen + " / 65000");
          for (long targetOffset = 0; targetOffset < 9; targetOffset++)
          {
            for (long sourceOffset = 0; sourceOffset < 9; sourceOffset++)
            {
              // --- prepare ---
              for (long i = 0; i < targetBuf.Length; i++) target[i] = ulong.MaxValue;

              AsmWrapper.UIntX_Copy(target + targetOffset, source + sourceOffset, copyLen);

              // --- check ---
              for (long i = 0; i < targetOffset; i++) if (target[i] != ulong.MaxValue) throw new Exception("err < targetOffset: " + new { copyLen, targetOffset, sourceOffset });
              for (long i = 0; i < copyLen; i++) if (target[targetOffset + i] != (ulong)(i + sourceOffset) * 91UL + 19UL) throw new Exception("err (room): " + new { copyLen, targetOffset, sourceOffset });
              for (long i = targetOffset + copyLen; i < Len; i++) if (target[i] != ulong.MaxValue) throw new Exception("err >= targetOffset + copyLen: " + new { copyLen, targetOffset, sourceOffset });
            }
          }
        }
      }
    }

    static void Main(string[] args)
    {
      // --- Funktionalitäten prüfen ---
      //Constructor();
      //Add();
      //Compare();
      //Sub();
      //Mul();
      //Div();
      //Mod();
      //Copy();

      // --- Geschwindigkeiten messen ---
      //SpeedCheckAdd();
      //SpeedCheckInc();
      //SpeedCheckSub();
      //SpeedCheckDec();
      //SpeedCheckMul();
      //SpeedCheckDiv();
      //SpeedCheckStr();

      // --- Extras prüfen ---
      //SpeedDiv();
      //SpeedValues();
      SpeedCalc();
      //MemTest();
    }
  }
}
