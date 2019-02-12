using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Configuration;

namespace Common.Package
{
    /// <summary>
    /// 配置信息读取器，配置信息以“a=b;c=d的方式存储”
    /// </summary>
    public class KeyValueSettingsReader : IConfiguration, IEnumerable<string>
    {
        public KeyValueSettingsReader(string s, char separator = ';')
        {
            foreach (string part in (s ?? "").Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries))
            {
                int index = part.IndexOf('=');
                if (index >= 0)
                {
                    _dict[part.Substring(0, index).Trim()] = part.Substring(index + 1).Trim();
                }
            }
        }

        private readonly Dictionary<string, string> _dict = new Dictionary<string, string>();

        /// <summary>
        /// 根据名称获取配置信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Get(string name)
        {
            if (name == null)
                return null;

            string result;
            _dict.TryGetValue(name, out result);
            return result;
        }

        /// <summary>
        /// 获取全部的键名
        /// </summary>
        /// <returns></returns>
        public string[] GetAllKeys()
        {
            return _dict.Keys.ToArray();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
