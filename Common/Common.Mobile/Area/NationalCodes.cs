using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 国际长途前缀
    /// </summary>
    public class NationalCodes
    {
        public NationalCodes(int minLength, int maxLength, IList<ushort> list)
        {
            Contract.Requires(minLength >= 0 && maxLength >= 0 && list != null);

            MinLength = minLength;
            MaxLength = maxLength;
            List = list;
        }

        /// <summary>
        /// 最大位数
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// 最小位数
        /// </summary>
        public int MinLength { get; private set; }

        /// <summary>
        /// 国际长途区号集合，升序排列
        /// </summary>
        public IList<ushort> List { get; private set; }
    }
}
