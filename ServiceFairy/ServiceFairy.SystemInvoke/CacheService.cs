using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Cache;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public static class CacheService
    {
        /// <summary>
        /// 获取一项缓存
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Cache_Get_Reply> Get(IServiceClient serviceClient, Cache_Get_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            
            return serviceClient.Call<Cache_Get_Reply>(SFNames.ServiceNames.Cache + "/Get", request, settings);
        }

        /// <summary>
        /// 批量获取缓存
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Cache_GetRange_Reply> GetRange(IServiceClient serviceClient, Cache_GetRange_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Cache_GetRange_Reply>(SFNames.ServiceNames.Cache + "/GetRange", request, settings);
        }

        /// <summary>
        /// 设置一项缓存
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Set(IServiceClient serviceClient, Cache_Set_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Cache + "/Set", request, settings);
        }

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SetRange(IServiceClient serviceClient, Cache_SetRange_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Cache + "/SetRange", request, settings);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Remove(IServiceClient serviceClient, Cache_Remove_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Cache + "/Remove", request, settings);
        }

        /// <summary>
        /// 批量获取键，只返回存在的键
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Cache_GetKeys_Reply> GetKeys(IServiceClient serviceClient, Cache_GetKeys_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Cache_GetKeys_Reply>(SFNames.ServiceNames.Cache + "/GetKeys", request, settings);
        }

        /// <summary>
        /// 将缓存赋予一个增量
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Cache_Increase_Reply> Increase(IServiceClient serviceClient, Cache_Increase_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Cache_Increase_Reply>(SFNames.ServiceNames.Cache + "/Increase", request, settings);
        }
    }
}
