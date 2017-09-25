#region # using *.*
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
// ReSharper disable ConvertToConstant.Local
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming
#pragma warning disable 169
#pragma warning disable 414
#endregion

namespace TaschenRechnerTest
{
  static partial class Program
  {
    const long ValueTest = 123;
    static readonly long ValueStatic = CreateValue();

    static long CreateValue()
    {
      if (Environment.TickCount != 0) return ValueTest; // always true, but must decide at runtime
      return 0;
    }

    sealed class MemberTestProperty
    {
      public long Value { get; set; }
    }

    sealed class MemberTestField
    {
      public long value;
    }

    sealed class MemberTestPropertyPrivate
    {
      public long Value { get; private set; }
      public MemberTestPropertyPrivate(long value) { Value = value; }
    }

    sealed class MemberTestFieldReadonly
    {
      public readonly long value;
      public MemberTestFieldReadonly(long value) { this.value = value; }
    }

    struct MemberTestStruct
    {
      public long value;
    }

    struct MemberTestStructReadonly
    {
      public readonly long value;
      public MemberTestStructReadonly(long value) { this.value = value; }
    }

    static class MemberStatic
    {
      public static long Value = ValueTest;
      public static readonly long ValueReadonly = ValueTest;
      public const long ValueConst = ValueTest;
    }

    static MemberTestProperty mGlobalProperty = new MemberTestProperty { Value = ValueTest };
    static MemberTestField mGlobalField = new MemberTestField { value = ValueTest };
    static MemberTestPropertyPrivate mGlobalPropertyPrivate = new MemberTestPropertyPrivate(ValueTest);
    static MemberTestFieldReadonly mGlobalFieldReadonly = new MemberTestFieldReadonly(ValueTest);

    static readonly MemberTestProperty msGlobalProperty = new MemberTestProperty { Value = ValueTest };
    static readonly MemberTestField msGlobalField = new MemberTestField { value = ValueTest };
    static readonly MemberTestPropertyPrivate msGlobalPropertyPrivate = new MemberTestPropertyPrivate(ValueTest);
    static readonly MemberTestFieldReadonly msGlobalFieldReadonly = new MemberTestFieldReadonly(ValueTest);

    static MemberTestStruct mGlobalStruct = new MemberTestStruct { value = ValueTest };
    static readonly MemberTestStruct msGlobalStruct = new MemberTestStruct { value = ValueTest };
    static MemberTestStructReadonly mGlobalStructReadonly = new MemberTestStructReadonly(ValueTest);
    static readonly MemberTestStructReadonly msGlobalStructReadonly = new MemberTestStructReadonly(ValueTest);

    static void SpeedValues()
    {
      const int RetryCount = 10;
      const int TestCount = 300000000;

      Console.WriteLine();
      Console.WriteLine("  --- Test Member ---");

      #region # // --- global instance ---
      //Console.WriteLine();
      //Console.WriteLine("  - global instance - property -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mGlobalProperty.Value + mGlobalProperty.Value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global instance - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mGlobalField.value + mGlobalField.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global instance - private property -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mGlobalPropertyPrivate.Value + mGlobalPropertyPrivate.Value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global instance - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mGlobalFieldReadonly.value + mGlobalFieldReadonly.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}
      #endregion

      #region # // --- global readonly instance ---
      //Console.WriteLine();
      //Console.WriteLine("  - global readonly instance - property -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += msGlobalProperty.Value + msGlobalProperty.Value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global readonly instance - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += msGlobalField.value + msGlobalField.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global readonly instance - private property -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += msGlobalPropertyPrivate.Value + msGlobalPropertyPrivate.Value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global readonly instance - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += msGlobalFieldReadonly.value + msGlobalFieldReadonly.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}
      #endregion

      #region # // --- local instance ---
      //Console.WriteLine();
      //Console.WriteLine("  - local instance - property -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var mLocalProperty = mGlobalProperty;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mLocalProperty.Value + mLocalProperty.Value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - local instance - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var mlocalField = mGlobalField;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mlocalField.value + mlocalField.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - local instance - private property -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var mLocalPropertyPrivate = mGlobalPropertyPrivate;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mLocalPropertyPrivate.Value + mLocalPropertyPrivate.Value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - local instance - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var mLocalFieldReadonly = mGlobalFieldReadonly;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mLocalFieldReadonly.value + mLocalFieldReadonly.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}
      #endregion

      #region # // --- global struct ---
      //Console.WriteLine();
      //Console.WriteLine("  - global struct - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mGlobalStruct.value + mGlobalStruct.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global readonly struct - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += msGlobalStruct.value + msGlobalStruct.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global struct - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mGlobalStructReadonly.value + mGlobalStructReadonly.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - global readonly struct - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += msGlobalStructReadonly.value + msGlobalStructReadonly.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}
      #endregion

      #region # // --- local struct ---
      //Console.WriteLine();
      //Console.WriteLine("  - local struct - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var mLocalStruct = mGlobalStruct;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mLocalStruct.value + mLocalStruct.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - local readonly struct - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var msLocalStruct = msGlobalStruct;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += msLocalStruct.value + msLocalStruct.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - local struct - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var mLocalStructReadonly = mGlobalStructReadonly;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mLocalStructReadonly.value + mLocalStructReadonly.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - local readonly struct - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  var mslocalStructReadonly = msGlobalStructReadonly;
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += mslocalStructReadonly.value + mslocalStructReadonly.value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}
      #endregion

