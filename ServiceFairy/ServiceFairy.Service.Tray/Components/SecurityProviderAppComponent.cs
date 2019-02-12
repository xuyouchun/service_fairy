using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 安全策略
    /// </summary>
    [AppComponent("安全策略", "连接安全服务，验证用户的安全级别")]
    class SecurityProviderAppComponent : AppComponent, ISecurityProvider
    {
        public SecurityProviderAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        public UserSessionState GetUserSessionState(Sid sid)
        {
            return _service.UserManager.GetSessionState(sid, throwError: false);
        }

        public UserBasicInfo GetUserBasicInfo(string username)
        {
            return _service.UserManager.GetUserBasicInfo(username, throwError: false);
        }
    }
}
