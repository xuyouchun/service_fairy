using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 标签库的管理器
    /// </summary>
    interface IStatementField
    {
        /// <summary>
        /// 获取标签库
        /// </summary>
        /// <returns></returns>
        LabelLibrary GetLabelLibrary();

        /// <summary>
        /// 在遇到错误时是否继续运行
        /// </summary>
        bool OnErrorResumeNext { get; set; }
    }
}
