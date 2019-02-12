using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using Common.Framework.TrayPlatform;
using Common.Utility;
using Common.Contracts.Service;
using Common;
using Common.WinForm;
using System.Windows.Forms;
using ServiceFairy.WinForm;
using ServiceFairy.Install;
using Common.Communication.Wcf;

namespace ServiceFairy.TrayManagement
{
    static class Utility
    {
        public static string GetServiceStatusDesc(ServiceControllerStatus status)
        {
            switch (status)
            {
                case ServiceControllerStatus.ContinuePending:
                    return "正在继续 ...";

                case ServiceControllerStatus.PausePending:
                    return "正在暂停 ...";

                case ServiceControllerStatus.Paused:
                    return "已暂停";

                case ServiceControllerStatus.Running:
                    return "正在运行 ...";

                case ServiceControllerStatus.StartPending:
                    return "正在启动 ...";

                case ServiceControllerStatus.StopPending:
                    return "正在停止 ...";

                case ServiceControllerStatus.Stopped:
                    return "已停止";

                case ServiceControllerStatusError:
                    return "出现错误";

                default:
                    return "";
            }
        }

        public const ServiceControllerStatus ServiceControllerStatusError = (ServiceControllerStatus)int.MaxValue;
        public const ServiceControllerStatus ServiceControllerUnknown = (ServiceControllerStatus)(int.MaxValue - 1);

        private static readonly char[] _separator = new char[] { ',', ';' };

        public static string[] Split(string s)
        {
            if (s == null)
                return Array<string>.Empty;

            return s.Trim(_separator).Split(_separator, StringSplitOptions.RemoveEmptyEntries).SelectFromList(s0 => s0.Trim());
        }

        /// <summary>
        /// 转换为CommunicationOption实体
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns></returns>
        public static CommunicationOption[] ToCommunicationOption(string addresses)
        {
            return Split(addresses).SelectFromList(address => CommunicationOption.Parse(address));
        }

        /// <summary>
        /// 转换为ServiceDesc实体
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ServiceDesc[] ToServiceDescs(string services)
        {
            return Split(services).SelectFromList(service => ServiceDesc.Parse(service));
        }

        /// <summary>
        /// 编辑中心服务器地址
        /// </summary>
        /// <param name="window"></param>
        /// <param name="masterAddresses"></param>
        /// <param name="running"></param>
        /// <returns></returns>
        public static string EditMasterAddress(IWin32Window window, string masterAddresses, bool running)
        {
            CommunicationOption[] addresses = Split(masterAddresses).SelectFromList(s => CommunicationOption.Parse(s));

            if (running)
            {
                addresses = CollectionEditDialog.Show<CommunicationOption>(window, addresses, "编辑中心服务器地址",
                    _EditCommunication, _RemoveCommunication, _EditCommunication, state: true);
            }
            else
            {
                addresses = CollectionEditDialog.Show<CommunicationOption>(window, addresses, "编辑中心服务器地址");
            }

            if (addresses != null)
                return string.Join(";", addresses.CastAsString());

            return null;
        }

        /// <summary>
        /// 编辑要开启的端口
        /// </summary>
        /// <param name="window"></param>
        /// <param name="communications"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        public static string EditCommunicationToOpen(IWin32Window window, string communications, bool editable)
        {
            CommunicationOption[] communicationOptions = Split(communications).SelectFromList(s => CommunicationOption.Parse(s));

            if (editable)
            {
                communicationOptions = CollectionEditDialog.Show<CommunicationOption>(window, communicationOptions, "信道",
                    _EditCommunication, _RemoveCommunication, _EditCommunication);
            }
            else
            {
                communicationOptions = CollectionEditDialog.Show<CommunicationOption>(window, communicationOptions, "信道");
            }

            if (communicationOptions != null)
                return string.Join(";", (object[])communicationOptions);

            return null;
        }

        private static bool _RemoveCommunication(CollectionEditDialogContext<CommunicationOption> ctx)
        {
            return true;
        }

        private static CommunicationOption _EditCommunication(CollectionEditDialogContext<CommunicationOption> ctx)
        {
        _start:
            CommunicationDialog dlg = new CommunicationDialog(true, ctx.State is bool ? (bool)ctx.State : false);
            try
            {
                dlg.CommunicationOption = ctx.FocusedItem;
                if (dlg.ShowDialog(ctx.Window) == DialogResult.OK)
                {
                    CommunicationOption op = dlg.CommunicationOption;
                    if (ctx.Contains(op))
                    {
                        ctx.Window.ShowError("该地址已经存在");
                        goto _start;
                    }

                    return op;
                }
            }
            catch (Exception ex)
            {
                ctx.Window.ShowError(ex);
            }

            return null;
        }

        /// <summary>
        /// 编辑服务
        /// </summary>
        /// <param name="window"></param>
        /// <param name="services"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        public static string EditServices(IWin32Window window, string services, TraySystemInvoker traySystemInvoker, bool editable)
        {
            ServiceDesc[] sas = Split(services).SelectFromList(s => ServiceDesc.Parse(s));
            ServiceDesc[] sds;

            if (editable)
            {
                sds = CollectionEditDialog.Show<ServiceDesc>(window, sas, "服务",
                    _EditServiceDesc, _RemoveServiceDesc, _EditServiceDesc, state: traySystemInvoker);
            }
            else
            {
                sds = CollectionEditDialog.Show<ServiceDesc>(window, sas, "服务", state: traySystemInvoker);
            }

            if (sds == null)
                return null;

            return string.Join(",", (object[])sds);
        }

        private static bool _RemoveServiceDesc(CollectionEditDialogContext<ServiceDesc> ctx)
        {
            return true;
        }

        private static ServiceDesc _EditServiceDesc(CollectionEditDialogContext<ServiceDesc> ctx)
        {
            _start:

            try
            {
                string sdTxt = DropdownSelectDialog.Show(ctx.Window, delegate() {
                    return _GetAllServices(ctx.State as TraySystemInvoker).SelectFromList(s => s.ToString());
                }, null, "请选择要启动的服务", editable: true);

                if (string.IsNullOrWhiteSpace(sdTxt))
                    return null;

                ServiceDesc sd = ServiceDesc.Parse(sdTxt);
                if (ctx.Contains(sd))
                {
                    ctx.Window.ShowError("该服务已经存在");
                    goto _start;
                }

                return sd;
            }
            catch (Exception ex)
            {
                ctx.Window.ShowError(ex);
            }

            return null;
        }

        private static ServiceDesc[] _GetAllServices(TraySystemInvoker traySystemInvoker)
        {
            if (traySystemInvoker != null)
            {
                try
                {
                    ServiceDesc[] allServiceDescs = traySystemInvoker.GetAllServiceDescs();
                    if (allServiceDescs != null)
                        return allServiceDescs;
                }
                catch { }
            }

            return InstallUtility.GetAllServiceDescsFromInstallPath();
        }
    }
}
