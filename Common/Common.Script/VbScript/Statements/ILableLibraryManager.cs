using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Statements
{
    /// <summary>
    /// 标签库的管理器
    /// </summary>
    interface ILableLibraryManager
    {
        /// <summary>
        /// 获取标签库
        /// </summary>
        /// <returns></returns>
        LabelLibrary GetLabelLibrary();
    }
}
