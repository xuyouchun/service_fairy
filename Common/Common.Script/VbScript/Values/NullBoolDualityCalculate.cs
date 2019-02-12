using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(Nullable), typeof(bool))]
    [DualityCalculate(typeof(bool), typeof(Nullable))]
    class NullBoolDualityCalculate : DefaultDualityCalculate
    {
        public override Value GreaterThan(Value value1, Value value2)
        {
            return false;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return false;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return false;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return false;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return false;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return false;
        }

        //public override Value Addition(Value value1, Value value2)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Value Subtraction(Value value1, Value value2)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Value Multiplication(Value value1, Value value2)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Value Division(Value value1, Value value2)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Value Modulus(Value value1, Value value2)
        //{
        //    throw new NotImplementedException();
        //}

        public override Value And(Value value1, Value value2)
        {
            return false;
        }

        public override Value Or(Value value1, Value value2)
        {
            return value1.IsEmpty() ? value2 : value1;
        }

        //public override Value Pow(Value value1, Value value2)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Value Xor(Value value1, Value value2)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
