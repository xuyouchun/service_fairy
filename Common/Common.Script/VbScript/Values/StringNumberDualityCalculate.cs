using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(string), typeof(byte))]
    [DualityCalculate(typeof(string), typeof(int))]
    [DualityCalculate(typeof(string), typeof(long))]
    [DualityCalculate(typeof(string), typeof(float))]
    [DualityCalculate(typeof(string), typeof(double))]
    [DualityCalculate(typeof(string), typeof(decimal))]
    class StringNumberDualityCalculate : DefaultDualityCalculate
    {
        public override Value Addition(Value value1, Value value2)
        {
            return _Addition((string)value1, value2);
        }

        private Value _Addition(string s, Value v2)
        {
            Value result;
            if (TryParseToValue(s, v2, out result))
                return result;

            return s + v2.ToString();
        }

        internal static bool TryParseToValue(string s, Value v2, out Value result)
        {
            result = null;

            if (s.IndexOf('.') >= 0)
            {
                double v;
                if (!double.TryParse(s, out v))
                    return false;

                v += (double)v2;
                TypeCode tCode = Type.GetTypeCode(v2.GetValueType());
                if (tCode <= TypeCode.Single && v <= float.MaxValue && v >= float.MinValue)
                    result = (float)v;
                else
                    result = v;
            }
            else
            {
                long v;
                if (!long.TryParse(s, out v))
                    return false;

                v += (long)v2;
                TypeCode tCode = Type.GetTypeCode(v2.GetValueType());
                if (tCode == TypeCode.Byte && v <= byte.MaxValue && v >= byte.MinValue)
                    result = (byte)v;
                else if (tCode == TypeCode.Int16 && v <= short.MaxValue && v >= short.MinValue)
                    result = (short)v;
                else if (tCode == TypeCode.Int32 && v <= int.MaxValue && v >= int.MinValue)
                    result = (int)v;
                else if (tCode == TypeCode.Int64 && v <= int.MaxValue && v >= int.MinValue)
                    result = (long)v;
                else
                {
                    if (v <= byte.MaxValue && v >= byte.MinValue)
                        result = (byte)v;
                    else if (v <= short.MaxValue && v >= short.MinValue)
                        result = (short)v;
                    else if (v <= int.MaxValue && v >= int.MinValue)
                        result = (int)v;
                    else
                        result = v;
                }
            }

            return true;
        }

        private static IComparable _ConvertToNumber(Type t, Value v)
        {
            return Convert.ChangeType(v.InnerValue, t) as IComparable;
        }

        private static int _Compare(Value value1, Value value2)
        {
            try
            {
                return _ConvertToNumber(value2.GetValueType(), value1).CompareTo(value2.InnerValue);
            }
            catch
            {
                return value1.ToString().CompareTo(value2.ToString());
            }
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return _Compare(value1, value2) > 0;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return _Compare(value1, value2) >= 0;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return _Compare(value1, value2) <= 0;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return _Compare(value1, value2) < 0;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return _Compare(value1, value2) == 0;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return _Compare(value1, value2) != 0;
        }
    }
}
