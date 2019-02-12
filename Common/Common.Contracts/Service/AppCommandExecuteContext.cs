using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// AppCommand的上下文执行环境
    /// </summary>
    public class AppCommandExecuteContext
    {
        public AppCommandExecuteContext(IAppService service, IServiceProvider serviceProvider,
            DataFormat defaultDataFormat, CommunicateContext communicateContext, CallingSettings settings, 
            Func<Sid, UserSessionState> sessionStateLoader)
        {
            Contract.Requires(service != null && serviceProvider != null
                && communicateContext != null && settings != null && sessionStateLoader != null);

            Service = service;
            Settings = settings;
            ServiceProvider = serviceProvider;
            DefaultDataFormat = defaultDataFormat;
            CommunicateContext = communicateContext;
            _sessionStateLoader = sessionStateLoader;
        }

        public AppCommandExecuteContext(AppCommandExecuteContext context)
        {
            Contract.Requires(context != null);

            Service = context.Service;
            Settings = context.Settings;
            ServiceProvider = context.ServiceProvider;
            DefaultDataFormat = context.DefaultDataFormat;
            CommunicateContext = context.CommunicateContext;
            _sessionStateLoader = context._sessionStateLoader;
        }

        /// <summary>
        /// 所属的服务
        /// </summary>
        public IAppService Service { get; private set; }

        /// <summary>
        /// 与平台的通信接口
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// 默认的编码方式
        /// </summary>
        public DataFormat DefaultDataFormat { get; private set; }

        /// <summary>
        /// 通信的上下文环境
        /// </summary>
        public CommunicateContext CommunicateContext { get; private set; }

        /// <summary>
        /// 调用设置
        /// </summary>
        public CallingSettings Settings { get; private set; }

        /// <summary>
        /// 用户的会话状态
        /// </summary>
        public UserSessionState SessionState
        {
            get
            {
                Sid sid = Settings.Sid;
                if (sid.IsEmpty())
                    return null;

                return _sessionState ?? (_sessionState = _sessionStateLoader(sid));
            }
        }

        private UserSessionState _sessionState;

        private readonly Func<Sid, UserSessionState> _sessionStateLoader;

        /// <summary>
        /// 获取当前用户的会话状态
        /// </summary>
        /// <param name="throwError">是否在出现错误时抛出异常</param>
        /// <returns></returns>
        public UserSessionState GetSessionState(bool throwError = true)
        {
            UserSessionState uss = SessionState;
            if (uss == null)
            {
                if (throwError)
                    throw new ServiceException(ServerErrorCode.InvalidUser, "用户未登录");

                return null;
            }

            if (uss.BasicInfo == null)
            {
                if (throwError)
                    throw new ServiceException(ServerErrorCode.InvalidUser, "用户不存在");

                return null;
            }

            if (!uss.BasicInfo.Enable)
            {
                if (throwError)
                    throw new ServiceException(ServerErrorCode.InvalidUser, "帐号已停用");
            }

            return uss;
        }

        /// <summary>
        /// 获取当前登录用户的ID
        /// </summary>
        /// <returns></returns>
        public int GetUserId()
        {
            return GetUserBasicInfo(throwError: true).UserId;
        }

        /// <summary>
        /// 获取当前登录用户的用户名
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return GetUserBasicInfo(throwError: true).UserName;
        }

        /// <summary>
        /// 获取当前登录用户的基础信息
        /// </summary>
        /// <param name="throwError">是否在出现错误时抛出异常</param>
        /// <returns></returns>
        public UserBasicInfo GetUserBasicInfo(bool throwError = true)
        {
            UserSessionState uss = GetSessionState(throwError);
            return uss == null ? null : uss.BasicInfo;
        }

        /// <summary>
        /// 是否来自本地调用
        /// </summary>
        public bool IsLocalCall
        {
            get { return CommunicateContext == null || CommunicateContext.From == null; }
        }
    }
}
