using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package;
using Common.Contracts;
using Common;
using Common.Utility;

namespace ServiceFairy
{
    #region Class ServiceFairyAppCommandBase ...

    public abstract class ServiceFairyAppCommandBase : AppCommandBase
    {
        /// <summary>
        /// 对请求参数进行验证
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <param name="context">上下文环境</param>
        /// <param name="request">请求参数</param>
        protected virtual void ValidateInput<TRequest>(AppCommandExecuteContext context, ref TRequest request) where TRequest : RequestEntity
        {
            if (!context.Service.Wait(AppServiceWaitType.Running, 15 * 1000))
                throw new ServiceException(ServerErrorCode.ServiceInvalid, "服务不可用");

            if (request == null)
                request = _CreateDefaultRequestEntity<TRequest>(EntityType.Request);

            request.Validate();
        }

        /// <summary>
        /// 对输出参数进行修正
        /// </summary>
        /// <typeparam name="TReply">应答参数类型</typeparam>
        /// <param name="context">上下文环境</param>
        /// <param name="reply">应答参数</param>
        protected virtual void ValidateOutput<TReply>(AppCommandExecuteContext context, ref TReply reply) where TReply : ReplyEntity
        {
            if (reply == null)
                reply = _CreateDefaultRequestEntity<TReply>(EntityType.Reply);

            reply.Validate();
        }

        enum EntityType { [Desc("请求")] Request, [Desc("应答")] Reply };

        private static T _CreateDefaultRequestEntity<T>(EntityType type) where T : Entity
        {
            try
            {
                return ObjectFactory.CreateObject<T>();
            }
            catch (Exception ex)
            {
                string errorMsg = type.GetDesc() + "参数为空，在创建默认请求参数时出现错误：" + ex.Message;
                throw new ServiceException(ServerErrorCode.ArgumentError, errorMsg);
            }
        }

        /// <summary>
        /// 创建应答参数
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sr"></param>
        /// <returns></returns>
        internal OutputAppCommandArg CreateOutputArgs(object entity, ServiceResult sr)
        {
            sr = sr ?? ServiceResult.Success;
            return new OutputAppCommandArg(entity, sr.StatusCode, sr.StatusDesc, sr.Sid);
        }

        protected virtual ServiceException CreateServiceException(string message, int statusCode)
        {
            return new ServiceException(statusCode, message);
        }

        protected virtual ServiceException CreateServiceException(string message, Enum statusCode)
        {
            return new ServiceException(statusCode, message);
        }

        protected ServiceException CreateServiceException(int statusCode)
        {
            return CreateServiceException("", statusCode);
        }

        protected ServiceException CreateServiceException(Enum statusCode)
        {
            return CreateServiceException(null, statusCode);
        }

        /// <summary>
        /// 创建仅用于描述错误信息的应答参数
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected OutputAppCommandArg CreateErrorOutputArg(Exception error)
        {
            ServiceException ex = error as ServiceException;
            int statusCode = (ex != null) ? ex.StatusCode : (int)ServiceStatusCode.ServerError;
            string statusDesc = (ex != null) ? ex.Message : ServiceStatusCode.ServerError.GetDesc();

            return new OutputAppCommandArg(null, statusCode, statusDesc);
        }
    }

    #endregion

    #region Class AppCommandBase<TRequest, TReply> ...

