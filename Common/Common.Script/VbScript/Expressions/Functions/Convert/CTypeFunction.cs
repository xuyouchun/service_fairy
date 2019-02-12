using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CType", typeof(object))]
    [FunctionInfo("获取指定数值的类型", "value")]
    class CTypeFunction : FunctionBase
    {
        static CTypeFunction()
        {
            _Types.Add(typeof(int), VbConstValues.vbInteger);
            _Types.Add(typeof(long), VbConstValues.vbLong);
            _Types.Add(typeof(float), VbConstValues.vbSingle);
            _Types.Add(typeof(double), VbConstValues.vbDouble);
            //_Types.Add(typeof(decimal), VbConstValues.vbCurrency);
            _Types.Add(typeof(DateTime), VbConstValues.vbDate);
            _Types.Add(typeof(string), VbConstValues.vbString);
            _Types.Add(typeof(char), VbConstValues.vbString);
            _Types.Add(typeof(object), VbConstValues.vbObject);
            _Types.Add(typeof(bool), VbConstValues.vbBoolean);
            _Types.Add(typeof(Decimal), VbConstValues.vbDecimal);
            _Types.Add(typeof(byte), VbConstValues.vbByte);
            _Types.Add(typeof(ArrayObject), VbConstValues.vbArray);
        }

        private static readonly Dictionary<Type, int> _Types = new Dictionary<Type, int>();

        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            object obj = values[0].InnerValue;
            if (obj == null)
                return VbConstValues.vbNull;

            int t;
            if (_Types.TryGetValue(obj.GetType(), out t))
                return t;

            return VbConstValues.vbObject;
        }
    }
}
