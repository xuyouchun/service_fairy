using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.IO;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Contracts.UIObject;
using Common.Package.UIObject;
using Common.Contracts;
using Common.Package;
using Common.WinForm;
using System.Windows.Forms;
using System.Threading;

namespace ServiceFairy.Management
{
    public static class SmUtility
    {
        /// <summary>
        /// 获取UI程序集的路径
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static string GetUIAssemblyPath(ServiceDesc serviceDesc, string basePath)
        {
            Contract.Requires(serviceDesc != null);

            basePath = _GetFullBasePath(basePath);
            return PathUtility.Revise(Path.Combine(basePath, serviceDesc.Name, serviceDesc.Version.ToString(), "Main.UI.dll"));
        }

        /// <summary>
        /// 获取UI程序集的配置文件路径
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static string GetUIConfigPath(ServiceDesc serviceDesc, string basePath)
        {
            Contract.Requires(serviceDesc != null);

            basePath = _GetFullBasePath(basePath);
            return PathUtility.Revise(Path.Combine(basePath, serviceDesc.Name, serviceDesc.Version.ToString(), SFSettings.UIAssemblyConfigFile));
        }

        private static string _GetFullBasePath(string basePath)
        {
            if (string.IsNullOrEmpty(basePath))
                return AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, basePath);
        }

        private static bool _Include(ServiceDesc sd, string[] sdNames)
        {
            return sdNames.Contains(sd.Name);
        }

        /// <summary>
        /// 该服务是否可以停止
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static bool CanStop(this ServiceDesc sd)
        {
            Contract.Requires(sd != null);
            return !_Include(sd, new string[] { SFNames.ServiceNames.Tray, SFNames.ServiceNames.Master });
        }

        /// <summary>
        /// 该服务是否可以手动启动
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static bool CanStart(this ServiceDesc sd)
        {
            Contract.Requires(sd != null);
            return !_Include(sd, new string[] { SFNames.ServiceNames.Tray, SFNames.ServiceNames.Master });
        }

        /// <summary>
        /// 该服务是否可以重启
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static bool CanRestart(this ServiceDesc sd)
        {
            Contract.Requires(sd != null);
            return !_Include(sd, new string[] { SFNames.ServiceNames.Tray, SFNames.ServiceNames.Master });
        }

        /// <summary>
        /// 将终端标识转换为字符串
        /// </summary>
        /// <param name="ep"></param>
        /// <param name="mgrCtx"></param>
        /// <returns></returns>
        public static string ToString(this ServiceEndPoint ep, SfManagementContext mgrCtx)
        {
            if (ep == null)
                return string.Empty;

            if (mgrCtx == null)
                return ep.ToString();

            string clientTitle = mgrCtx.ClientDescs.GetClientTitle(ep.ClientId);
            if (string.IsNullOrEmpty(clientTitle))
                return ep.ToString();

            return clientTitle + "/" + ep.ServiceDesc;
        }

        /// <summary>
        /// 将时间转化为字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetString(this DateTime dt)
        {
            if (dt.IsEmpty())
                return string.Empty;

            return dt.ToString();
        }

        /// <summary>
        /// 将时间间隔转化为字符串
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string GetString(this TimeSpan ts)
        {
            if (ts.IsEmpty())
                return string.Empty;

            string timeStr = ts.Hours.ToString().PadLeft(2, '0') + ":"
                + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');

            int days = ts.Days;
            if (days > 0)
                timeStr = days + " " + timeStr;

            return timeStr;
        }

        /// <summary>
        /// 获取从某个时间直到现在所经历的时间，并转化为字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetUntilNowString(this DateTime dt)
        {
            if (dt.IsEmpty())
                return string.Empty;

            return GetString(DateTime.Now - dt);
        }

        /// <summary>
        /// 获取转换为本地时间的字符串形式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetLocalTimeString(this DateTime dt)
        {
            if (dt.IsEmpty())
                return string.Empty;

            return dt.ToLocalTime().GetString();
        }

        /// <summary>
        /// 获取从某个时间直到现在的所经历的时间，并转化为字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetUtcUntilNowString(this DateTime dt)
        {
            if (dt.IsEmpty())
                return string.Empty;

            return GetString(DateTime.UtcNow - dt);
        }

        /// <summary>
        /// 创建基于资源的UIObjectImageLoader
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static ResourceUIObjectImageLoader CreateResourceImageLoader(string resourceName)
        {
            return _resImgLoaderDict.GetOrSet(resourceName, (key) =>
                ResourceUIObjectImageLoader.Load(typeof(SmUtility), resourceName)
            );
        }

        private static readonly Dictionary<string, ResourceUIObjectImageLoader> _resImgLoaderDict
            = new Dictionary<string, ResourceUIObjectImageLoader>();

        /// <summary>
        /// 带有图标的选择对话框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiOperation"></param>
        /// <param name="items"></param>
        /// <param name="title"></param>
        /// <param name="imageLoader"></param>
        /// <param name="selectionMode"></param>
        /// <returns></returns>
        public static T[] SelectWithImage<T>(this IUIOperation uiOperation, IEnumerable<T> items, string title, IUIObjectImageLoader imageLoader, SelectionMode selectionMode = SelectionMode.MultiExtended)
        {
            Contract.Requires(uiOperation != null && items != null);

            if (imageLoader == null)
                return uiOperation.Select(items.ToArray(), title, selectionMode);

            var imgItems = UIObjectImageLoaderAdapter<T>.Convert(items, imageLoader);
            var selectedItems = uiOperation.Select(imgItems, title, selectionMode);
            if (selectedItems == null)
                return null;

            return selectedItems.ToArray(si => si.Obj);
        }

        /// <summary>
        /// 带有图标的选择对话框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uiOperation"></param>
        /// <param name="items"></param>
        /// <param name="title"></param>
        /// <param name="imageResourceName"></param>
        /// <param name="selectionMode"></param>
        /// <returns></returns>
        public static T[] SelectWithImage<T>(this IUIOperation uiOperation, IEnumerable<T> items, string title, string imageResourceName, SelectionMode selectionMode = SelectionMode.MultiExtended)
        {
            Contract.Requires(items != null);
            return SelectWithImage<T>(uiOperation, items.ToArray(), title, CreateResourceImageLoader(imageResourceName));
        }

        public static string GetFileCategory(string filename)
        {
            switch (Path.GetFileName(filename).ToLower())
            {
                case "main.dll":
                    return "主程序集";

                case "main.pdb":
                    return "主程序集调试信息";

                case "main.ico":
                    return "服务图标";

                case "main.dll.config":
                    return "主程序集配置文件";

                case "main.ui.dll":
                    return "服务管理界面程序集";

                case "main.ui.pdb":
                    return "服务管理界面程序集调试信息";

                case "main.ui.dll.config":
                    return "服务管理界面程序集配置";
            }

            string ext = Path.GetExtension(filename).Trim('.');
            switch (ext.ToLower())
            {
                case "dll":
                    return "程序集";

                case "exe":
                    return "可执行文件";

                case "pdb":
                    return "程序集调试信息";

                case "txt":
                    return "文本文件";

                case "xml":
                    return "XML文件";

                case "ico":
                    return "图标文件";

                case "jpg":
                case "gif":
                case "pdf":
                case "bmp":
                    return "图片文件";

                case "config":
                    return "配置文件";

                case "deploypackageinfo":
                    return "安装包信息";

                case "deploypackage":
                    return "安装包";
            }

            return ext + "文件";
        }
    }
}
