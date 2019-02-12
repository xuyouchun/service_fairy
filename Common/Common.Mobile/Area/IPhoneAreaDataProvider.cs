using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Entities;

namespace Common.Mobile.Area
{
    /// <summary>
    /// 手机号码归属地提供策略
    /// </summary>
    public interface IPhoneAreaDataProvider : IDisposable
    {
        /// <summary>
        /// 根据mcc查询运营商
        /// </summary>
        /// <param name="nationalCode">国家</param>
        /// <param name="mnc"></param>
        /// <returns></returns>
        OperationInfo GetOperationInfo(string nationalCode, string mnc);

        /// <summary>
        /// 获取所有运营商信息，按InternalPrefix的长度逆序排序
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <returns></returns>
        OperationInfo[] GetOperationInfos(string nationalCode);

        /// <summary>
        /// 获取所有国际长途前缀，按升序排序
        /// </summary>
        /// <returns></returns>
        NationalCodes GetNationalCodes();

        /// <summary>
        /// 获取运营商的前缀列表
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <returns></returns>
        OperationPrefixes GetOperationPrefixes(string nationalCode);

        /// <summary>
        /// 获取归属地数据（按升序排序）
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <param name="opInfo"></param>
        /// <param name="opPrefix"></param>
        /// <returns></returns>
        AreaDatas GetAreaDatas(string nationalCode, OperationInfo opInfo, string opPrefix);

        /// <summary>
        /// 获取城市字符串表
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <returns></returns>
        CityStringTable GetCityStringTable(string nationalCode);
    }
}
