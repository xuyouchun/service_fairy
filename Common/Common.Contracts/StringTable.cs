using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 字符串表
    /// </summary>
    public class StringTable
    {
        public StringTable()
        {

        }

        public StringTable(IEnumerable<string> strs)
        {
            if (strs == null)
                return;

            lock (_thisLocker)
            {
                foreach (string str in strs)
                {
                    _AddString(str);
                }
            }
        }

        public const int NullString = -1;
        public const int EmptyString = -2;

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int AddString(string str)
        {
            lock (_thisLocker)
            {
                return _AddString(str);
            }
        }

        private int _AddString(string str)
        {
            if (str == null)
                return NullString;

            if (str == "")
                return EmptyString;

            int id;
            if (!_dictOfStr.TryGetValue(str, out id))
            {
                _list.Add(str);
                _dictOfStr.Add(str, id = _list.Count - 1);
            }

            return id;
        }

        /// <summary>
        /// 转换为String
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetString(int id)
        {
            if (id == NullString)
                return null;

            if (id == EmptyString)
                return "";

            lock (_thisLocker)
            {
                if ((uint)id < _list.Count)
                    return _list[id];
            }

            return null;
        }

        private readonly object _thisLocker = new object();
        private readonly List<string> _list = new List<string>();
        private readonly Dictionary<string, int> _dictOfStr = new Dictionary<string, int>();

        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <returns></returns>
        public string[] ToStringArray()
        {
            lock (_thisLocker)
            {
                return _list.ToArray();
            }
        }
    }
}
