using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 带有字段名的SQL表达式
    /// </summary>
    public interface ISqlExpressionWithFieldName
    {
        string[] GetFieldNames();
    }
}
