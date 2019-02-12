using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 开始或结束执行某条语句事件参数的基类
    /// </summary>
    public class ScriptExecutorExecuteStatementEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="statement"></param>
        internal ScriptExecutorExecuteStatementEventArgs(Statement statement)
        {
            Statement = statement;
        }

        /// <summary>
        /// 相关语句
        /// </summary>
        public Statement Statement { get; private set; }

        /// <summary>
        /// 获取标签名字
        /// </summary>
        public string GetLabelName()
        {
            LabelStatement label = Statement as LabelStatement;
            if (label == null)
                return null;

            return label.Name;
        }
    }

    /// <summary>
    /// 开始执行某条语句的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ScriptExecutorBeforeExecuteStatementEventHandler(object sender, ScriptExecutorBeforeExecuteStatementEventArgs e);

    /// <summary>
    /// 开始执行某条语句的事件参数
    /// </summary>
    public class ScriptExecutorBeforeExecuteStatementEventArgs : ScriptExecutorExecuteStatementEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="statement"></param>
        internal ScriptExecutorBeforeExecuteStatementEventArgs(Statement statement)
            : base(statement)
        {
            
        }

        /// <summary>
        /// 是否取消该条语句的执行
        /// </summary>
        public bool Cancel { get; private set; }
    }


    /// <summary>
    /// 结束执行某条语句的参数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ScriptExecutorEndExecuteStatementEventHandler(object sender, ScriptExecutorEndExecuteStatementEventArgs e);

    /// <summary>
    /// 结束执行某条语句
    /// </summary>
    public class ScriptExecutorEndExecuteStatementEventArgs : ScriptExecutorExecuteStatementEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="statement"></param>
        public ScriptExecutorEndExecuteStatementEventArgs(Statement statement, Exception error)
            : base(statement)
        {
            Error = error;
        }

        /// <summary>
        /// 出现的错误，成功时该值为null
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get { return Error == null; } }
    }
}
