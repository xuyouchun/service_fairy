using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Common.Contracts.Entities
{

    /// <summary>
    /// 用于标识国家信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    class CountryAttribute : Attribute
    {
        public CountryAttribute(string englishName, string isoCode, string isoCode3, string chineseName, string internationalCode, string mcc, float timeZone)
        {
            CountryInfo = new CountryInfo(englishName, isoCode, isoCode3, chineseName, internationalCode, mcc, timeZone);
        }

        /// <summary>
        /// 国家信息
        /// </summary>
        public CountryInfo CountryInfo { get; private set; }

        private static readonly Lazy<CountryInfo[]> _infos = new Lazy<CountryInfo[]>(_LoadAllCountryInfos);

        private static CountryInfo[] _LoadAllCountryInfos()
        {
            Type t = typeof(CountryNames);
            BindingFlags flags = BindingFlags.Static | BindingFlags.Public;
            return t.SearchMembers<CountryInfo, CountryAttribute>((attrs, mInfo) => attrs[0].CountryInfo, flags).OrderBy(cInfo => cInfo.ChineseName).ToArray();
        }

        /// <summary>
        /// 获取所有的国家信息
        /// </summary>
        /// <returns></returns>
        public static CountryInfo[] GetAllCountryInfos()
        {
            return _infos.Value;
        }
    }

    /// <summary>
    /// 国家信息
    /// </summary>
    public class CountryInfo
    {
        internal CountryInfo(string englishName, string short2Name, string short3Name, string chineseName, string nationalCode, string mcc, float timeZone)
        {
            EnglishName = englishName;
            IsoCode = short2Name;
            IsoCode3 = short3Name;
            ChineseName = chineseName;
            NationalCode = nationalCode;
            Mcc = mcc;
            TimeZone = timeZone;
        }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName { get; private set; }

        /// <summary>
        /// 两个字母表示的短名称
        /// </summary>
        public string IsoCode { get; private set; }

        /// <summary>
        /// 三个字母表示的短名称
        /// </summary>
        public string IsoCode3 { get; private set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        public string ChineseName { get; private set; }

        /// <summary>
        /// 国际长途区号
        /// </summary>
        public string NationalCode { get; private set; }

        /// <summary>
        /// 移动国家码
        /// </summary>
        public string Mcc { get; private set; }

        /// <summary>
        /// 时区
        /// </summary>
        public float TimeZone { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", ChineseName, EnglishName);
        }

        private static readonly Lazy<Dictionary<string, CountryInfo>> _dictOfEnName = new Lazy<Dictionary<string, CountryInfo>>(delegate {
            return CountryAttribute.GetAllCountryInfos().ToDictionary(info => info.EnglishName.ToLower());
        });

        /// <summary>
        /// 从英文名称来寻找
        /// </summary>
        /// <param name="englishName"></param>
        /// <returns></returns>
        public static CountryInfo FromEnglishName(string englishName)
        {
            Contract.Requires(englishName != null);
            return _dictOfEnName.Value.GetOrDefault(englishName.ToLower());
        }

        private static readonly Lazy<Dictionary<string, CountryInfo>> _dictOfCnName = new Lazy<Dictionary<string, CountryInfo>>(delegate {
            return CountryAttribute.GetAllCountryInfos().ToDictionary(info => info.ChineseName.ToLower());
        });

        /// <summary>
        /// 从中文名称来寻找
        /// </summary>
        /// <param name="chineseName"></param>
        /// <returns></returns>
        public static CountryInfo FromChineseName(string chineseName)
        {
            Contract.Requires(chineseName != null);
            return _dictOfCnName.Value.GetOrDefault(chineseName.ToLower());
        }

        private static readonly Lazy<Dictionary<string, CountryInfo>> _dictOfShort2Name = new Lazy<Dictionary<string, CountryInfo>>(delegate {
            return CountryAttribute.GetAllCountryInfos().Where(info=>!string.IsNullOrEmpty(info.IsoCode)).ToDictionary(info => info.IsoCode.ToLower());
        });

        private static readonly Lazy<Dictionary<string, CountryInfo>> _dictOfShort3Name = new Lazy<Dictionary<string, CountryInfo>>(delegate {
            return CountryAttribute.GetAllCountryInfos().Where(info => !string.IsNullOrEmpty(info.IsoCode3)).ToDictionary(info => info.IsoCode3.ToLower());
        });

        /// <summary>
        /// 根据2字母的国际代码寻找
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        public static CountryInfo FromIsoCode(string shortName)
        {
            Contract.Requires(shortName != null);

            if (shortName.Length == 2)
                return _dictOfShort2Name.Value.GetOrDefault(shortName.ToLower());
            else
                return _dictOfShort3Name.Value.GetOrDefault(shortName.ToLower());
        }

        private static readonly Lazy<Dictionary<string, CountryInfo[]>> _dictOfInternationalCode = new Lazy<Dictionary<string, CountryInfo[]>>(delegate {
            return CountryAttribute.GetAllCountryInfos().Where(info => !string.IsNullOrEmpty(info.NationalCode))
                .GroupBy(info => info.NationalCode).ToDictionary(g => g.Key, g => g.ToArray());
        });

        /// <summary>
        /// 根据国际区号寻找
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <returns></returns>
        public static CountryInfo[] FromNationalCode(string nationalCode)
        {
            Contract.Requires(nationalCode != null);
            return _dictOfInternationalCode.Value.GetOrDefault(nationalCode) ?? new CountryInfo[0];
        }

        private static readonly Lazy<Dictionary<string, CountryInfo>> _dictOfMcc = new Lazy<Dictionary<string, CountryInfo>>(delegate {
            return CountryAttribute.GetAllCountryInfos().Where(info => !string.IsNullOrEmpty(info.Mcc)).ToDictionary(info => info.Mcc);
        });

        /// <summary>
        /// 根据移动国家码MCC来查找
        /// </summary>
        /// <param name="mcc"></param>
        /// <returns></returns>
        public static CountryInfo FromMcc(string mcc)
        {
            Contract.Requires(mcc != null);
            return _dictOfMcc.Value.GetOrDefault(mcc);
        }

        /// <summary>
        /// 获取所有的国家信息
        /// </summary>
        /// <returns></returns>
        public static CountryInfo[] GetAll()
        {
            return CountryAttribute.GetAllCountryInfos();
        }
    }
}
