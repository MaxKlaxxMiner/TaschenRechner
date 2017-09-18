// BigInt.cs (v 1.03)
// stephen swensen
// created march 2009
// updated may 2009
// c# 3.0, .net 3.5
// License: The Code Project Open License (CPOL) 1.02

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TaschenRechnerTest
{
  /// <summary>
  /// BigInt is a general-purpose unbounded integer implementation consistent with C# and .NET numeric type conventions
  /// </summary>
  public struct BigInt : IComparable<BigInt>, IEquatable<BigInt>, IConvertible
  {
    #region Instance Fields

    readonly LinkedList<byte> digits; //is null if and only if bigint is zero
    readonly bool isneg; //value irrelevant if digits is null

    #endregion Instance Fields

    #region Constructors

    //as is more natural for ValueTypes, we use explicit and and implicit conversion operators 
    //(and Parse for strings) in-lue of equivalent contructors (and to spare redundancy)

    /// <summary>
    /// shallow copy
    /// </summary>
    private BigInt(LinkedList<byte> digits, bool isneg)
    {
      this.digits = digits;
      this.isneg = isneg;
    }

    #endregion Constructors

    #region Static Fields

    //being common and useful privately, we deduce the same for the public

    public static readonly BigInt Zero = new BigInt(); //data null, isneg false
    public static readonly BigInt One = Parse("1");
    public static readonly BigInt NegativeOne = Parse("-1");
    public static readonly BigInt Two = Parse("2");

    #endregion Static Fields

    #region Instance Properties

    /// <summary>
    /// Traverse the unsigned digits of this BigInt, left to right.
    /// </summary>
    public IEnumerable<byte> DigitsLeftToRight //DigitsL2R so tempting!
    {
      get
      {
        if (digits == null) yield return 0;
        else
        {
          for (var cur = digits.First; cur != null; cur = cur.Next) yield return cur.Value;
        }
      }
    }

    /// <summary>
    /// Traverse the unsigned digits of this BigInt, right to left.
    /// </summary>
    public IEnumerable<byte> DigitsRightToLeft //DigitsR2L
    {
      get
      {
        if (digits == null) yield return 0;
        else
        {
          for (var cur = digits.Last; cur != null; cur = cur.Previous) yield return cur.Value;
        }
      }
    }

    /// <summary>
    /// Iterate the proper divisors of this BigInt.  Zero yeilds no divisors.  Negatives yeild negative divisors.
    /// </summary>
    public IEnumerable<BigInt> ProperDivisors //naive implementation
    {
      get
      {
        if (this > Zero)
        {
          var half = DivideCeiling(this, Two);
          var cur = One;
          do
          {
            if (this % cur == Zero) yield return cur;
            cur++;
          } while (cur <= half);
        }
        else if (this < Zero)
        {
          var half = DivideFloor(this, Two);
          var cur = NegativeOne;
          do
          {
            if (this % cur == Zero) yield return cur;
            cur--;
          } while (cur >= half);
        }
        //else //is zero - NOT PRACTICAL
        //{
        //    BigInt cur = BigInt.Zero;
        //    while (true)
        //        yield return ++cur;
        //}
      }
    }

    /// <summary>
    /// Iterate the divisors of this BigInt.  Zero yeilds no divisors.  Negatives yeild negative divisors.
    /// </summary>
    public IEnumerable<BigInt> Divisors //naive implementation
    {
      get
      {
        foreach (var cur in ProperDivisors) yield return cur;

        if (digits != null) yield return this; //not zero
      }
    }
    #endregion Instance Properties

    #region Equals, GetHashCode, and ToString Overrides

    /// <summary>
    /// Determines whether this BigInt is equivalent to the given object
    /// </summary>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(obj, null)) return false;

      return (obj is BigInt) && Equals((BigInt)obj);
    }

    /// <summary>
    /// Returns the hashcode for this BigInt
    /// </summary>
    public override int GetHashCode()
    {
      if (digits == null)
        return 0;


      int count;
      var code = count = digits.Count;
      unchecked
      {
        foreach (byte b in digits)
        {
          code = code * (b == 0 ? count : b);
          if (code == 0)
            code = count;
        }
      }

      return (isneg ? -1 : 1) * code;
    }

    /// <summary>
    /// String representation of this BigInt
    /// </summary>
    public override string ToString()
    {
      if (digits == null) return "0";

      var sb = new StringBuilder();

      if (isneg) sb.Append("-");

      foreach (byte digit in digits) sb.Append(digit.ToString());

      return sb.ToString();
    }

    #endregion Equals, GetHashCode, and ToString Overrides

    #region Comparison and Equality

    /// <summary>
    /// Compare two BigInts
    /// </summary>
    public static int Compare(BigInt lhs, BigInt rhs)
    {
      return lhs.CompareTo(rhs);
    }

    /// <summary>
    /// Compare this BigInt to another
    /// </summary>
    public int CompareTo(BigInt rhs) //was less than
    {
      //permutations of this and rhs being zero
      if (digits == null && rhs.digits == null) return 0; //they are equal (both 0)
      if (digits == null && rhs.digits != null) return rhs.isneg ? 1 : -1;
      if (digits != null && rhs.digits == null) return isneg ? -1 : 1;

      //this is negative and rhs is positive
      if (isneg && !rhs.isneg) return -1;
      if (!isneg && rhs.isneg) return 1;
      if (isneg && rhs.isneg) //this and rhs are negative
      {
        if (digits.Count > rhs.digits.Count) return -1;
        if (digits.Count < rhs.digits.Count) return 1;
        return CompareFirstDiffDigit(rhs.digits, digits);
      }
      if (digits.Count > rhs.digits.Count) return 1;
      if (digits.Count < rhs.digits.Count) return -1;
      return CompareFirstDiffDigit(digits, rhs.digits); // count == count
    }

    public static bool operator <(BigInt lhs, BigInt rhs)
    {
      return lhs.CompareTo(rhs) == -1;
    }

    public static bool operator <=(BigInt lhs, BigInt rhs)
    {
      int result = lhs.CompareTo(rhs);
      return (result == -1 || result == 0);
    }

    public static bool operator >(BigInt lhs, BigInt rhs)
    {
      return lhs.CompareTo(rhs) == 1;
    }

    public static bool operator >=(BigInt lhs, BigInt rhs)
    {
      int result = lhs.CompareTo(rhs);
      return (result == 1 || result == 0);
    }

    public static bool operator ==(BigInt lhs, BigInt rhs)
    {
      return lhs.CompareTo(rhs) == 0;
    }

    public static bool operator !=(BigInt lhs, BigInt rhs)
    {
      return !(lhs == rhs);
    }

    /// <summary>
    /// arguments may not be null
    /// </summary>
    private static bool IsLessThan(LinkedList<byte> lhsDigits, LinkedList<byte> rhsDigits)
    {
      return (lhsDigits.Count == rhsDigits.Count && (CompareFirstDiffDigit(lhsDigits, rhsDigits) == -1)) || (lhsDigits.Count < rhsDigits.Count);
    }

    /// <summary>
    /// compares first most signifigant digits
    /// assumes lhs and rhs are both non-null, with equal counts greater than 0
    /// </summary>
    private static int CompareFirstDiffDigit(LinkedList<byte> lhs, LinkedList<byte> rhs)
    {
      var curLhs = lhs.First;
      var curRhs = rhs.First;
      for (int i = 0; i < lhs.Count; i++, curLhs = curLhs.Next, curRhs = curRhs.Next)
        if (curLhs.Value != curRhs.Value)
          return curLhs.Value < curRhs.Value ? -1 : 1;

      return 0;//all digits equal
    }

    /// <summary>
    /// Determines whether this BigInt is equivalent to the given BigInt
    /// </summary>
    public bool Equals(BigInt obj)
    {
      return this == obj;
    }

    #endregion Comparison and Equality

    #region Arithmetic Algorithms and Operators

    public static BigInt operator +(BigInt lhs, BigInt rhs)
    {
      return Add(lhs, rhs);
    }

    /// <summary>
    /// Add two BigInts
    /// </summary>
    public static BigInt Add(BigInt lhs, BigInt rhs)
    {
      if (lhs.digits == null && rhs.digits == null) return Zero; //0 + 0 = 0
      if (lhs.digits == null) return rhs; //0 + b = b
      if (rhs.digits == null) return lhs; //a + b = 0
      return AddPhase2(lhs.digits, lhs.isneg, rhs.digits, rhs.isneg);
    }

    /// <summary>
    /// operands are non-zero
    /// 
    /// There are 4 general cases:
    /// 1) c = a + b
    /// 2) c = -a + -b = -(a + b)
    /// 3) c = a + -b = a - b
    /// 4) c = -a + b = b - a
    /// special: c = a + b = b + a
    /// </summary>
    private static BigInt AddPhase2(LinkedList<byte> lhsDigits, bool lhsIsneg, LinkedList<byte> rhsDigits, bool rhsIsneg)
    {
      //use algerbra to make sure a and b are positive
      if (lhsIsneg && rhsIsneg) return new BigInt(AddPhase3(lhsDigits, rhsDigits), true); // (-a) + (-b) = -(a + b)
      if (!lhsIsneg && rhsIsneg) return SubtractPhase3(lhsDigits, rhsDigits); // a + (-b) = a - b
      if (lhsIsneg) return SubtractPhase3(rhsDigits, lhsDigits); // (-a) + b = b - a
      return new BigInt(AddPhase3(lhsDigits, rhsDigits), false);
    }

    /// <summary>
    /// operands are non-zero and positive
    /// </summary>
    private static LinkedList<byte> AddPhase3(LinkedList<byte> lhsDigits, LinkedList<byte> rhsDigits)
    {
      //make sure a is greater than b
      if (IsLessThan(lhsDigits, rhsDigits)) return AddPhase4(rhsDigits, lhsDigits); // a + b = b + a
      return AddPhase4(lhsDigits, rhsDigits);
    }

    /// <summary>
    /// operands are non-zero, positive, and lhs >= rhs
    /// </summary>
    private static LinkedList<byte> AddPhase4(LinkedList<byte> lhsDigits, LinkedList<byte> rhsDigits)
    {
      var sumDigits = new LinkedList<byte>();
      var curLhs = lhsDigits.Last;
      var curRhs = rhsDigits.Last;
      int carry = 0;
      int rhsTopindex = rhsDigits.Count - 1;
      for (int i = 0; ; )
      {
        byte valLhs = curLhs.Value;
        byte valRhs = i <= rhsTopindex ? curRhs.Value : (byte)0;
        int sum = (valLhs + valRhs + carry);
        carry = 0;
        if (sum > 9)
        {
          sum -= 10;
          carry = 1;
        }
        sumDigits.AddFirst((byte)sum);

        if (curLhs == lhsDigits.First)
          break;

        curLhs = curLhs.Previous;
        if (++i <= rhsTopindex)
          curRhs = curRhs.Previous;
      }
      if (carry == 1)
        sumDigits.AddFirst((byte)carry);

      return sumDigits;
    }

    /// <summary>
    /// operands are non-zero and both positive or both negative.  mutates lhs_digits.  because of extra checks required to handeld both a less than b
    /// and b less than a cases, slightly slower than AddPhase4, but when doing accumlative addition (such as Multiply), performance
    /// is much increased by sparing repeated large memory allocation for tempory states.
    /// </summary>
    private static void AddTo(LinkedList<byte> lhsDigits, LinkedList<byte> rhsDigits)
    {
      var curLhs = lhsDigits.Last;
      var curRhs = rhsDigits.Last;
      int carry = 0;
      int rhsTopindex = rhsDigits.Count - 1;
      int lhsTopindex = lhsDigits.Count - 1;
      int maxIndex = Math.Max(rhsTopindex, lhsTopindex);
      for (int i = 0; ; )
      {
        byte valLhs = i <= lhsTopindex ? curLhs.Value : (byte)0;
        byte valRhs = i <= rhsTopindex ? curRhs.Value : (byte)0;
        int sum = (valLhs + valRhs + carry);
        carry = 0;
        if (sum > 9)
        {
          sum -= 10;
          carry = 1;
        }

        if (i > lhsTopindex)
        {
          lhsDigits.AddFirst((byte)sum);
          if (i == maxIndex)
            break;

          curLhs = lhsDigits.First;
        }
        else
        {
          curLhs.Value = (byte)sum;
          if (i == maxIndex)
            break;

          curLhs = curLhs.Previous;
        }

        if (++i <= rhsTopindex)
          curRhs = curRhs.Previous;
      }

      if (carry == 1)
        lhsDigits.AddFirst((byte)carry);
    }

    public static BigInt operator -(BigInt lhs, BigInt rhs)
    {
      return Subtract(lhs, rhs);
    }

    /// <summary>
    /// Subtract the right-hand-side from the left-hand-side
    /// </summary>
    public static BigInt Subtract(BigInt lhs, BigInt rhs)
    {
      if (lhs.digits == null && rhs.digits == null) return Zero; //0 - 0 = 0,  (may consider value of implementing a - a = 0 [SEE Subtract3])
      if (lhs.digits == null) return new BigInt(rhs.digits, !rhs.isneg); //0 - (a) = -a, 0 - (-a) = a
      if (rhs.digits == null) return lhs; //a - 0 = a
      return SubtractPhase2(lhs.digits, lhs.isneg, rhs.digits, rhs.isneg);
    }

    /// <summary>
    /// a and b are non-zero
    /// 
    /// There are 4 general cases:
    /// 1) c = a - b
    /// 2) c = a - (-b) = a + b
    /// 3) c = (-a) - b = (-a) + (-b)
    /// 4) c = (-a) - (-b) = b - a
    /// special: c = a - b = -(b - a)
    /// </summary>
    private static BigInt SubtractPhase2(LinkedList<byte> lhsDigits, bool lhsIsneg, LinkedList<byte> rhsDigits, bool rhsIsneg)
    {
      //use algerbra to make sure a and b are positive
      if (!lhsIsneg && rhsIsneg) return new BigInt(AddPhase3(lhsDigits, rhsDigits), false); // a - (-b) = a + b
      if (lhsIsneg && !rhsIsneg) return new BigInt(AddPhase3(lhsDigits, rhsDigits), true); // (-a) - b = (-a) + (-b) = -(a + b) //also: return (-a + b); works
      if (lhsIsneg) return SubtractPhase3(rhsDigits, lhsDigits); // (-a) - (-b) = b - a
      return SubtractPhase3(lhsDigits, rhsDigits); //a and b are positive.
    }

    /// <summary>
    /// a and b are non-zero and positive (returns zero if equal)
    /// </summary>
    private static BigInt SubtractPhase3(LinkedList<byte> lhsDigits, LinkedList<byte> rhsDigits)
    {
      //we do a special check for a == b which means a - b = 0 for performance increases
      if (lhsDigits.Count == rhsDigits.Count) //a is either equal to or less than b
      {
        int diff = CompareFirstDiffDigit(lhsDigits, rhsDigits);
        if (diff == 0) return Zero; //they are equal
        if (diff == -1) goto neg_b_minus_a; //a < b
        goto pos_a_minus_b;
      }
      if (lhsDigits.Count < rhsDigits.Count) goto neg_b_minus_a; //a < b
      goto pos_a_minus_b;

      //the following are the only two uses of SubtractPhase4, so we are ok so far in placing equality condition here, in Phase3
    neg_b_minus_a: return new BigInt(SubtractPhase4(rhsDigits, lhsDigits), true);  //a - b = -(b - a), when a < b
    pos_a_minus_b: return new BigInt(SubtractPhase4(lhsDigits, rhsDigits), false);
    }

    /// <summary>
    /// a and b are non-zero, positive, a > b, a does not equl b (use phase 3)
    /// </summary>
    private static LinkedList<byte> SubtractPhase4(LinkedList<byte> lhsDigits, LinkedList<byte> rhsDigits)
    {
      //use a standard borrowing subtraction algorithm
      var diffDigits = new LinkedList<byte>();
      var curLhs = lhsDigits.Last;
      var curRhs = rhsDigits.Last;
      int borrow = 0;
      int rhsTopindex = rhsDigits.Count - 1;
      for (int i = 0; ; )
      {
        byte valLhs = curLhs.Value;
        byte valRhs = i <= rhsTopindex ? curRhs.Value : (byte)0;
        int diff = (valLhs - valRhs - borrow);
        borrow = 0;
        if (diff < 0)
        {
          diff += 10;
          borrow = 1;
        }
        diffDigits.AddFirst((byte)diff);

        if (curLhs == lhsDigits.First)
          break;

        curLhs = curLhs.Previous;
        if (++i <= rhsTopindex)
          curRhs = curRhs.Previous;
      }

      while (diffDigits.First.Value == 0) diffDigits.RemoveFirst();

      return diffDigits;
    }

    public static BigInt operator *(BigInt lhs, BigInt rhs)
    {
      return Multiply(lhs, rhs);
    }

    private static LinkedList<byte> GenMultPart(int leadingMult, int zeroCount)
    {
      var llist = new LinkedList<byte>();

      string leadingMultStr = leadingMult.ToString();
      for (int i = 0; i < leadingMultStr.Length; i++) llist.AddLast(byte.Parse(leadingMultStr[i].ToString()));

      for (int j = 0; j < zeroCount; j++) llist.AddLast(0);

      return llist;
    }

    /// <summary>
    /// Multiply two BigInts
    /// </summary>
    public static BigInt Multiply(BigInt lhs, BigInt rhs)
    {
      if (lhs.digits == null || rhs.digits == null) return Zero; //0 * b = 0 = a * 0

      LinkedList<byte> resultMult = null;

      int lzeros = lhs.digits.Count - 1;
      var ldigit = lhs.digits.First;
      for (; lzeros >= 0; lzeros--, ldigit = ldigit.Next) //standard multiplication algorithm
      {
        int rzeros = rhs.digits.Count - 1;
        var rdigit = rhs.digits.First;
        for (; rzeros >= 0; rzeros--, rdigit = rdigit.Next)
        {
          int leadingMult = (ldigit.Value * rdigit.Value);
          if (leadingMult != 0) //skip adding zero products
          {
            if (resultMult == null) //init result_mult... don't like doing this check after already initialized
              resultMult = GenMultPart(leadingMult, rzeros + lzeros);
            else //add to result mult
              AddTo(resultMult, GenMultPart(leadingMult, rzeros + lzeros)); //mutational addition for better memory management (less allocation of large linkedlists).
          }
        }
      }

      return new BigInt(resultMult, lhs.isneg ^ rhs.isneg);
    }

    /// <summary>
    /// Increment
    /// </summary>
    public static BigInt operator ++(BigInt bi)
    {
      return bi + One;
    }

    /// <summary>
    /// Decrement
    /// </summary>
    public static BigInt operator --(BigInt bi)
    {
      return bi - One;
    }

    ///// <summary>
    ///// version 1, very slow
    ///// Raise a BigInt to an int power
    ///// </summary>
    //public static BigInt Pow(BigInt lhs, int power)
    //{
    //    if (power < 0)
    //        throw new ArgumentOutOfRangeException("rhs must be postive");
    //    else if (power == 0)
    //        return BigInt.One;
    //    else if (power == 1)
    //        return lhs;

    //    BigInt result = lhs; //Multiply is non-mutational, so don't need deep copy.
    //    for (int i = 1; i < power; i++)
    //        result = BigInt.Multiply(result, lhs);

    //    return result;
    //}

    /// <summary>
    /// Raises a BigInt to an uint power
    /// </summary>
    /// <param name="x"></param>
    /// <param name="n"></param>
    /// <see cref="http://en.wikipedia.org/wiki/Exponentiation_by_squaring"/>
    /// <returns></returns>
    public static BigInt Pow(BigInt x, uint n)
    {
      var result = One;
      while (n != 0)
      {
        if ((n & 1) != 0) result = Multiply(result, x); /* n is odd, bitwise test */

        x = Multiply(x, x); //might consider mutational mult. (would have to remember to reassign x to a deep copy of itself first)
        n /= 2;     /* integer division, rounds down */
      }
      return result;
    }


    /// <summary>
    /// Perform division with remainder
    /// </summary>
    /// <param name="a">dividend</param>
    /// <param name="d">divisor</param>
    /// <param name="r">remainder</param>
    /// <returns>quotient</returns>
    public static BigInt Divide(BigInt a, BigInt d, out BigInt r)
    {
      if (d.digits == null) // a / 0 DNE
        throw new DivideByZeroException();

      if (a.digits == null) // 0 / d = 0, remainder 0
      {
        r = Zero;
        return Zero;
      }

      //now we know a.digits and d.digits are both non-null
      if (IsLessThan(a.digits, d.digits)) // a < d -> a / d = 0, remainder = a
      {
        r = a;
        return Zero;
      }

      var q = new LinkedList<byte>(); //we know q isn't zero
      LinkedList<byte> rDigits = null;
      for (var curbyte = a.digits.First; curbyte != null; curbyte = curbyte.Next)
      {
        if (rDigits != null || curbyte.Value != 0) //skip leading zeros
        {
          if (rDigits == null)
            rDigits = new LinkedList<byte>();

          rDigits.AddLast(curbyte.Value);
        }

        //d.digits is never null, so if r_digits is null, it is "less" than d.digits
        if (rDigits == null || IsLessThan(rDigits, d.digits))
        {
          if (q.Count != 0) //skip leading zeros
            q.AddLast(0);
        }
        else
        {
          byte qDigit = BruteDivide(ref rDigits, d.digits); //r_digits is both input and output parameter
          if (q.Count != 0 || qDigit != 0) //skip leading zeros
            q.AddLast(qDigit);
        }
      }

      r = new BigInt(rDigits, a.isneg);//even though mathematicians would always have r positive, we are being consistant with int type.
      return new BigInt(q, a.isneg ^ d.isneg);
    }

    /// <summary>
    /// brut divisions where
    /// a_digits != null
    /// d_digits != null
    /// a and d are positive
    /// a is not less than d
    /// and quotient is a less than 10
    /// </summary>
    private static byte BruteDivide(ref LinkedList<byte> aDigits, LinkedList<byte> dDigits)
    {
      BigInt r = new BigInt(aDigits, false);
      int q = 0;
      for (; r.digits != null; q++)
      {
        r = SubtractPhase3(r.digits, dDigits);
        if (r.isneg) break;
        if (r.digits == null)
        {
          q++;
          break;
        }
      }

      if (r.isneg)
        r = SubtractPhase3(dDigits, r.digits); //d + r = d - abs(r), when r<0

      aDigits = r.digits;
      return (byte)q;
    }

    /// <summary>
    /// Division operator
    /// </summary>
    public static BigInt operator /(BigInt lhs, BigInt rhs)
    {
      return Divide(lhs, rhs);
    }

    /// <summary>
    /// Perform division without remainder
    /// </summary>
    public static BigInt Divide(BigInt lhs, BigInt rhs)
    {
      BigInt r;
      return Divide(lhs, rhs, out r);
    }

    /// <summary>
    /// The ceiling after division
    /// </summary>
    public static BigInt DivideCeiling(BigInt lhs, BigInt rhs)
    {
      BigInt r;
      BigInt result = Divide(lhs, rhs, out r);

      if (r == Zero) //no remainder
        return result;
      if (lhs.isneg ^ rhs.isneg) //result (with remainder) is negative
        return result;
      return ++result;
    }

    /// <summary>
    /// The floor after division
    /// </summary>
    public static BigInt DivideFloor(BigInt lhs, BigInt rhs)
    {
      BigInt r;
      BigInt result = Divide(lhs, rhs, out r);

      if (r == Zero) //no remainder
        return result;
      if (lhs.isneg ^ rhs.isneg) //result (with remainder) is negative
        return --result;
      return result;
    }

    /// <summary>
    /// mod: remainder after division, operator
    /// </summary>
    public static BigInt operator %(BigInt lhs, BigInt rhs)
    {
      return Mod(lhs, rhs);
    }

    /// <summary>
    /// The remainder after division
    /// </summary>
    public static BigInt Mod(BigInt lhs, BigInt rhs)
    {
      BigInt r;
      Divide(lhs, rhs, out r);
      return r;
    }

    /// <summary>
    /// negation operator
    /// </summary>
    public static BigInt operator -(BigInt bi)
    {
      return Negate(bi);
    }

    /// <summary>
    /// The negation of a BigInt
    /// </summary>
    public static BigInt Negate(BigInt bi)
    {
      return new BigInt(bi.digits, !bi.isneg);
    }

    /// <summary>
    /// The absolute value of a BigInt
    /// </summary>
    public static BigInt Abs(BigInt bi)
    {
      return new BigInt(bi.digits, false);
    }

    #endregion Arithmetic Algorithms and Operators

    #region  Implicit Operators (from integral types to BigInt)

    //note: implicit operators also serve as explicit operators

    public static implicit operator BigInt(byte value)
    {
      return Parse(value.ToString());
    }

    public static implicit operator BigInt(sbyte value)
    {
      return Parse(value.ToString());
    }

    public static implicit operator BigInt(ushort value)
    {
      return Parse(value.ToString());
    }

    public static implicit operator BigInt(short value)
    {
      return Parse(value.ToString());
    }

    public static implicit operator BigInt(uint value)
    {
      return Parse(value.ToString());
    }

    public static implicit operator BigInt(int value)
    {
      return Parse(value.ToString());
    }

    public static implicit operator BigInt(ulong value)
    {
      return Parse(value.ToString());
    }

    public static implicit operator BigInt(long value)
    {
      return Parse(value.ToString());
    }

    //may consider implicit conversion from bool or datetime to since no precision / information loss
    //on the other hand, these are not integral types, so not purely logical

    #endregion Implicit Operators (from integral types to BigInt)

    #region Explicit Operators (from boolean, DateTime, string, and rational types to BigInt)

    public static explicit operator BigInt(decimal value)
    {
      return Parse(Math.Truncate(value).ToString(CultureInfo.InvariantCulture));
    }

    public static explicit operator BigInt(double value)
    {
      return Parse(Math.Truncate(value).ToString(CultureInfo.InvariantCulture));
    }

    public static explicit operator BigInt(float value)
    {
      return Parse(Math.Truncate(value).ToString(CultureInfo.InvariantCulture));
    }

    public static explicit operator BigInt(bool value)
    {
      return value ? One : Zero;
    }

    public static explicit operator BigInt(DateTime value)
    {
      return value.Ticks;
    }

    //while a little unnatural and can always use BigInt.Parse(string),
    //will serve well for purposes of Enumerable<string>.Cast<BigInt>()
    public static explicit operator BigInt(string value)
    {
      return Parse(value);
    }

    #endregion Explicit Operators (from boolean, DateTime, and rational types to BigInt)

    #region Explicit Operators (from BigInt to other types)

    public static explicit operator byte(BigInt value)
    {
      return byte.Parse(value.ToString());
    }

    public static explicit operator sbyte(BigInt value)
    {
      return sbyte.Parse(value.ToString());
    }

    public static explicit operator ushort(BigInt value)
    {
      return ushort.Parse(value.ToString());
    }

    public static explicit operator short(BigInt value)
    {
      return short.Parse(value.ToString());
    }

    public static explicit operator uint(BigInt value)
    {
      return uint.Parse(value.ToString());
    }

    public static explicit operator int(BigInt value)
    {
      return int.Parse(value.ToString());
    }

    public static explicit operator ulong(BigInt value)
    {
      return ulong.Parse(value.ToString());
    }

    public static explicit operator long(BigInt value)
    {
      return long.Parse(value.ToString());
    }

    //though will never throw, unnatural for implicit conversion
    public static explicit operator bool(BigInt value)
    {
      return value.digits != null;
    }

    //though will never throw, unnatural for implicit conversion
    public static explicit operator DateTime(BigInt value)
    {
      return new DateTime(long.Parse(value.ToString()));
    }

    public static explicit operator char(BigInt value)
    {
      return (char)ushort.Parse(value.ToString());
    }

    #endregion Explicit Operators (from BigInt to other types)

    #region IConvertable

    //we risk redundant implementations for the sake of performance

    /// <summary>
    /// Returns the System.TypeCode for the Swensen.Object
    /// </summary>
    /// <returns></returns>
    public TypeCode GetTypeCode()
    {
      return TypeCode.Object;
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      return digits != null;
    }

    byte IConvertible.ToByte(IFormatProvider provider)
    {
      return byte.Parse(ToString());
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
      return (char)ushort.Parse(ToString());
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      return new DateTime(long.Parse(ToString()));
    }

    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      return decimal.Parse(ToString());
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
      return double.Parse(ToString());
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
      return short.Parse(ToString());
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
      return int.Parse(ToString());
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
      return long.Parse(ToString());
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      return sbyte.Parse(ToString());
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
      return float.Parse(ToString());
    }

    string IConvertible.ToString(IFormatProvider provider)
    {
      return ToString();
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
      return Convert.ChangeType(this, conversionType, provider); //huh?
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      return ushort.Parse(ToString());
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      return uint.Parse(ToString());
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      return ulong.Parse(ToString());
    }

    #endregion IConvertable

    #region Parse Methods

    /// <summary>
    /// Converts the string representation of an integer to its BigInt equivalent
    /// </summary>
    public static BigInt Parse(string numstr)
    {
      if (numstr == null)
        throw new ArgumentNullException("numstr");

      BigInt result;
      if (!TryParse(numstr, out result))
        throw new FormatException("Input string was not in the correct format");

      return result;
    }

    /// <summary>
    /// Converts the string representation of an integer to its BigInt equivalent.  
    /// A return value indicates whether the conversion succeeded
    /// </summary>
    public static bool TryParse(string numstr, out BigInt result)
    {
      LinkedList<byte> digits;
      bool isneg;

      bool success = TryParse(numstr, out digits, out isneg);
      result = new BigInt(digits, isneg); //digits is null if success is false; thus result == BigInt.zero

      return success;
    }

    /// <summary>
    /// Convert numstr into digits, and isneg BigInt components; return success; digits is null if success is false
    /// </summary>
    private static bool TryParse(string numstr, out LinkedList<byte> digits, out bool isneg)
    {
      digits = null;
      isneg = false;

      if (numstr == null)
        return false;

      numstr = numstr.Trim();
      if (numstr == string.Empty)
        return false;

      if (numstr[0] == '-')
      {
        if (numstr.Length == 1) //"-"
          return false;

        isneg = true;
      }

      int i = isneg ? 1 : 0;
      //skip leading zeros, including sole zero
      for (; i < numstr.Length; i++)
        if (numstr[i] != '0')
          break;

      if (i == numstr.Length) //"-00000" == "0"
        return true;

      var digitsTry = new LinkedList<byte>();
      for (; i < numstr.Length; i++)
      {
        byte digit;
        if (!byte.TryParse(numstr[i].ToString(), out digit))
          return false; //digits is still null

        digitsTry.AddLast(digit);
      }

      digits = digitsTry;
      return true;
    }
    #endregion Parse Methods

    #region Eval
    //note that ops of longer length are listed first where using compound symbols (compared to BinaryOpTokens too)
    static readonly string[] UnaryOpTokens =
    {
      "gethashcode", "properdivisors", "divisors",
      "negate", "abs", "++", "--",
      "sum", "min", "max", "range",
      "lcm", "gcd", "sqrt", "pow"
    };

    //note that ops of longer length are listed first where using compound symbols
    static readonly string[] BinaryOpTokens =
    {
      "==", "!=", ">=", "<=", ">=", "<=", ">", "<",
      "/floor", "/ceiling", "/%", "/", "%", // "/%" is division with remainder
      "+", "*", "^", "-" //make sure "-" is last
    };

    static readonly string[] AllOpTokens = UnaryOpTokens.Union(BinaryOpTokens).ToArray();

    /// <summary>
    /// Evaluates simple BigInt expressions.
    /// </summary>
    public static object Eval(string expression)
    {
      if (expression == null)
        throw new NullReferenceException("expression cannot be null");

      string[] tokens = null;

      expression = expression.Replace(" ", string.Empty);

      string op = null;
      for (int i = 0; i < AllOpTokens.Length; i++)
      {
        op = AllOpTokens[i];
        if (expression.Contains(op))
        {
          tokens = expression.Split(new[] { op }, StringSplitOptions.RemoveEmptyEntries);
          break;
        }
      }

      if (tokens == null)
        throw new FormatException("no op tokens in expression");
      if (tokens.Length == 2)
      {
        BigInt a = Parse(tokens[0]);
        BigInt b = Parse(tokens[1]);

        switch (op)
        {
          case "+":
            return (a + b);
          case "-":
            return (a - b);
          case "*":
            return (a * b);
          case "^":
            return (Pow(a, Convert.ToUInt32(tokens[1])));
          case "/%":
            BigInt r;
            BigInt q = Divide(a, b, out r);
            return string.Format("q = {0}, r = {1}", q, r);
          case "/floor":
            return DivideFloor(a, b);
          case "/ceiling":
            return DivideCeiling(a, b);
          case "/":
            return (a / b);
          case "%":
            return (a % b);
          case "==":
            return (a == b);
          case ">":
            return (a > b);
          case ">=":
            return (a >= b);
          case "<":
            return (a < b);
          case "<=":
            return (a <= b);
          default:
            throw new FormatException("Invalid binary operator");
        }
      }
      if (tokens.Length == 1)
      {
        var strArgs = tokens[0].Split(',');
        var args = Parse(strArgs).ToArray();
        BigInt a = args[0];
        switch (op)
        {
          case "gethashcode":
            return a.GetHashCode();
          case "abs":
            return Abs(a);
          case "negate":
            return Negate(a);
          case "properdivisors":
            return ToString(a.ProperDivisors);
          case "divisors":
            return ToString(a.Divisors);
          case "sum":
            return Sum(args);
          case "range":
            return ToString(Range(args[0], args[1]));
          case "min":
            return Min(args);
          case "max":
            return Max(args);
          case "lcm":
            return Lcm(args[0], args[1]);
          case "gcd":
            return Gcd(args[0], args[1]);
          case "pow":
            return Pow(a, Convert.ToUInt32(strArgs[1]));
          case "sqrt":
            return Sqrt(a);
          case "++":
            return (++a);
          case "--":
            return (--a);
          default:
            throw new FormatException("Invalid unary operator");
        }
      }

      throw new FormatException("invalid expression");
    }

    private static IEnumerable<BigInt> Parse(IEnumerable<string> input)
    {
      foreach (string i in input)
        yield return Parse(i);
    }

    private static string ToString<T>(IEnumerable<T> objs)
    {
      if (objs == null)
        return string.Empty;

      return string.Join(", ", objs.Select(x => x.ToString()).ToArray());
    }

    #endregion Eval

    #region Common Algorithms

    /// <summary>
    /// Yields the inclusive range of values from start to end
    /// </summary>
    public static IEnumerable<BigInt> Range(BigInt start, BigInt end)
    {
      //if (upperBound < lowerBound)
      //throw new ArgumentOutOfRangeException("upperBound cannot be less than lowerBound");

      if (end >= start)
      {
        do
        {
          yield return start;
          start++;
        } while (start <= end);
      }
      else
      {
        do
        {
          yield return start;
          start--;
        } while (start >= end);
      }
    }

    /// <summary>
    /// Calculates the maximum of two BigInts
    /// </summary>
    public static BigInt Max(BigInt lhs, BigInt rhs)
    {
      return lhs >= rhs ? lhs : rhs;
    }

    /// <summary>
    /// Calculates the maximum of a set of BigInts
    /// </summary>
    public static BigInt Max(params BigInt[] seq)
    {
      return seq.Max();
    }

    /// <summary>
    /// Calculates the maximum of a set of BigInts
    /// </summary>
    public static BigInt Max(IEnumerable<BigInt> seq)
    {
      return seq.Max();
    }

    /// <summary>
    /// Calculates the minimum of two BigInts
    /// </summary>
    public static BigInt Min(BigInt lhs, BigInt rhs)
    {
      return lhs <= rhs ? lhs : rhs;
    }

    /// <summary>
    /// Calculates the minimum of a set of BigInts
    /// </summary>
    public static BigInt Min(params BigInt[] seq)
    {
      return seq.Min();
    }

    /// <summary>
    /// Calculates the minimum of a set of BigInts
    /// </summary>
    public static BigInt Min(IEnumerable<BigInt> seq)
    {
      return seq.Min();
    }

    /// <summary>
    /// Calculates the sumation of a set of BigInts
    /// </summary>
    public static BigInt Sum(params BigInt[] seq)
    {
      return Sum((IEnumerable<BigInt>)seq);
    }

    /// <summary>
    /// Calculates the sumation of a set of BigInts
    /// </summary>
    public static BigInt Sum(IEnumerable<BigInt> seq)
    {
      bool seeded = false;
      BigInt sum = Zero;
      foreach (BigInt bi in seq)
      {
        if (bi != Zero) //don't bother
        {
          if (!seeded) //seed sum
          {
            sum = new BigInt(new LinkedList<byte>(bi.digits), bi.isneg); //deep copy for AddTo
            seeded = true;
          }
          else if (sum.isneg ^ bi.isneg) //can use AddTo if and only if sum and bi are both positive or both negative
            sum += bi;
          else //when we can
            AddTo(sum.digits, bi.digits);
        }
      }

      return sum;
    }

    /// <summary>
    /// Finds the greatest common divisor of two BigInts
    /// </summary>
    //public static BigInt Gcd(BigInt lhs, BigInt rhs)
    //{
    //    if (lhs == BigInt.Zero)
    //        throw new ArgumentOutOfRangeException("lhs", "Gcd is not defined for BigInt.Zero");
    //    else if (lhs == BigInt.Zero) //bug
    //        throw new ArgumentOutOfRangeException("rhs", "Gcd is not defined for BigInt.Zero");

    //    lhs = BigInt.Abs(lhs);
    //    rhs = BigInt.Abs(rhs);
    //    return Max(Enumerable.Intersect(lhs.Divisors, rhs.Divisors)); //not fast, should use euclidean method
    //}

    /// <summary>
    /// Finds the greatest common divisor of two BigInts
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <see cref="http://en.wikipedia.org/wiki/Euclidean_algorithm"/>
    /// <returns></returns>
    public static BigInt Gcd(BigInt lhs, BigInt rhs)
    {
      if (lhs.digits == null)
        throw new ArgumentOutOfRangeException("lhs", "Gcd is not defined for BigInt.Zero");
      if (rhs.digits == null)
        throw new ArgumentOutOfRangeException("rhs", "Gcd is not defined for BigInt.Zero");

      while (rhs.digits != null)
      {
        if (lhs > rhs) //not sure if we can trust digits not to be null
          lhs = Subtract(lhs, rhs);
        else
          rhs = Subtract(rhs, lhs);
      }

      return lhs;
    }

    //public static BigInt GCD(params BigInt[] seq)
    //{
    //    return GCD((IEnumerable<BigInt>)seq);
    //}

    //public static BigInt GCD(IEnumerable<BigInt> seq)
    //{

    //}

    /// <summary>
    /// Finds the least common multiple of two BigInts
    /// </summary>
    public static BigInt Lcm(BigInt lhs, BigInt rhs)
    {
      if (lhs.digits == null || rhs.digits == null)
        return Zero;

      return (lhs * rhs) / Gcd(lhs, rhs);
    }

    /// <summary>
    /// Calcultes the truncated square root of a BigInt
    /// </summary>
    /// <param name="value">the operand</param>
    /// <see cref="http://www.codecodex.com/wiki/index.php?title=Calculate_an_integer_square_root"/>
    /// <returns>Square root of value</returns>
    public static BigInt Sqrt(BigInt value)
    {
      if (value.digits == null) return Zero;  // Avoid zero divide

      var n = DivideCeiling(value, Two);// Initial estimate, never low
      var n1 = (n + (value / n)) / Two;
      while (n1 < n)
      {
        n = n1;
        n1 = (n + (value / n)) / Two;
      } // end while

      return n;
    } // end Isqrt()

    #endregion Common Algorithms
  }
}
