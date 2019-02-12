using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics.Contracts;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.Utility;
using System.IO;

namespace Common.Contracts
{
    public static class ContractUtility
    {
        /// <summary>
        /// 执行默认的动作
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <param name="executeContext"></param>
        public static void ExecuteDefaultAction(this IServiceObject serviceObject, IUIObjectExecuteContext executeContext)
        {
            ExecuteAction(serviceObject, executeContext, ServiceObjectActionType.Default);
        }

        /// <summary>
        /// 执行指定类型的动作
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <param name="executeContext"></param>
        /// <param name="actionType"></param>
        public static void ExecuteAction(this IServiceObject serviceObject, IUIObjectExecuteContext executeContext, ServiceObjectActionType actionType)
        {
            Contract.Requires(serviceObject != null && executeContext != null);

            ServiceObjectAction action = serviceObject.GetActions(actionType).FirstOrDefault();
            IActionUIObject uiObj;
            if (action != null && (uiObj = GetUIObject(action) as IActionUIObject) != null)
                uiObj.Execute(executeContext, serviceObject);
        }

        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Get<T>(this IServiceObjectEntity entity) where T : class
        {
            Contract.Requires(entity != null);

            return (T)entity.Items.GetOrDefault(typeof(T));
        }

        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="obj"></param>
        public static void Set<T>(this IServiceObjectEntity entity, T obj) where T : class
        {
            Contract.Requires(entity != null);

            entity.Items[typeof(T)] = obj;
        }

        /// <summary>
        /// 获取UIObject
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IUIObject GetUIObject(this IServiceObjectEntity entity)
        {
            return Get<IUIObject>(entity);
        }

        /// <summary>
        /// 设置UIObject
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="uiObject"></param>
        public static void SetUIObject(this IServiceObjectEntity entity, IUIObject uiObject)
        {
            Set<IUIObject>(entity, uiObject);
        }

        /// <summary>
        /// 获取ServiceObjectTreeNode
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IServiceObjectTreeNode GetTreeNode(this IServiceObjectEntity entity)
        {
            return Get<IServiceObjectTreeNode>(entity);
        }

        /// <summary>
        /// 设置ServiceObjectTreeNode
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="treeNode"></param>
        public static void SetTreeNode(this IServiceObjectEntity entity, IServiceObjectTreeNode treeNode)
        {
            Set<IServiceObjectTreeNode>(entity, treeNode);
        }

        /// <summary>
        /// 获取指定尺寸的图片
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image GetImage(this IUIObjectImageLoader loader, int size)
        {
            Contract.Requires(loader != null);

            return (Image)loader.GetImage(new Size(size, size));
        }

        public static Image GetImageOrTransparent(this IUIObjectImageLoader loader, Size size)
        {
            Contract.Requires(loader != null);

            return (Image)(loader.GetImage(size) ?? TransparentUIObjectImageLoader.Instance.GetImage(size));
        }

        public static Image GetImageOrTransparent(this IUIObjectImageLoader loader, int size)
        {
            return (Image)(GetImageOrTransparent(loader, new Size(size, size)));
        }

        /// <summary>
        /// 获取ServiceObjectInfo的标题
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetTitle(this ServiceObjectInfo info)
        {
            Contract.Requires(info != null);

            return StringUtility.GetFirstNotNullOrWhiteSpaceString(info.Title, info.Name);
        }

        /// <summary>
        /// 是否为成功的状态码
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static bool IsSuccess(int statusCode)
        {
            return GetServiceStatusCodeCategory(statusCode) == ServiceStatusCode.Ok;
        }

        /// <summary>
        /// 获取状态码的类别
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static ServiceStatusCode GetServiceStatusCodeCategory(int statusCode)
        {
            return (ServiceStatusCode)(statusCode & 0xC0000000);
        }

        /// <summary>
        /// 获取状态码的服务号
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static int GetServiceCode(int statusCode)
        {
            return (statusCode & 0x3FFFFFFF) >> 16;
        }

        /// <summary>
        /// 获取状态码的内部编号
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static int GetInnerStatusCode(int statusCode)
        {
            return statusCode & 0x0000FFFF;
        }

        /// <summary>
        /// 注册数据接收器
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="serviceClient">服务终端</param>
        /// <param name="method">方法</param>
        /// <param name="receiver">接收器</param>
        /// <returns></returns>
        public static IServiceClientReceiverHandler RegisterReceiver<TEntity>(this IServiceClient serviceClient,
            string method, IServiceClientReceiver<TEntity> receiver)
        {
            Contract.Requires(serviceClient != null && receiver != null);

            return serviceClient.RegisterReceiver(method, typeof(TEntity), new ServiceClientReceiverAdapter<TEntity>(receiver));
        }

        /// <summary>
        /// 通过服务终端调用指定的方法
        /// </summary>
        /// <typeparam name="TOuput">应答数据类型</typeparam>
        /// <param name="serviceClient">服务终端</param>
        /// <param name="method">方法</param>
        /// <param name="input">输入参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns>应答参数</returns>
        public static ServiceResult<TOutput> Call<TOutput>(this IServiceClient serviceClient, string method, object input, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            ServiceResult<object> sr = serviceClient.Call(method, input, typeof(TOutput), settings);
            if (sr == null)
                return null;

            return new ServiceResult<TOutput>((TOutput)sr.Result, sr.StatusCode, sr.StatusDesc, sr.Sid);
        }

        /// <summary>
        /// 通过服务终端调用指定的方法
        /// </summary>
        /// <typeparam name="TOutput">应答数据类型</typeparam>
        /// <param name="serviceClient">服务终端</param>
        /// <param name="method">方法</param>
        /// <param name="settings">调用设置</param>
        /// <returns>应答参数</returns>
        public static ServiceResult<TOutput> Call<TOutput>(this IServiceClient serviceClient, string method, CallingSettings settings = null)
        {
            return Call<TOutput>(serviceClient, method, (object)null, settings);
        }

        /// <summary>
        /// 通过服务终端调用指定的方法
        /// </summary>
        /// <param name="serviceClient">服务终端</param>
        /// <param name="method">方法</param>
        /// <param name="settings">调用设置</param>
        /// <returns>应答参数</returns>
        public static ServiceResult Call(this IServiceClient serviceClient, string method, CallingSettings settings = null)
        {
            return serviceClient.Call(method, (object)null, settings);
        }

        /// <summary>
        /// 将安全码写入流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sid"></param>
        public static void Write(this Stream stream, Sid sid)
        {
            Contract.Requires(stream != null);
            stream.Write(sid.ToBytes());
        }

        /// <summary>
        /// 从流中读取安全码
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Sid ReadSid(this Stream stream)
        {
            Contract.Requires(stream != null);
            byte[] bytes = stream.ReadBytes(16);
            return Sid.FromBytes(bytes);
        }

        /// <summary>
        /// 获取安全级别的描述
        /// </summary>
        /// <param name="level">安全级别</param>
        /// <returns>描述信息</returns>
        public static string GetSecurityLevelDesc(this SecurityLevel level)
        {
            string s = ((int)level).ToString(), desc = level.GetDesc();
            if (!string.IsNullOrEmpty(desc))
                s += "(" + desc + ")";

            return s;
        }
    }
}
