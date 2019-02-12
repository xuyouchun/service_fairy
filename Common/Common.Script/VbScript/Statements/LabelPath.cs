using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 标签路径
    /// </summary>
    class LabelPath
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="labelName"></param>
        /// <param name="path"></param>
        public LabelPath(string labelName, Statement[] path)
        {
            LabelName = labelName;
            Path = path;
            _List = new ContainsList<Statement>(Path);
        }

        public string LabelName { get; private set; }

        /// <summary>
        /// 路径
        /// </summary>
        public Statement[] Path { get; private set; }

        private readonly ContainsList<Statement> _List;

        /// <summary>
        /// 是否包含指定的代码块
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public bool Contains(Statement statement)
        {
            return _List.Contains(statement);
        }

        public override string ToString()
        {
            return LabelName;
        }
    }
}
