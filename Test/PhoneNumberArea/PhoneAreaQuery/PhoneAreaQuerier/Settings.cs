using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Contracts.Entities;
using System.IO;
using System.Xml;
using Common.Mobile;

namespace PhoneAreaQuerier
{
    static class Settings
    {
        /// <summary>
        /// 归属地数据的基路径
        /// </summary>
        public static string AreaDataBasePath
        {
            get { return CommonUtility.GetFromAppConfig("areaDataBasePath", ""); }
        }

        public static string DefaultCountryIsoCode
        {
            get { return CommonUtility.GetFromAppConfig("defaultCountryIsoCode", "cn"); }
        }

        public static string DefaultOperationMnc
        {
            get { return CommonUtility.GetFromAppConfig("defaultOperationMnc", "00"); }
        }

        private static readonly OperationCountryManager _ocMgr = new OperationCountryManager(Path.Combine(AreaDataBasePath, "operations.xml"));

        /// <summary>
        /// 获取全部的OperationCountry
        /// </summary>
        /// <returns></returns>
        public static OperationCountry[] GetAllOperationCountries()
        {
            return _ocMgr.GetAllOperationCountries();
        }

        /// <summary>
        /// 获取指定国家的的OperationCountry
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public static OperationCountry GetOperationCountry(CountryInfo cInfo)
        {
            return _ocMgr.GetOperationCountry(cInfo);
        }
    }
}
