using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 数组
    /// </summary>
    class ArrayObject : Value, IEnumerable
    {
        public ArrayObject(int[] upBounds)
            : base(_CreateArray(upBounds))
        {

        }

        public ArrayObject(int upBound)
            : this(new[] { upBound })
        {

        }

        private static object _CreateArray(int[] upBounds)
        {
            if (upBounds.Length == 0)
                return new Value[0];

            try
            {
                for (int k = 0; k < upBounds.Length; k++)
                {
                    upBounds[k]++;
                }

                return Array.CreateInstance(typeof(Value), upBounds);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ScriptRuntimeException("创建数组时要求长度必须为非负数");
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public Value GetValue(int[] indexes)
        {
            Array arr = (Array)InnerValue;

            if (arr.Rank != indexes.Length)
                throw new ScriptRuntimeException("访问数组时指定的维数与定义时不同");

            try
            {
                object r = arr.GetValue(indexes);
                return r as Value ?? Value.Void;
            }
            catch (IndexOutOfRangeException)
            {
                throw new ScriptRuntimeException("访问数组时下标越界");
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="indexes"></param>
        /// <param name="value"></param>
        public void SetValue(int[] indexes, Value value)
        {
            Array arr = (Array)InnerValue;

            if (arr.Rank != indexes.Length)
                throw new ScriptRuntimeException("访问数组时指定的维数与定义时不同");

            try
            {
                arr.SetValue(value, indexes);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ScriptRuntimeException("访问数组时下标越界");
            }
        }

        public override string ToString()
        {
            return "Array";
        }

        public Value GetRankLength(int bound)
        {
            return ((Array)InnerValue).GetLength(bound - 1);
        }

        public int GetRank()
        {
            return ((Array)InnerValue).Rank;
        }

        public static ArrayObject Create(object[] values)
        {
            if (values.Length == 0)
                return new ArrayObject(0);

            ArrayObject arr = new ArrayObject(values.Length - 1);

            for (int k = 0; k < values.Length; k++)
            {
                arr.SetValue(new[] { k }, new Value(values[k]));
            }

            return arr;
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            if (GetRank() == 0)
                yield break;

            int length = GetRankLength(0);
            for (int k = 0; k < length; k++)
            {
                yield return GetValue(new int[] { k });
            }
        }

        #endregion
    }
}
