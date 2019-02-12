using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using Common;

namespace ServiceFairy.Service.DatabaseCenter.Components
{
    /// <summary>
    /// 数据库管理器
    /// </summary>
    [AppComponent("数据库管理器", "维护数据库的连接")]
    class DbManagerAppComponent : AppComponent
    {
        public DbManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        const string CONNECTION_STRING_KEY = "connectionString";

        [ObjectProperty("数据库连接字符串")]
        private string ConnectionString
        {
            get
            {
                return _service.Context.ConfigReader.Get(CONNECTION_STRING_KEY);
            }
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public string GetConStr(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return ConnectionString;

            return null;
        }

        /// <summary>
        /// 获取所有的数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public string[] GetAllConStrs()
        {
            string conStr = GetConStr();
            return string.IsNullOrEmpty(conStr) ? Array<string>.Empty : new[] { conStr };
        }
    }
}
