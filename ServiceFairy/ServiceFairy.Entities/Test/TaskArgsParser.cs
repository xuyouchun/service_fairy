using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Utility;

namespace ServiceFairy.Entities.Test
{
    /// <summary>
    /// 用于解析任务的参数
    /// </summary>
    public class TaskArgsParser
    {
        public TaskArgsParser(string args)
        {
            _args = args ?? string.Empty;

            MatchCollection matchs = Regex.Matches(_args, @"\b""([^""]+)""\b|\b([^\s""]+)\b", RegexOptions.Singleline);
            foreach (Match m in matchs)
            {
                for (int k = 1; k < m.Groups.Count; k++)
                {
                    var g = m.Groups[k];
                    int index;
                    if (g.Success && (index = g.Value.IndexOf(':')) > 0)
                    {
                        _dict[g.Value.Substring(0, index)] = g.Value.Substring(index + 1);
                    }
                }
            }
        }

        private readonly string _args;
        private readonly Dictionary<string, string> _dict = new Dictionary<string, string>();

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return _dict.GetOrDefault(key);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue)
        {
            return Get(key).ToType<T>(defaultValue);
        }

        /// <summary>
        /// 线程数
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetThreadCount(int defaultValue = 1)
        {
            return Get<int>("/task_count", defaultValue);
        }

        //public int Get

        public override string ToString()
        {
            return _args;
        }
    }
}
