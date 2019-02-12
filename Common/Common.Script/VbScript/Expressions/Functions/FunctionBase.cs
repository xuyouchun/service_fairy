using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    abstract class FunctionBase : IFunction
    {
        /// <summary>
        /// 验证参数的正确性
        /// </summary>
        /// <param name="values"></param>
        private void _ValidateParameters(Value[] values)
        {
            _ValidateParameters(values, true);
        }

        /// <summary>
        /// 验证参数的正确性
        /// </summary>
        /// <param name="values">参数</param>
        /// <param name="force">是否严格地验证参数个数</param>
        private void _ValidateParameters(Value[] values, bool force)
        {
            FunctionAttribute attr = _GetAttr();

            if (force && attr.ParameterTypes.Length != values.Length)
                throw new ScriptRuntimeException(string.Format("函数“{0}”参数个数错误", attr.FunctionName));

            int count = Math.Min(values.Length, attr.ParameterTypes.Length);
            for (int k = 0; k < count; k++)
            {
                if (!ValueTypes.IsCorrectType(attr.ParameterTypes[k], values[k]))
                    throw new ScriptRuntimeException(string.Format("函数“{0}”第{1}个参数的类型错误", attr.FunctionName, k + 1));
            }
        }

        /// <summary>
        /// 验证参数个数
        /// </summary>
        /// <param name="values"></param>
        /// <param name="count"></param>
        protected void VariableExpressionCount(Value[] values, int count)
        {
            if (count != values.Length)
                throw new ScriptRuntimeException(string.Format("函数“{0}”参数个数错误", _GetAttr().FunctionName));
        }

        private FunctionAttribute _Attr;

        private FunctionAttribute _GetAttr()
        {
            if (_Attr == null)
            {
                object[] attrs = this.GetType().GetCustomAttributes(typeof(FunctionAttribute), false);
                if (attrs.Length == 0)
                    throw new ScriptException("无法验证参数信息");

                _Attr = (FunctionAttribute)attrs[0];
            }

            return _Attr;
        }

        protected abstract Value OnExecute(IExpressionContext context, Value[] values);

        public virtual Value Execute(IExpressionContext context, Value[] values)
        {
            _ValidateParameters(values, _GetAttr().ForceValidate);
            return OnExecute(context, values);
        }

        /// <summary>
        /// 将指定的数值转换为整型
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static int[] ConvertToIntegers(Value[] values)
        {
            int[] nums = new int[values.Length];
            for (int k = 0; k < nums.Length; k++)
            {
                nums[k] = ConvertToInteger(values[k]);
            }

            return nums;
        }

        protected static int ConvertToInteger(Value value)
        {
            if (value.IsFloat())
            {
                double dbl = (double)value;
                if ((int)dbl == dbl)
                    return (int)dbl;
            }

            try
            {
                return (int)value;
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException(string.Format("数值{0}不可转换为整型", value));
            }
        }
    }
}
