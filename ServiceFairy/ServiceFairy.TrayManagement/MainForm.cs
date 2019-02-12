using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using ServiceFairy.Install;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.WinForm;
using Common.Utility;
using System.Threading;
using System.IO;
using ServiceFairy.SystemInvoke;
using Common.Framework.TrayPlatform;
using ServiceFairy.WinForm;
using Common.Communication.Wcf;

namespace ServiceFairy.TrayManagement
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _modelRadioButtons = new RunningModelRadioButtonManager(_ctlModelGroupBox);
            _modelRadioButtons.CurrentChanged += new EventHandler(_modelRadioButtons_CurrentChanged);
            _modelRadioButtons.ClickWhenReadonly += new EventHandler(_modelRadioButtons_ClickWhenReadonly);
            _sc = new ServiceController(Settings.ServiceName);

            _refreshStatusTimerHandle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(0), new TaskFuncAdapter(_RefreshServiceStatus), false, false);
            _freqRefreshStatusTimerHandle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromMilliseconds(500), new TaskFuncAdapter(_RefreshServiceStatus), false, false);

            _cfgMgr = new ConfigurationManager(this, Path.Combine(PathUtility.GetExecutePath(), "Configuration.xml"));
            _modelRadioButtons.Value = _cfgMgr.Configuration.RunningModel;
            _ResetSystemInvoker();
        }

        private void _modelRadioButtons_ClickWhenReadonly(object sender, EventArgs e)
        {
            this.ShowError("在运行状态下，无法更改运行模式");
        }

        private readonly RunningModelRadioButtonManager _modelRadioButtons;
        private readonly ServiceController _sc;
        private readonly ConfigurationManager _cfgMgr;
        private readonly IGlobalTimerTaskHandle _refreshStatusTimerHandle, _freqRefreshStatusTimerHandle;
        private TraySystemInvoker _traySystemInvoker;

        private void _ResetSystemInvoker()
        {
            if (_traySystemInvoker != null)
                _traySystemInvoker.Dispose();

            string masterAddresses = _cfgMgr.Configuration.MasterCommunications;
            CommunicationOption op = TrayUtility.PickCommunicationOption(
                Utility.Split(masterAddresses).SelectFromList(sa => CommunicationOption.Parse(sa))
            );

            if (op == null)
                _traySystemInvoker = null;
            else
                _traySystemInvoker = new TraySystemInvoker(op, _cfgMgr.Configuration.ClientID);
        }

        private void _modelRadioButtons_CurrentChanged(object sender, EventArgs e)
        {
            _cfgMgr.Configuration.RunningModel = _modelRadioButtons.Value;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _RefreshServiceStatus();
            _refreshStatusTimerHandle.Start();
        }

        private void _DoChangeStateAction(Action action)
        {
            _cfgMgr.Save();
            _SetServiceDisableStatus();

            Exception error = null;

            try
            {
                _freqRefreshStatusTimerHandle.Start();

                action();
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                _freqRefreshStatusTimerHandle.Stop();
                _RefreshServiceStatus();
            }

            if (error != null)
                ErrorDialog.Show(this, error);
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tsStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_cfgMgr.Configuration.MasterCommunications))
            {
                this.ShowError("尚未设置中心服务器地址");
                return;
            }

            _DoChangeStateAction(delegate {
                _sc.Start();
                _sc.WaitForStatus(ServiceControllerStatus.Running);
            });
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tsStop_Click(object sender, EventArgs e)
        {
            if (!this.ShowQuestion("确认要停止该服务吗？"))
                return;

            _DoChangeStateAction(delegate {
                _sc.Stop();
                _sc.WaitForStatus(ServiceControllerStatus.Stopped);
            });
        }

        /// <summary>
        /// 正在重新启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tsRestart_Click(object sender, EventArgs e)
        {
            if (!this.ShowQuestion("确认要重启该服务吗？"))
                return;

            _DoChangeStateAction(delegate {
                _sc.Stop();
                _sc.WaitForStatus(ServiceControllerStatus.Stopped);
                _sc.Start();
                _sc.WaitForStatus(ServiceControllerStatus.Running);
            });
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            
        }

        private void _RefreshServiceStatus()
        {
            ServiceControllerStatus status = Utility.ServiceControllerUnknown;
            string message = null;
            RunningInfo runningInfo = null;

            try
            {
                _sc.Refresh();
                status = _sc.Status;
                runningInfo = _GetRunningInfo();
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }

            this.Invoke(delegate {
                _SetServiceStatus(status, message, runningInfo);
            });
        }

        private void _SetServiceDisableStatus()
        {
            _SetServiceStatus((ServiceControllerStatus)(-1));
        }

        private RunningInfo _GetRunningInfo()
        {
            RunningConfiguration cfg = _cfgMgr.Configuration;
            RunningInfo rInfo = new RunningInfo();

            if (_traySystemInvoker == null)
                return new RunningInfo() { Communications = "", ServiceDescs = "" };

            if (_IsRunning())
            {
                ServiceDesc[] serviceDescs = _traySystemInvoker.GetRunningServices();
                CommunicationOption[] communications = _traySystemInvoker.GetOpenedCommunicationOptions();
                rInfo.ServiceDescs = serviceDescs == null ? "正在获取 ..." : serviceDescs.Length == 0 ? "（无）" : string.Join(", ", (object[])serviceDescs);
                rInfo.Communications = communications == null ? "正在获取 ..." : communications.Length == 0 ? "（无）" : string.Join(", ", (object[])communications);
            }
            else
            {
                rInfo.ServiceDescs = string.IsNullOrWhiteSpace(cfg.ServicesToStart) ? "（无）" : cfg.ServicesToStart;
                rInfo.Communications = string.IsNullOrWhiteSpace(cfg.CommunicationsToOpen) ? "（无）" : cfg.CommunicationsToOpen;
            }

            return rInfo;
        }

        class RunningInfo
        {
            public string ServiceDescs { get; set; }

            public string Communications { get; set; }
        }

        private void _SetServiceStatus(ServiceControllerStatus status, string message = null, RunningInfo runningInfo = null)
        {
            try
            {
                this.SuspendLayout();

                string statusMsg = Utility.GetServiceStatusDesc(status);
                _tsServiceStatus.Text = statusMsg;
                if (!string.IsNullOrEmpty(message))
                {
                    _balloonTip.SetBalloonText(_tsStrip, message);
                    //_balloonTip.ShowBalloon(_tsStrip);
                }
                else
                {
                    _balloonTip.Remove(_tsStrip);
                }

                if (status == ServiceControllerStatus.Running)
                {
                    _tsStart.Enabled = false;
                    _tsStop.Enabled = true;
                    _tsRestart.Enabled = true;
                }
                else if (status == ServiceControllerStatus.Stopped)
                {
                    _tsStart.Enabled = true;
                    _tsStop.Enabled = false;
                    _tsRestart.Enabled = false;
                }
                else
                {
                    _tsStart.Enabled = false;
                    _tsStop.Enabled = false;
                    _tsRestart.Enabled = false;
                }


                bool running = (status == ServiceControllerStatus.Running);
                _modelRadioButtons.Readonly = running;
                //_ctlMasterAddress.Enabled = _ctlRunningServices.Enabled = _ctlCommunicationOption.Enabled = !running;

                if (!running)
                {
                    _ctlCommunicationLabel.Text = "开启端口：";
                    _ctlRunningServicesLabel.Text = "启动服务：";
                }
                else
                {
                    _ctlCommunicationLabel.Text = "已开启的端口：";
                    _ctlRunningServicesLabel.Text = "运行中的服务：";
                }

                RunningConfiguration cfg = _cfgMgr.Configuration;
                _ctlMasterAddress.Text = string.IsNullOrWhiteSpace(cfg.MasterCommunications) ? "（未设置）" : cfg.MasterCommunications;
                _ctlRunningServices.Text = runningInfo == null ? "?" : runningInfo.ServiceDescs;
                _ctlCommunicationOption.Text = runningInfo == null ? "?" : runningInfo.Communications;

                _ctlModibyName.Text = string.Join(" ", new object[] {
                    StringUtility.GetFirstNotNullOrWhiteSpaceString( cfg.ClientTitle, NetworkUtility.GetHostName()),
                    cfg.ClientDesc
                });

                _ctlModibyName.Left = _ctlModibyName.Parent.Width - _ctlModibyName.Width - 15;
            }
            finally
            {
                this.ResumeLayout(false);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _refreshStatusTimerHandle.Dispose();
            _freqRefreshStatusTimerHandle.Dispose();

            if (_traySystemInvoker != null)
                _traySystemInvoker.Dispose();
        }

        private bool _IsRunning()
        {
            return _sc.Status == ServiceControllerStatus.Running;
        }

        // 中心服务器地址
        private void _ctlMasterAddress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (_IsRunning())
                {
                    Utility.EditMasterAddress(this, _cfgMgr.Configuration.MasterCommunications, false);
                }
                else
                {
                    string masterAddress = Utility.EditMasterAddress(this, _cfgMgr.Configuration.MasterCommunications, true);
                    if (masterAddress != null)
                    {
                        _cfgMgr.Configuration.MasterCommunications = masterAddress;
                        _RefreshServiceStatus();
                        _ResetSystemInvoker();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cfgMgr.Save();
        }

        // 选择要开启的端口
        private void _ctlCommunicationOption_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (_IsRunning())
                {
                    RunningInfo rInfo = _GetRunningInfo();
                    if (rInfo != null)
                    {
                        Utility.EditCommunicationToOpen(this, rInfo.Communications, false);
                    }
                }
                else
                {
                    string communications = Utility.EditCommunicationToOpen(this, _cfgMgr.Configuration.CommunicationsToOpen, true);
                    if (communications != null)
                    {
                        _cfgMgr.Configuration.CommunicationsToOpen = communications;
                        _RefreshServiceStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        // 选择要开启的服务
        private void _ctlRunningServices_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (_IsRunning())
                {
                    RunningInfo rInfo = _GetRunningInfo();
                    if (rInfo != null)
                    {
                        Utility.EditServices(this, rInfo.ServiceDescs, _traySystemInvoker, false);
                    }
                }
                else
                {
                    string services = Utility.EditServices(this, _cfgMgr.Configuration.ServicesToStart, _traySystemInvoker, true);
                    if (services != null)
                    {
                        _cfgMgr.Configuration.ServicesToStart = services;
                        _RefreshServiceStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void _ctlModibyName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string clientTitle = _cfgMgr.Configuration.ClientTitle;
            string clientDesc = _cfgMgr.Configuration.ClientDesc;
            if (EditTitleAndDescDialog.Show(this, ref clientTitle, ref clientDesc))
            {
                _cfgMgr.Configuration.ClientTitle = clientTitle;
                _cfgMgr.Configuration.ClientDesc = clientDesc;

                _RefreshServiceStatus();
            }
        }

        private void _ctlRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _RefreshServiceStatus();
        }
    }
}
