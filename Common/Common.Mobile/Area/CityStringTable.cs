using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 城市字符串表
    /// </summary>
    public class CityStringTable
    {
        public CityStringTable(IList<ProvinceStringTable> list)
        {
            Contract.Requires(list != null);

            List = list;
        }

        /// <summary>
        /// 字符串的二维数组
        /// </summary>
        public IList<ProvinceStringTable> List { get; private set; }
    }

    /// <summary>
    /// 省的字符串表
    /// </summary>
    public class ProvinceStringTable
    {
        public ProvinceStringTable(string name, string[] cities)
        {
            Name = name;
            Cities = cities;
        }

        /// <summary>
        /// 省
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 市
        /// </summary>
        public string[] Cities { get; private set; }
    }
}
