using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 归属地集合
    /// </summary>
    public class AreaDatas
    {
        public AreaDatas(AreaDataModel areaDataModel, int minLength, int maxLength, IList<AreaData> list)
        {
            Contract.Requires(minLength >= 0 && maxLength >= 0 && list != null);

            AreaDataModel = areaDataModel;
            MinLength = minLength;
            MaxLength = maxLength;
            List = list;
        }

        /// <summary>
        /// 数据存储模式
        /// </summary>
        public AreaDataModel AreaDataModel { get; private set; }

        /// <summary>
        /// 最小宽度
        /// </summary>
        public int MinLength { get; private set; }

        /// <summary>
        /// 最大宽度
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// 归属地
        /// </summary>
        public IList<AreaData> List { get; private set; }
    }

    /// <summary>
    /// 归属地
    /// </summary>
    public class AreaData
    {
        public AreaData(ushort areaCode, byte provinceIndex, byte cityIndex)
        {
            AreaCode = areaCode;
            ProvinceIndex = provinceIndex;
            CityIndex = cityIndex;
        }

        /// <summary>
        /// 归属地号码
        /// </summary>
        public ushort AreaCode { get; private set; }

        /// <summary>
        /// 省索引
        /// </summary>
        public byte ProvinceIndex { get; private set; }

        /// <summary>
        /// 市索引
        /// </summary>
        public byte CityIndex { get; set; }
    }

    /// <summary>
    /// 归属地数据存储模式
    /// </summary>
    public enum AreaDataModel
    {
        /// <summary>
        /// 完整存储
        /// </summary>
        Full = 1,

        /// <summary>
        /// 省掉递增的行
        /// </summary>
        Ellipsis = 2,
    }
}
