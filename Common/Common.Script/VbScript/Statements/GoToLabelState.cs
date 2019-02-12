using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 用于控制Goto的行为
    /// </summary>
    class GoToLabelState
    {
        public GoToLabelState(RunningContext context, LabelPath labelPath)
        {
            RunningContext = context;
            LabelPath = labelPath;

            _Initialize();
        }

        public RunningContext RunningContext { get; private set; }

        /// <summary>
        /// Label的路径
        /// </summary>
        public LabelPath LabelPath { get; private set; }

        private int _StartIndex;

        public bool MoveNext()
        {
            if (_StartIndex >= LabelPath.Path.Length - 1)
                return false;

            _StartIndex++;
            return true;
        }

        /// <summary>
        /// Goto语句与label共同的父节点
        /// </summary>
        public Statement RootStatement { get; private set; }

        /// <summary>
        /// 前一个代码块
        /// </summary>
        public Statement PreviousStatement
        {
            get
            {
                if (_StartIndex < 1)
                    return null;

                return LabelPath.Path[_StartIndex - 1];
            }
        }

        /// <summary>
        /// 是否已经退出根节点
        /// </summary>
        public bool HasExitToRoot { get; set; }

        /// <summary>
        /// 当前需要执行到的代码块
        /// </summary>
        public Statement CurrentStatement
        {
            get
            {
                if (_StartIndex >= LabelPath.Path.Length || _StartIndex < 0)
                    return null;

                return LabelPath.Path[_StartIndex];
            }
        }

        /// <summary>
        /// 下一个代码块
        /// </summary>
        public Statement NextStatement
        {
            get
            {
                int index = _StartIndex + 1;
                if (index >= LabelPath.Path.Length || index < 0)
                    return null;

                return LabelPath.Path[index];
            }
        }

        private void _Initialize()
        {
            foreach (Statement statement in RunningContext.StatementStack)
            {
                if (LabelPath.Contains(statement))
                {
                    _StartIndex = ((IList<Statement>)LabelPath.Path).IndexOf(statement);
                    RootStatement = statement;
                    break;
                }
            }
        }

        public bool AtEnd()
        {
            return _StartIndex >= LabelPath.Path.Length - 1;
        }
    }
}
