using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using Common.Contracts.UIObject;
using System.IO;
using Common.Package;
using System.Xml;
using Common.Utility;
using System.Reflection;
using Common.Contracts;
using Common.Package.UIObject;
using System.Drawing;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using ServiceFairy.Management.AppClients;
using ServiceFairy.Management.AppServices;
using ServiceFairy.Entities.Master;
using Common.Communication.Wcf;

namespace ServiceFairy.Management
{
    /// <summary>
    /// 服务树
    /// </summary>
    public class SmServiceObjectTree : MarshalByRefObjectEx, IServiceObjectTree
    {
        public SmServiceObjectTree(IServiceObjectTreeNode root)
        {
            Contract.Requires(root != null);

            _root = root;
        }

        private readonly IServiceObjectTreeNode _root;

        /// <summary>
        /// 根节点
        /// </summary>
        public IServiceObjectTreeNode Root
        {
            get { return _root; }
        }

        #region Class AppServiceKindItem ...

        class AppServiceKindItem : IComparable<AppServiceKindItem>
        {
            public AppServiceKindItem(int weight, IServiceObjectTreeNode node)
            {
                Weight = weight;
                Node = node;
            }

            public int Weight { get; private set; }

            public IServiceObjectTreeNode Node { get; private set; }

            public int CompareTo(AppServiceKindItem other)
            {
                return Weight.CompareTo(other.Weight);
            }
        }

        #endregion


        /*
        /// <summary>
        /// 从指定的路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SmServiceObjectTree FromPath(string path)
        {
            Contract.Requires(path != null);

            // 从配置文件中加载
            Dictionary<AppServiceCategory, List<AppServiceKindItem>> dict = new Dictionary<AppServiceCategory, List<AppServiceKindItem>>();
            foreach (ServiceDesc serviceDesc in ServiceFairyUtility.GetAllServiceDescs(path))
            {
                AppServiceCategory category;
                int weight;
                IServiceObjectTreeNode node = _LoadFromServiceDesc(path, serviceDesc, out category, out weight);
                if (node != null)
                    dict.GetOrSet(category).Add(new AppServiceKindItem(weight, node));
            }

            IServiceObjectTreeNode rootNode = _CreateServiceObjectTreeNodeByAppServiceCategory(dict);
            return new SmServiceObjectTree(rootNode);
        } */

        private static IServiceObjectTreeNode _CreateServiceObjectTreeNodeByAppServiceCategory(Dictionary<AppServiceCategory, List<AppServiceKindItem>> dict, SfManagementContext mgrCtx = null)
        {
            // 分组
            List<IServiceObjectTreeNode> nodes = new List<IServiceObjectTreeNode>();
            foreach (AppServiceCategory category in Enum.GetValues(typeof(AppServiceCategory)).Cast<AppServiceCategory>().OrderByDescending(v => v))
            {
                List<AppServiceKindItem> list = dict.GetOrDefault(category) ?? new List<AppServiceKindItem>();
                IServiceObjectTreeNode categoryNode = new AppServiceCategoryNode(mgrCtx, category, list.OrderBy(v => v.Weight).ToArray(v => v.Node));
                ServiceObjectInfo soInfo = new ServiceObjectInfo(typeof(AppServiceCategory).Name + "_" + category, category.ToString(), category.GetDesc());

                IUIObjectImageLoader imageLoader = AppServiceCategoryUtility.CreateUIObjectImageLoader(category);
                IServiceObject so = categoryNode.ServiceObject;
                so.SetUIObject(new ServiceUIObject(so.Info, imageLoader));

                nodes.Add(categoryNode);
            }

            // 系统管理
            nodes.Add(new SystemManagementTreeNode(mgrCtx));

            // 其它
            nodes.Add(new OthersTreeNode(mgrCtx));
            
            // 创建统一的Service Object Provider
            return new RootServiceObjectNode(nodes.ToArray());
        }

        private static IUIObjectImageLoader _CreateImageLoader(string basePath, string imageFile)
        {
            string filePath;
            if (string.IsNullOrEmpty(imageFile) || !File.Exists(filePath = Path.Combine(basePath, imageFile)))
                return EmptyUIObjectImageLoader.Instance;

            try
            {
                Image img = Image.FromFile(filePath);
                return new UIObjectImageLoader(img);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return EmptyUIObjectImageLoader.Instance;
            }
        }

        private static IServiceObjectTreeNode _LoadFromServiceUIInfo(SfManagementContext mgrCtx, ServiceUIInfo serviceUIInfo)
        {
            ServiceUIInfo si = serviceUIInfo;
            ServiceObjectInfo info = new ServiceObjectInfo("Service", si.ServiceDesc.ToString(), si.Title, si.Desc, "", si.Weight);

            return new AppServiceObjectTreeNode(mgrCtx, si, info);
        }

        /// <summary>
        /// 使用指定的invoker加载
        /// </summary>
        /// <param name="mgrCtx"></param>
        /// <returns></returns>
        public static IServiceObjectTree Load(SfManagementContext mgrCtx)
        {
            Contract.Requires(mgrCtx != null);

            ServiceUIInfo[] uiInfos = mgrCtx.ServiceUIInfos.GetAll();
            Dictionary<AppServiceCategory, List<AppServiceKindItem>> dict = new Dictionary<AppServiceCategory, List<AppServiceKindItem>>();
            foreach (ServiceUIInfo uiInfo in uiInfos)
            {
                IServiceObjectTreeNode node = _LoadFromServiceUIInfo(mgrCtx, uiInfo);
                if (node != null)
                    dict.GetOrSet(uiInfo.Category).Add(new AppServiceKindItem(uiInfo.Weight, node));
            }

            IServiceObjectTreeNode rootNode = _CreateServiceObjectTreeNodeByAppServiceCategory(dict, mgrCtx);
            return new SmServiceObjectTree(rootNode);
        }
    }
}
