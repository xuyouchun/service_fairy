using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Entities;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 电话号码归属地描述
    /// </summary>
    public class PhoneArea
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="countryInfos">国家</param>
        /// <param name="opInfos">运营商</param>
        /// <param name="provinceName">省、州</param>
        /// <param name="cityName">城市</param>
        public PhoneArea(CountryInfo[] countryInfos, OperationInfo[] opInfos, string provinceName, string cityName)
        {
            Contract.Requires(countryInfos != null);

            CountryInfos = countryInfos;
            OperationInfos = opInfos;
            ProvinceName = provinceName;
            CityName = cityName;
        }

        /// <summary>
        /// 国家
        /// </summary>
        public CountryInfo[] CountryInfos { get; private set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public OperationInfo[] OperationInfos { get; private set; }

        /// <summary>
        /// 省、州
        /// </summary>
        public string ProvinceName { get; private set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; private set; }

        /// <summary>
        /// 空的号码归属地数据
        /// </summary>
        public static readonly PhoneArea Empty = new PhoneArea(new CountryInfo[0], new OperationInfo[0], "", "");

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!CountryInfos.IsNullOrEmpty())
            {
                sb.Append(string.Join("/", CountryInfos.Select(ci => ci.ChineseName)));
            }

            if (!string.IsNullOrWhiteSpace(ProvinceName))
            {
                sb.Append(" ").Append(ProvinceName.Trim());
            }

            if (!string.IsNullOrWhiteSpace(CityName))
            {
                sb.Append(" ").Append(CityName.Trim());
            }

            if (!OperationInfos.IsNullOrEmpty())
            {
                sb.Append(" ").Append(OperationInfos[0].Desc);
                if (OperationInfos.Length > 1)
                    sb.Append("...");
            }

            return sb.ToString();
        }
    }
}
