using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    class KeywordInfo
    {
        public KeywordInfo(string name, Keywords keyword, bool showInList)
        {
            Name = name;
            Keyword = keyword;
            ShowInList = showInList;
        }

        public string Name { get; private set; }

        public Keywords Keyword { get; private set; }

        /// <summary>
        /// 指示当用编辑器写代码时，该关键字是否应该出现在列表中
        /// </summary>
        public bool ShowInList { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
