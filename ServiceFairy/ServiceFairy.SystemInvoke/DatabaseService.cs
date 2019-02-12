using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 数据库服务
    /// </summary>
    public static class DatabaseService
    {
        private static string _CreateServiceName(string method)
        {
            return SFNames.ServiceNames.Database + "/" + method;
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Database_Select_Reply> Select(IServiceClient serviceClient,
            Database_Select_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Database_Select_Reply>(_CreateServiceName("Select"), request, settings);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Database_Insert_Reply> Insert(IServiceClient serviceClient,
            Database_Insert_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Database_Insert_Reply>(_CreateServiceName("Insert"), request, settings);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Database_Update_Reply> Update(IServiceClient serviceClient,
            Database_Update_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Database_Update_Reply>(_CreateServiceName("Update"), request, settings);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Database_Delete_Reply> Delete(IServiceClient serviceClient,
            Database_Delete_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Database_Delete_Reply>(_CreateServiceName("Delete"), request, settings);
        }

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Database_Merge_Reply> Merge(IServiceClient serviceClient,
            Database_Merge_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Database_Merge_Reply>(_CreateServiceName("Merge"), request, settings);
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Database_GetTableInfos_Reply> GetTableInfos(IServiceClient serviceClient,
            Database_GetTableInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Database_GetTableInfos_Reply>(_CreateServiceName("GetTableInfos"), request, settings);
        }

        /// <summary>
        /// 初始化表的元数据
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult InitMetadata(IServiceClient serviceClient,
            Database_InitMetadata_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_CreateServiceName("InitMetadata"), request, settings);
        }

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Database_ExecuteSql_Reply> ExecuteSql(IServiceClient serviceClient,
            Database_ExecuteSql_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Database_ExecuteSql_Reply>(_CreateServiceName("ExecuteSql"), request, settings);
        }
    }
}
