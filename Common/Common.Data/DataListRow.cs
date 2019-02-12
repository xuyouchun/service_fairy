using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Common.Data
{
    /// <summary>
    /// 数据项
    /// </summary>
    [Serializable, DataContract]
    //[System.Diagnostics.DebuggerTypeProxy(typeof(DebuggerProxy))]
    public class DataListRow
    {
        internal DataListRow(int fieldCount)
        {
            Cells = new object[fieldCount];
        }

        internal DataListRow(object[] cells)
        {
            Cells = cells;
        }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public object[] Cells { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < Cells.Length; k++)
            {
                if (k > 0)
                    sb.Append(", ");

                sb.Append(Cells[k] ?? "<null>");
            }

            return sb.ToString();
        }
    }
}
