using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Common.Script.VbScript.Statements
{
    static class KeywordManager
    {
        static KeywordManager()
        {
            foreach (FieldInfo fInfo in typeof(Keywords).GetFields())
            {
                object[] attrs = fInfo.GetCustomAttributes(typeof(KeywordAttribute), false);
                if (attrs.Length > 0)
                {
                    KeywordAttribute attr = (KeywordAttribute)attrs[0];
                    KeywordInfo info = new KeywordInfo(attr.Keyword.ToUpper(), (Keywords)fInfo.GetValue(null), attr.ShowInList);
                    _Dict.Add(attr.Keyword, info);
                    _Dict2.Add(info.Keyword, info);
                }
            }
        }

        static readonly Dictionary<string, KeywordInfo> _Dict = new Dictionary<string, KeywordInfo>();
        static readonly Dictionary<Keywords, KeywordInfo> _Dict2 = new Dictionary<Keywords, KeywordInfo>();

        public static KeywordInfo GetKeywordInfo(string keyword)
        {
            if (keyword == null)
                return null;

            KeywordInfo info;
            _Dict.TryGetValue(keyword.ToUpper(), out info);

            return info;
        }

        public static KeywordInfo GetKeywordInfo(Keywords keyword)
        {
            KeywordInfo info;
            _Dict2.TryGetValue(keyword, out info);

            return info;
        }

        public static IEnumerable<KeywordInfo> GetAllKeywordInfos()
        {
            return _Dict2.Values;
        }
    }
}
