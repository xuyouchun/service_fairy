using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 局部变量的缓存，用于在运行过程中读取上次获得的局部变量，防止两次读取不一致的情况
    /// </summary>
    class LocalValueCache
    {
        private readonly Dictionary<string, Value> _Dict = new Dictionary<string, Value>();

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddValue(string name, Value value)
        {
            _Dict[name.ToUpper()] = value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Value GetValue(string name)
        {
            Value v;
            _Dict.TryGetValue(name.ToUpper(), out v);
            return v;
        }
    }
}
