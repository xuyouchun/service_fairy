using System;
using System.Collections.Generic;
using System.Text;
using KeywordManager = global::Common.Script.VbScript.Statements.KeywordManager;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 关键字列表
    /// </summary>
    public static class KeywordManager
    {
        static KeywordManager()
        {
            foreach (var info in Common.Client.Script.Statements.KeywordManager.GetAllKeywordInfos())  // 关键字
            {
                if (info.ShowInList)
                    _Infos.Add(new KeywordInfo(info.Name));
            }

            foreach (var info in Common.Client.Script.Expressions.OperatorAttribute.GetAllOperatorInfos())
            {
                if (info.ShowInList)
                    _Infos.Add(new KeywordInfo(info.OperatorName));
            }

            foreach (string item in new string[] { "TRUE", "FALSE", "NOTHING", "EMPTY", "NULL", "EXIT", "END", "ON ERROR RESUME NEXT" })
            {
                _Infos.Add(new KeywordInfo(item));
            }
        }

        private static readonly List<KeywordInfo> _Infos = new List<KeywordInfo>();

        /// <summary>
        /// 获取所有关键字
        /// </summary>
        /// <returns></returns>
        public static KeywordInfo[] GetAllKeywords()
        {
            return _Infos.ToArray();
        }
    }

    /// <summary>
    /// 关键字信息
    /// </summary>
    public class KeywordInfo
    {
        public KeywordInfo(string name)
        {
            Name = _Convert(name);
        }

        private static string _Convert(string name)  // 转换为首字母大写的形式
        {
            if (name.IndexOf(' ') < 0)
                return _ConvertName(name);

            StringBuilder sb = new StringBuilder();
            foreach (string part in name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (sb.Length > 0)
                    sb.Append(" ");

                sb.Append(_ConvertName(part));
            }

            return sb.ToString();
        }

        private static string _ConvertName(string name)  // 将单词转换为首字母大写的形式
        {
            return char.ToUpper(name[0]) + name.Substring(1).ToLower();
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
