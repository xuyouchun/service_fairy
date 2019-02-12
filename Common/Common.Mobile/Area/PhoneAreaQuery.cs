using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Entities;
using System.Diagnostics.Contracts;
using Common.Package;
using Common.Algorithms;
using Common.Utility;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 电话号码归属地查询器
    /// </summary>
    public class PhoneAreaQuery : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="thisCountry">当前国家</param>
        /// <param name="mnc">运营商标识</param>
        /// <param name="dataProvider">数据提供</param>
        private PhoneAreaQuery(CountryInfo thisCountry, string mnc, IPhoneAreaDataProvider dataProvider)
        {
            Contract.Requires(thisCountry != null && dataProvider != null);

            _thisCountry = thisCountry;
            _mnc = mnc;
            _provider = dataProvider;

            _opInfo = new Lazy<OperationInfo>(() => dataProvider.GetOperationInfo(thisCountry.NationalCode, mnc));
            _nationalCodes = new Lazy<NationalCodes>(() => dataProvider.GetNationalCodes());
        }

        private readonly CountryInfo _thisCountry;
        private readonly string _mnc;
        private readonly IPhoneAreaDataProvider _provider;
        private readonly Lazy<NationalCodes> _nationalCodes;
        private readonly Lazy<OperationInfo> _opInfo;

        /// <summary>
        /// 获取手机号码归属地信息
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public virtual PhoneArea GetPhoneArea(string phoneNumber)
        {
            if (phoneNumber == null || (phoneNumber = phoneNumber.Trim()).Length == 0)
                return PhoneArea.Empty;

            string nationalCode;

            // 确定国际长途还是国内长途
            PhoneCallType callType = _AnalyseNationalCall(ref phoneNumber, out nationalCode);

            // 确定运营商
            string opPrefix, province = "", city = "";
            OperationInfo[] opInfos;
            AreaData areaData;

            if (_TryAnalyseOperation(ref phoneNumber, nationalCode, out opPrefix, out opInfos))  // 该函数返回可能的运营商
            {
                // 确定归属地
                if (_AnalyseArea(ref phoneNumber, nationalCode, opInfos, opPrefix, out areaData))
                {
                    // 查询字符串表，获得省、市的名称
                    CityStringTable cst = _provider.GetCityStringTable(nationalCode);
                    province = cst.List[areaData.ProvinceIndex].Name;
                    city = cst.List[areaData.ProvinceIndex].Cities[areaData.CityIndex];
                }
            }

            return new PhoneArea(CountryInfo.FromNationalCode(nationalCode), opInfos, province, city);
        }

        // 确定是国内长途还是国际长途
        private PhoneCallType _AnalyseNationalCall(ref string phoneNumber, out string nationalCode)
        {
            OperationInfo opInfo = _opInfo.Value;

            string prefix;
            if (phoneNumber.StartsWith(prefix = "+")  // 以“+”开头都认为是国际长途
                || (!string.IsNullOrEmpty(opInfo.NationalPrefix) && phoneNumber.StartsWith(prefix = opInfo.NationalPrefix)))
            {
                phoneNumber = phoneNumber.Substring(prefix.Length);
                if (!_AnalyseNationalPrefix(ref phoneNumber, out nationalCode))
                    throw new PhoneAreaQueryException("错误的国际长途区号");

                return (nationalCode == _thisCountry.NationalCode) ? PhoneCallType.InternalCall : PhoneCallType.NationalCall;
            }

            nationalCode = _thisCountry.NationalCode;
            return PhoneCallType.NationalCall;
        }

        // 提取国际长途区号
        private bool _AnalyseNationalPrefix(ref string phoneNumber, out string nationCode)
        {
            NationalCodes nationalCodes = _nationalCodes.Value;
            for (int len = Math.Min(nationalCodes.MaxLength, phoneNumber.Length), min = Math.Max(1, nationalCodes.MinLength); len >= min; len--)
            {
                ushort iNativeCode;
                if (!ushort.TryParse(nationCode = phoneNumber.Substring(0, len), out iNativeCode))
                    continue;

                int index = Algorithm.BinarySearch(nationalCodes.List, iNativeCode, SortType.Asc);
                if (index >= 0)
                {
                    phoneNumber = phoneNumber.Substring(len);
                    return true;
                }
            }

            nationCode = null;
            return false;
        }

        private bool _TryAnalyseOperation(ref string phoneNumber, string nationalCode, out string opPrefix, out OperationInfo[] opInfos)
        {
            try
            {
                opInfos = _AnalyseOperation(ref phoneNumber, nationalCode, out opPrefix);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);

                opPrefix = null;
                opInfos = new OperationInfo[0];
                return false;
            }
        }

        // 确定运营商
        private OperationInfo[] _AnalyseOperation(ref string phoneNumber, string nationalCode, out string opPrefix)
        {
            // 先从运营商国内长途前缀表中搜索
            OperationInfo[] opInfos = _provider.GetOperationInfos(nationalCode);
            for (int k = 0, length = opInfos.Length; k < length; k++)
            {
                OperationInfo opInfo0 = opInfos[k];
                string internalPrefix = opInfo0.InternalPrefix;
                if (internalPrefix.Length == 0)
                    break;

                if (phoneNumber.StartsWith(internalPrefix))
                {
                    int i = k;
                    while (++i < opInfos.Length && opInfos[i].InternalPrefix == internalPrefix);

                    phoneNumber = phoneNumber.Substring(internalPrefix.Length);
                    opPrefix = "";
                    return CollectionUtility.Range(opInfos, k, i - k);
                }
            }

            // 再从运营商前缀表中搜索
            OperationPrefixes opProfixes = _provider.GetOperationPrefixes(nationalCode);
            for (int len = Math.Min(opProfixes.MaxLength, phoneNumber.Length); len >= 0; len--)
            {
                string prefix = phoneNumber.Substring(0, len);
                int index = Algorithm.BinarySearch<OperationPrefix, string>(opProfixes.List, prefix,
                    (item) => item.Prefix, SortType.Asc);

                if (index >= 0)
                {
                    opPrefix = prefix;
                    phoneNumber = phoneNumber.Substring(len);

                    int left = index, right = index;
                    while (left > 0 && opProfixes.List[left - 1].Prefix == prefix) left--;
                    while (right < opProfixes.List.Count - 1 && opProfixes.List[right + 1].Prefix == prefix) right++;
                    return opProfixes.List.Range(item => item.OperationInfo, left, right - left + 1);
                }
            }

            opPrefix = "";
            return new OperationInfo[0];
        }

        // 确定归属地
        private bool _AnalyseArea(ref string phoneNumber, string nationalCode, OperationInfo[] opInfos, string opPrefix, out AreaData areaData)
        {
            foreach (OperationInfo opInfo in opInfos)
            {
                if (_AnalyseArea(ref phoneNumber, nationalCode, opInfo, opPrefix, out areaData))
                    return true;
            }

            areaData = null;
            return false;
        }

        private bool _AnalyseArea(ref string phoneNumber, string nationalCode, OperationInfo opInfo, string opPrefix, out AreaData areaData)
        {
            AreaDatas areaDatas = _provider.GetAreaDatas(nationalCode, opInfo, opPrefix);
            IList<AreaData> ads = areaDatas.List;

            for (int width = Math.Min(areaDatas.MaxLength, phoneNumber.Length); width >= areaDatas.MinLength; width--)
            {
                ushort areaCode = ushort.Parse(phoneNumber.Substring(0, width));
                if (areaDatas.AreaDataModel == AreaDataModel.Full)  // 完整存储模式
                {
                    int index = Algorithm.BinarySearch<AreaData, ushort>(ads, areaCode, item => item.AreaCode, SortType.Asc);
                    if (index >= 0)
                    {
                        areaData = ads[index];
                        return true;
                    }
                }
                else if (areaDatas.AreaDataModel == AreaDataModel.Ellipsis)  // 简略存储模式
                {
                    int begin = 0, end = ads.Count - 1, mid;
                    while (begin <= end)
                    {
                        mid = (begin + end) / 2;
                        AreaData ad = ads[mid];

                        if (areaCode >= ad.AreaCode && (mid >= end || areaCode < ads[mid + 1].AreaCode))
                        {
                            areaData = ad;
                            return true;
                        }

                        if (areaCode < ad.AreaCode)
                        {
                            end = mid - 1;
                        }
                        else
                        {
                            begin = mid + 1;
                        }
                    }
                }
                else
                    throw new PhoneAreaQueryException("不支持该种存储模式：" + areaDatas.AreaDataModel);
            }

            areaData = null;
            return false;
        }

        /// <summary>
        /// 创建查询器
        /// </summary>
        /// <param name="thisCountry">本国信息</param>
        /// <param name="mnc">运营商</param>
        /// <param name="dataProvider">数据提供</param>
        /// <returns></returns>
        public static PhoneAreaQuery CreatePhoneAreaQuery(CountryInfo thisCountry, string mnc, IPhoneAreaDataProvider dataProvider)
        {
            Contract.Requires(thisCountry != null && mnc != null && dataProvider != null);

            return new PhoneAreaQuery(thisCountry, mnc, dataProvider);
        }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
