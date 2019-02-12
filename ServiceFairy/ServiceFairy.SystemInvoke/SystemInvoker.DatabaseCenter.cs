using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.DatabaseCenter;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private DatabaseCenterInvoker _databaseCenter;

        /// <summary>
        /// DatabaseCenter Service
        /// </summary>
        public DatabaseCenterInvoker DatabaseCenter
        {
            get { return _databaseCenter ?? (_databaseCenter = new DatabaseCenterInvoker(this)); }
        }

        /// <summary>
        /// 数据库中心服务
        /// </summary>
        public class DatabaseCenterInvoker : Invoker
        {
            public DatabaseCenterInvoker(SystemInvoker invoker)
                : base(invoker)
            {

            }

            /// <summary>
            /// 获取数据库连接字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns>数据库连接字符串</returns>
            public ServiceResult<string> GetConStrSr(string name = null, CallingSettings settings = null)
            {
                DatabaseCenter_GetConStr_Request req = new DatabaseCenter_GetConStr_Request { Name = name };
                var sr = DatabaseCenterService.GetConStr(Sc, req, settings);

                return CreateSr(sr, r => r.ConStr);
            }

            /// <summary>
            /// 获取数据库连接字符串
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns>数据库连接字符串</returns>
            public string GetConStr(string name = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetConStrSr(name, settings));
            }
        }
    }
}
