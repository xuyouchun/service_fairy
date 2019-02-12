using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 局部变量值缓存的管理器
    /// </summary>
    class LastValueCacheManager
    {
        private readonly Stack<LocalValueCache> _Stack = new Stack<LocalValueCache>();

        /// <summary>
        /// 添加一层缓存
        /// </summary>
        public void BeginLayer()
        {
            _Stack.Push(new LocalValueCache());
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndLayer()
        {
            _Stack.Pop();
        }

        /// <summary>
        /// 添加一个值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddValue(string name, Value value)
        {
            _Stack.Peek().AddValue(name, value);
        }

        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Value GetValue(string name)
        {
            Value v = _Stack.Peek().GetValue(name);
            if (v != null)
                return v;

            int index = 0;

            foreach (LocalValueCache cache in _Stack)
            {
                if (index++ == 0)
                    continue;

                v = cache.GetValue(name);
                if (v != null)
                    return v;
            }

            return null;
        }
    }
}