    /// <summary>
    /// AppCommand的泛型基类
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TReply"></typeparam>
    public abstract class AppCommandBase<TRequest, TReply> : ServiceFairyAppCommandBase
        where TRequest : RequestEntity
        where TReply : ReplyEntity
    {
        /// <summary>
        /// 执行接口
        /// </summary>
        /// <param name="context">执行环境上下文</param>
        /// <param name="arg">输入参数</param>
        /// <returns>输出参数</returns>
        protected sealed override OutputAppCommandArg OnExecute(AppCommandExecuteContext context, InputAppCommandArg arg)
        {
            try
            {
                TRequest request = (TRequest)arg.Value;
                ValidateInput(context, ref request);

                ServiceResult sr = ServiceResult.Success;
                TReply reply = OnExecute(context, request, ref sr);

                ValidateOutput(context, ref reply);
                return CreateOutputArgs(reply, sr);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return CreateErrorOutputArg(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="context">执行上下文环境</param>
        /// <param name="request">请求参数</param>
        /// <param name="sr">应答状态</param>
        /// <returns>应答参数</returns>
        protected abstract TReply OnExecute(AppCommandExecuteContext context, TRequest request, ref ServiceResult sr);

        /// <summary>
        /// 创建指令信息
        /// </summary>
        /// <returns></returns>
        protected override AppCommandInfo OnCreateInfo()
        {
            AppCommandInfo info = base.OnCreateInfo();
            return new AppCommandInfo(info.CommandDesc,
                inputParameter: new AppParameter(typeof(TRequest)), outputParameter: new AppParameter(typeof(TReply)),
                title: info.Title, desc: info.Desc, category: info.Category,
                securityLevel: info.SecurityLevel, usable: info.Usable, usableDesc: info.UsableDesc);
        }
    }

    #endregion

    #region Class AppCommandExecuteContext<TService> ...

    /// <summary>
    /// AppCommandExecuteContext的泛型形式
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class AppCommandExecuteContext<TService> : AppCommandExecuteContext where TService : IAppService
    {
        public AppCommandExecuteContext(AppCommandExecuteContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 服务
        /// </summary>
        public new TService Service
        {
            get { return (TService)base.Service; }
        }
    }

    #endregion

    #region Class AppCommandActionBase<TRequest> ...

    /// <summary>
    /// 无输出AppCommand的泛型基类
    /// </summary>
    /// <typeparam name="TRequest">输入参数类型</typeparam>
    public abstract class AppCommandActionBase<TRequest> : ServiceFairyAppCommandBase where TRequest : RequestEntity
    {
        protected sealed override OutputAppCommandArg OnExecute(AppCommandExecuteContext context, InputAppCommandArg arg)
        {
            try
            {
                ServiceResult sr = ServiceResult.Success;

                TRequest input = (TRequest)arg.Value;
                ValidateInput(context, ref input);
                OnExecute(context, input, ref sr);

                return CreateOutputArgs(null, sr);
            }
            catch (Exception ex)
            {
                return CreateErrorOutputArg(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="context">上下文环境</param>
        /// <param name="request">输入参数</param>
        /// <param name="sr">应答状态</param>
        /// <returns></returns>
        protected abstract void OnExecute(AppCommandExecuteContext context, TRequest request, ref ServiceResult sr);

        /// <summary>
        /// 创建指令信息
        /// </summary>
        /// <returns></returns>
        protected override AppCommandInfo OnCreateInfo()
        {
            AppCommandInfo info = base.OnCreateInfo();
            return new AppCommandInfo(info.CommandDesc,
                inputParameter: new AppParameter(typeof(TRequest)), outputParameter: AppParameter.Empty,
                title: info.Title, desc: info.Desc, category: info.Category, securityLevel: info.SecurityLevel,
                usable: info.Usable, usableDesc: info.UsableDesc);
        }
    }

    #endregion

    #region Class AppCommandBase ...

    /// <summary>
    /// 无输入AppCommand的泛型基类
    /// </summary>
    /// <typeparam name="TReply">应答类型</typeparam>
    public abstract class AppCommandBase<TReply> : ServiceFairyAppCommandBase where TReply : ReplyEntity
    {
        protected sealed override OutputAppCommandArg OnExecute(AppCommandExecuteContext context, InputAppCommandArg arg)
        {
            try
            {
                ServiceResult sr = ServiceResult.Success;

                TReply reply = OnExecute(context, ref sr);
                
                return CreateOutputArgs(reply, sr);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return CreateErrorOutputArg(ex);
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="context">上下文环境</param>
        /// <param name="request">输入参数</param>
        /// <param name="sr">应答状态</param>
        /// <returns>应答参数</returns>
        protected abstract TReply OnExecute(AppCommandExecuteContext context, ref ServiceResult sr);

        /// <summary>
        /// 创建指令信息
        /// </summary>
        /// <returns></returns>
        protected override AppCommandInfo OnCreateInfo()
        {
            AppCommandInfo info = base.OnCreateInfo();
            return new AppCommandInfo(info.CommandDesc,
                inputParameter: AppParameter.Empty, outputParameter: new AppParameter(typeof(TReply)),
                title: info.Title, desc: info.Desc, category: info.Category, securityLevel: info.SecurityLevel,
                usable: info.Usable, usableDesc: info.UsableDesc);
        }
    }

    #endregion

    /// <summary>
    /// 对各种泛型AppCommand基类的包装
    /// </summary>
    /// <typeparam name="TService">服务类型</typeparam>
    public class ACS<TService> where TService : IAppService
    {
        #region Class Func<TRequest, TReply> ...

        /// <summary>
        /// 有输入输出
        /// </summary>
        /// <typeparam name="TRequest">输入参数类型</typeparam>
        /// <typeparam name="TReply">输出参数类型</typeparam>
        public abstract class Func<TRequest, TReply> : AppCommandBase<TRequest, TReply>
            where TRequest : RequestEntity
            where TReply : ReplyEntity
        {
            protected sealed override TReply OnExecute(AppCommandExecuteContext context, TRequest request, ref ServiceResult sr)
            {
                return OnExecute(new AppCommandExecuteContext<TService>(context), request, ref sr);
            }

            protected abstract TReply OnExecute(AppCommandExecuteContext<TService> context, TRequest request, ref ServiceResult sr);
        }

        #endregion

        #region Class Func<TReply> ...

        /// <summary>
        /// 无输入有输出
        /// </summary>
        /// <typeparam name="TReply"></typeparam>
        public abstract class Func<TReply> : AppCommandBase<TReply>
            where TReply : ReplyEntity
        {
            protected sealed override TReply OnExecute(AppCommandExecuteContext context, ref ServiceResult sr)
            {
                return OnExecute(new AppCommandExecuteContext<TService>(context), ref sr);
            }

            /// <summary>
            /// 执行接口
            /// </summary>
            /// <param name="context">执行环境上下文</param>
            /// <param name="sr">应答状态</param>
            protected abstract TReply OnExecute(AppCommandExecuteContext<TService> context, ref ServiceResult sr);
        }

        #endregion

        #region Class Action<TRequest> ...

        /// <summary>
        /// 有输入无输出
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        public abstract class Action<TRequest> : AppCommandActionBase<TRequest>
            where TRequest : RequestEntity
        {
            protected sealed override void OnExecute(AppCommandExecuteContext context, TRequest request, ref ServiceResult sr)
            {
                OnExecute(new AppCommandExecuteContext<TService>(context), request, ref sr);
            }

            /// <summary>
            /// 执行接口
            /// </summary>
            /// <param name="context"></param>
            /// <param name="request"></param>
            /// <param name="sr"></param>
            protected abstract void OnExecute(AppCommandExecuteContext<TService> context, TRequest request, ref ServiceResult sr);
        }

        #endregion

        #region Class Action ...

        /// <summary>
        /// 无输入无输出
        /// </summary>
        public abstract class Action : ServiceFairyAppCommandBase
        {
            protected sealed override OutputAppCommandArg OnExecute(AppCommandExecuteContext context, InputAppCommandArg arg)
            {
                try
                {
                    ServiceResult sr = ServiceResult.Success;

                    OnExecute(new AppCommandExecuteContext<TService>(context), ref sr);

                    return CreateOutputArgs(null, sr);
                }
                catch (Exception ex)
                {
                    return CreateErrorOutputArg(ex);
                }
            }

            /// <summary>
            /// 执行接口
            /// </summary>
            /// <param name="context">执行环境上下文</param>
            /// <param name="sr">应答状态</param>
            protected abstract void OnExecute(AppCommandExecuteContext<TService> context, ref ServiceResult sr);
        }

        #endregion
    }
}