      #region # // --- static ---
      //Console.WriteLine();
      //Console.WriteLine("  - static - field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += MemberStatic.Value + MemberStatic.Value;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - static - readonly field -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += MemberStatic.ValueReadonly + MemberStatic.ValueReadonly;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}

      //Console.WriteLine();
      //Console.WriteLine("  - static - const -");
      //Console.WriteLine();
      //for (int r = 0; r < RetryCount; r++)
      //{
      //  long l = 0;
      //  var m = Stopwatch.StartNew();
      //  for (int i = 0; i < TestCount; i++)
      //  {
      //    l += MemberStatic.ValueConst + MemberStatic.ValueConst;
      //  }
      //  m.Stop();
      //  Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      //}
      #endregion

      #region # // --- hacks ---
      Console.WriteLine();
      Console.WriteLine("  - local variable -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long l = 0;
        long value = CreateValue();
        var m = Stopwatch.StartNew();
        for (int i = 0; i < TestCount; i++)
        {
          l += value + value;
        }
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - local variable + if-hack -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long l = 0;
        long value = CreateValue();
        var m = Stopwatch.StartNew();
        if (value == ValueTest)
        {
          for (int i = 0; i < TestCount; i++)
          {
            l += value + value;
          }
        }
        else
        {
          for (int i = 0; i < TestCount; i++)
          {
            l += value + value;
          }
        }
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - normal method -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long value = CreateValue();
        var m = Stopwatch.StartNew();
        long l = MemberMethodNoInline(value, value, TestCount);
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - inline method -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long value = CreateValue();
        var m = Stopwatch.StartNew();
        long l = MemberMethodInline(value, value, TestCount);
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - normal method + if-hack -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long l;
        long value = CreateValue();
        var m = Stopwatch.StartNew();
        if (value == ValueTest)
        {
          l = MemberMethodNoInline(value, value, TestCount);
        }
        else
        {
          l = MemberMethodNoInline(value, value, TestCount);
        }
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - inline method + if-hack -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        long l;
        long value = CreateValue();
        var m = Stopwatch.StartNew();
        if (value == ValueTest)
        {
          l = MemberMethodInline(value, value, TestCount);
        }
        else
        {
          l = MemberMethodInline(value, value, TestCount);
        }
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - inline method + const -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        long l = MemberMethodInline(ValueTest, ValueTest, TestCount);
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }

      Console.WriteLine();
      Console.WriteLine("  - inline method + static readonly -");
      Console.WriteLine();
      for (int r = 0; r < RetryCount; r++)
      {
        var m = Stopwatch.StartNew();
        long l = MemberMethodInline(ValueStatic, ValueStatic, TestCount);
        m.Stop();
        Console.WriteLine("    " + (l == TestCount * ValueTest * 2) + ": " + (m.ElapsedTicks * 1000 / (double)Stopwatch.Frequency).ToString("N2") + " ms");
      }
      #endregion
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static long MemberMethodInline(long value1, long value2, int testCount)
    {
      long l = 0;
      for (int i = 0; i < testCount; i++)
      {
        l += value1 + value2;
      }
      return l;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static long MemberMethodNoInline(long value1, long value2, int testCount)
    {
      long l = 0;
      for (int i = 0; i < testCount; i++)
      {
        l += value1 + value2;
      }
      return l;
    }
  }
}
