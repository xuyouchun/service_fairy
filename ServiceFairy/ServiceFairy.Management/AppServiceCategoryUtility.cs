using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Contracts.UIObject;

namespace ServiceFairy.Management
{
    public static class AppServiceCategoryUtility
    {
        public static string GetDesc(AppServiceCategory category)
        {
            switch (category)
            {
                case AppServiceCategory.Core:
                    return "系统运行所必须的服务";

                case AppServiceCategory.System:
                    return "提供系统组件功能的服务";

                case AppServiceCategory.Application:
                    return "与具体的业务功能相关的服务";
            }

            return "";
        }

        public static IUIObjectImageLoader CreateUIObjectImageLoader(AppServiceCategory category)
        {
            return SmUtility.CreateResourceImageLoader(category.ToString());
        }
    }
}
