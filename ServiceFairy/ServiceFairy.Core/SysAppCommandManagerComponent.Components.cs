using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Sys;
using Common.Utility;
using Common.Contracts;
using Common;
using Common.Package;
using System.Reflection;

namespace ServiceFairy
{
    partial class SysAppCommandManagerComponent
    {
        /// <summary>
        /// 获取组件的属性值
        /// </summary>
        [AppCommand("Sys_GetAppComponentPropertyValues", "获取组件的属性值", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class GetAppComponentPropertyValuesAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppComponentPropertyValues_Request, Sys_GetAppComponentPropertyValues_Reply>
        {
            public GetAppComponentPropertyValuesAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppComponentPropertyValues_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, Sys_GetAppComponentPropertyValues_Request req, ref ServiceResult sr)
            {
                IAppComponent[] components = ((TrayAppServiceBaseEx)context.Service).AppComponentManager.GetAll();
                var list = from component in components
                           let info = component.GetInfo()
                           where req.ComponentNames == null || req.ComponentNames.Contains(info.Name)
                           select _GetComponetProperties(component, req.Expressions);

                AppComponentPropertyValueGroup[] groups = list.SelectMany().ToArray();
                return new Sys_GetAppComponentPropertyValues_Reply() { ValueGroups = groups };
            }

            private AppComponentPropertyValue[] _GetPropertyValues(IObjectPropertyProvider objPropertyProvider)
            {
                return objPropertyProvider.GetAllProperties().Select(p => objPropertyProvider.GetPropertyValue(p.Name))
                            .ToArray(v => AppComponentPropertyValue.From(v));
            }

            private AppComponentPropertyValueGroup[] _GetComponetProperties(IAppComponent component, string[] expressions)
            {
                AppComponentInfo cInfo = component.GetInfo();
                List<AppComponentPropertyValueGroup> list = new List<AppComponentPropertyValueGroup>();
                if (expressions == null)
                {
                    list.Add(new AppComponentPropertyValueGroup {
                        ComponentName = cInfo.Name,
                        Expression = null,
                        Values = _GetPropertyValues(component),
                    });
                }
                else
                {
                    foreach (string expression in expressions.Where(exp => !string.IsNullOrWhiteSpace(exp)).Distinct())
                    {
                        AppComponentPropertyValueGroup g = _GetPropertyValueGroup(component, expression);
                        if (!g.Values.IsNullOrEmpty())
                            list.Add(g);
                    }
                }

                return list.ToArray();
            }

            private AppComponentPropertyValueGroup _GetPropertyValueGroup(IAppComponent component, string expression)
            {
                AppComponentInfo info = component.GetInfo();
                string[] exps = expression.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (exps.Length == 0)
                    goto _empty;

                object obj = component;
                foreach (string exp in exps)
                {
                    obj = _TryGetProperty(obj, exp);
                    if (obj == null)
                        goto _empty;
                }

                IObjectPropertyProvider provider = (obj as IObjectPropertyProvider) ?? new ObjectPropertyLoader(obj);
                return new AppComponentPropertyValueGroup {
                    ComponentName = info.Name, Expression = expression,
                    Values = _GetPropertyValues(provider),
                };

            _empty:
                return new AppComponentPropertyValueGroup { ComponentName = info.Name, Values = Array<AppComponentPropertyValue>.Empty };
            }

            private object _TryGetProperty(object obj, string name)
            {
                if (obj == null)
                    return null;

                try
                {
                    IObjectPropertyProvider p = obj as IObjectPropertyProvider;
                    if (p != null)
                    {
                        ObjectPropertyValue pv = p.GetPropertyValue(name);
                        return pv == null ? null : pv.RawValue;
                    }
                    else
                    {
                        int i = name.IndexOf('['), j = name.IndexOf(']', i + 1);
                        if (i >= 0 && j >= 0)  // 有参数
                        {
                            return null;  // 暂不支持
                        }
                        else
                        {
                            PropertyInfo pInfo = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                            if (pInfo == null)
                                return null;

                            return pInfo.GetValue(obj, null);
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 改变组件属性的值
        /// </summary>
        [AppCommand("Sys_SetAppComponentPropertyValues", "改变组件的属性值", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class SetAppComponentPropertyValuesAppCommand : ACS<TrayAppServiceBaseEx>.Action<Sys_SetAppComponentPropertyValues_Request>
        {
            public SetAppComponentPropertyValuesAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override void OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, Sys_SetAppComponentPropertyValues_Request req, ref ServiceResult sr)
            {
                AppComponentManager mgr = ((TrayAppServiceBaseEx)context.Service).AppComponentManager;
                foreach (AppComponentPropertyValueGroup group in req.ValueGroups)
                {
                    string componentName = group.ComponentName;
                    IAppComponent component = mgr.Get(componentName);
                    //component.
                }
            }
        }

        /// <summary>
        /// 获取所有的组件
        /// </summary>
        [AppCommand("Sys_GetAppComponentInfos", "获取所有组件的信息", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class GetAppComponentInfosAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppComponentInfos_Reply>
        {
            public GetAppComponentInfosAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppComponentInfos_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, ref ServiceResult sr)
            {
                IAppComponent[] components = ((TrayAppServiceBaseEx)context.Service).AppComponentManager.GetAll();
                return new Sys_GetAppComponentInfos_Reply() {
                    AppComponentInfos = components.ToArray(component => component.GetInfo())
                };
            }
        }

        /// <summary>
        /// 获取组件的所有属性
        /// </summary>
        [AppCommand("Sys_GetAppComponentProperties", "获取组件的所有属性", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class GetAppComponentPropertiesAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppComponentProperties_Request, Sys_GetAppComponentProperties_Reply>
        {
            public GetAppComponentPropertiesAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppComponentProperties_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, Sys_GetAppComponentProperties_Request req, ref ServiceResult sr)
            {
                IAppComponent[] components = ((TrayAppServiceBaseEx)context.Service).AppComponentManager.GetAll();
                var list = from component in components
                           let info = component.GetInfo()
                           where req.ComponentNames == null || req.ComponentNames.Contains(info.Name)
                           select new ObjectPropertyGroup() {
                               ComponentName = info.Name,
                               Properties = component.GetAllProperties()
                           };

                ObjectPropertyGroup[] groups = list.ToArray();
                return new Sys_GetAppComponentProperties_Reply() { PropertyGroups = groups };
            }
        }
    }
}
