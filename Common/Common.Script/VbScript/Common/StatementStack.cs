using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 记录代码的执行路径
    /// </summary>
    class StatementStack : IEnumerable<Statement>
    {
        public void Push(Statement statement)
        {
            _List.Add(statement);
        }

        private readonly List<Statement> _List = new List<Statement>();

        public void Pop()
        {
            _List.RemoveAt(_List.Count - 1);
        }

        public int Count { get { return _List.Count; } }

        public Statement this[int index]
        {
            get { return _List[index]; }
        }

        #region IEnumerable<Statement> 成员

        public IEnumerator<Statement> GetEnumerator()
        {
            for (int k = _List.Count - 1; k >= 0; k--)
            {
                yield return _List[k];
            }
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// 获取当前的语句块
        /// </summary>
        /// <returns></returns>
        public Statement GetCurrent()
        {
            if (_List.Count == 0)
                return null;

            return _List[_List.Count - 1];
        }
    }
}
