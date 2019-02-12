using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 标签库
    /// </summary>
    class LabelLibrary
    {
        private LabelLibrary()
        {

        }

        private void _AddLabelPath(LabelPath labelPath)
        {
            string name = labelPath.LabelName.ToUpper();
            if (_Dict.ContainsKey(name))
                throw new ScriptRuntimeException("包含重复的标签: " + name);

            _Dict.Add(name, labelPath);
        }

        public LabelPath GetLabelPath(string name)
        {
            name = name.ToUpper();
            LabelPath lp;

            _Dict.TryGetValue(name, out lp);
            return lp;
        }

        private readonly Dictionary<string, LabelPath> _Dict = new Dictionary<string, LabelPath>();

        public static LabelLibrary LoadFromStatement(Statement statement)
        {
            LabelLibrary library = new LabelLibrary();
            foreach (LabelPath lp in _GetLabelPaths(statement, new Stack<Statement>()))
            {
                library._AddLabelPath(lp);
            }

            return library;
        }

        private static IEnumerable<LabelPath> _GetLabelPaths(Statement statement, Stack<Statement> stack)
        {
            LabelStatement labelStatement = statement as LabelStatement;
            if (labelStatement != null)
            {
                List<Statement> list = new List<Statement>(stack.ToArray());
                list.Reverse();
                list.Add(labelStatement);

                yield return new LabelPath(labelStatement.Name, list.ToArray());
                yield break;
            }

            stack.Push(statement);

            foreach (Statement st in statement.GetChildStatements())
            {
                foreach (LabelPath path in _GetLabelPaths(st, stack))
                {
                    yield return path;
                }
            }

            stack.Pop();
        }
    }
}
