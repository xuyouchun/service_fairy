using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    [global::System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class KeywordAttribute : Attribute
    {
        public KeywordAttribute(string keyword)
            : this(keyword, true)
        {
            
        }

        public KeywordAttribute(string keyword, bool showInList)
        {
            Keyword = keyword;
            ShowInList = showInList;
        }

        public string Keyword { get; private set; }

        public bool ShowInList { get; private set; }
    }
}
