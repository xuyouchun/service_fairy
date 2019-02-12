using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 运营商配置信息
    /// </summary>
    public class OperationInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalPrefix"></param>
        /// <param name="nationalPrefix"></param>
        /// <param name="desc"></param>
        public OperationInfo(string nationalPrefix, string internalPrefix, string desc)
        {
            Contract.Requires(nationalPrefix != null && internalPrefix != null && desc != null);

            NationalPrefix = nationalPrefix;
            InternalPrefix = internalPrefix;
            Desc = desc;
        }

        /// <summary>
        /// 国除长途前缀
        /// </summary>
        public string NationalPrefix { get; private set; }

        /// <summary>
        /// 国内长途前缀
        /// </summary>
        public string InternalPrefix { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Desc { get; private set; }
    }
}
