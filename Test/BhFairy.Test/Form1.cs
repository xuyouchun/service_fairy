using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiceFairy;
using Common.Communication.Wcf;
using System.Configuration;
using Common.Contracts.Service;
using Common.Package.Service;
using BhFairy.Entities;
using Common.Contracts;
using Common.Framework.TrayPlatform;
using Common.Utility;
using System.Diagnostics;
using ServiceFairy.Entities;
using Common.Contracts.Entities;

namespace BhFairy.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private WcfService _wcfService = new WcfService();
        private static readonly ClientTag _clientTag = new ClientTag() { Tag = new Guid() };

        private SFClient _GetSFClient(CommunicationOption option)
        {
            WcfConnection con = _wcfService.Connect(option.Address, option.CommunicationType);
            con.Open();
            return new SFClient(con, DataFormat.Binary, BufferType.Bytes, true);
        }

        private CommunicationOption[] _communicateOptions;

        private CommunicationOption[] _GetProxyList()
        {
            string navigationAddress = ConfigurationManager.AppSettings.Get("navigationAddress");
            using (SFClient sf = _GetSFClient(new CommunicationOption() { Address = ServiceAddress.Parse(navigationAddress), CommunicationType = CommunicationType.Tcp }))
            {
                ServiceResult<Navigation_GetProxyList_Reply> r = sf.Call<Navigation_GetProxyList_Reply>(SFNames.ServiceNames.Navigation + "/GetProxyList",
                    new Navigation_GetProxyList_Request() { CommunicationType = CommunicationType.Tcp, MaxCount = 0 });

                if (r.StatusCode != ServiceStatusCode.OK)
                    throw new InvalidOperationException("无法获取代理服务器列表: " + r.StatusCode);

                return r.Result.CommunicationOptions;
            }
        }

        private SFClient _GetFrontSFClient()
        {
            if (_communicateOptions == null)
            {
                _communicateOptions = _GetProxyList();
                if (_communicateOptions.IsNullOrEmpty())
                    throw new ApplicationException("代理服务器列表为空");
            }

            return _GetSFClient(_communicateOptions[0]);
        }


        private void _ShowMessage(string msg, string caption = "")
        {
            MessageBox.Show(this, msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void _ShowError(Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void _ShowResult(ServiceStatusCode code)
        {
            if (code == ServiceStatusCode.OK)
                _ShowMessage("成功");
            else
                _ShowMessage("失败:" + code);
        }

        #region 注册 ...

        private void _register_Click(object sender, EventArgs e)
        {
            try
            {
                using (SFClient sf = _GetFrontSFClient())
                {
                    ServiceResult<Authentication_RegisterReply> r = sf.Call<Authentication_RegisterReply>(ApplicationServiceNames.Authentication + "/Register",
                        new Authentication_RegisterRequest() { LoginName = _loginName.Text, LoginNameType = LoginNameType.Email, Password = _password.Text, ClientTag = _clientTag });

                    _ShowResult(r.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _ShowError(ex);
            }
        }

        #endregion

        #region 登录 ...

        private void _loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SFClient sf = _GetFrontSFClient())
                {
                    var r = sf.Call<Authentication_LoginReply>(ApplicationServiceNames.Authentication + "/Login",
                        new Authentication_LoginRequest() { ClientTag = _clientTag, Password = _passwordTxt.Text, LoginName = _loginNameTxt.Text });

                    //Stopwatch sw = Stopwatch.StartNew();
                    //for (int k = 0; k < 1000; k++)
                    //{
                    //    ServiceResult<Authentication_LoginReply> r = sf.Call<Authentication_LoginReply>(ApplicationServiceNames.Authentication + "/UserLogin",
                    //    new Authentication_LoginRequest() { ClientTag = _clientTag, Password = _passwordTxt.Text, LoginName = _loginNameTxt.Text });
                    //}

                    //sw.Stop();
                    //_ShowMessage(sw.ElapsedMilliseconds.ToString());

                    _ShowResult(r.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _ShowError(ex);
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region 代理服务器 ...

        private void btnGetProxyList_Click(object sender, EventArgs e)
        {
            try
            {
                CommunicationOption[] options = _GetProxyList();
                txtProxy.Text = string.Join("\r\n", (object[])options);

                SFClient sf = _GetFrontSFClient();
                ServiceResult<Share_ShareMyLocationReply> sr = sf.Call<Share_ShareMyLocationReply>(ApplicationServiceNames.Share + "/ShareMyLocation", new Share_ShareMyLocationRequest() {
                    ClientTag = new ClientTag(), UserSID = new UserSID(), Location = new GeoLocation(100, 100)
                });
            }
            catch (Exception ex)
            {
                _ShowError(ex);
            }
        }

        #endregion
    }
}
