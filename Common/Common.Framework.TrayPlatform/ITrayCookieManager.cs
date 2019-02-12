using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// Cookier管理器
    /// </summary>
    public interface ITrayCookieManager
    {
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="data">数据</param>
        void AddCookie(string type, byte[] data);

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="type"></param>
        void RemoveCookie(string type);

        /// <summary>
        /// 清空Cookie
        /// </summary>
        void ClearCookie();

        /// <summary>
        /// 获取指定类型的Cookie
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="serviceDesc">服务描述</param>
        /// <returns></returns>
        AppServiceCookieCollection GetCookies(string type = null, ServiceDesc serviceDesc = null);

        /// <summary>
        /// 设置应答Cookie
        /// </summary>
        /// <param name="cookies"></param>
        void SetReplyCookies(AppServiceCookieCollection cookies);

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        AppServiceCookie[] GetReplyCookies(string type);
    }
}
