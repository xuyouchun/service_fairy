using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.Contracts.Entities;
using Common.Mobile.Area;
using Common.Mobile;
namespace PhoneAreaQuerier
{
    public partial class MainForm : XDialog
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _thisCountry = CountryInfo.FromIsoCode(Settings.DefaultCountryIsoCode);
            OperationCountry oc = Settings.GetOperationCountry(_thisCountry);
            _opDetail = oc.GetOperationDetail(Settings.DefaultOperationMnc) ?? oc.GetDefaultOperationDetail();

            _Init();
            _Query();
        }

        private void _Init()
        {
            string s = string.Format("所在国家：{0}({1}) 国际区号:{2} MCC:{3} ISO代号:{4}",
                _thisCountry.ChineseName, _thisCountry.EnglishName, _thisCountry.NationalCode,
                _thisCountry.Mcc, _thisCountry.IsoCode + "/" + _thisCountry.IsoCode3);

            s += "\r\n当前运营商：" + (_opDetail == null ? "（无）" : _opDetail.ToString());

            _lbInfo.Text = s;

            if (_query != null)
                _query.Dispose();

            try
            {
                if (_opDetail == null)
                {
                    _query = null;
                }
                else
                {
                    StreamTablePhoneAreaDataProvider dp = new StreamTablePhoneAreaDataProvider(Settings.AreaDataBasePath);
                    _query = PhoneAreaQuery.CreatePhoneAreaQuery(_thisCountry, _opDetail.Mnc, dp);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }

        private PhoneAreaQuery _query;

        private void _txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            _Query();
        }

        private void _Query()
        {
            if (_query == null)
            {
                _lbAreaText.Text = "（未选择运营商）";
                return;
            }

            try
            {
                PhoneArea area = _query.GetPhoneArea(_txtPhoneNumber.Text.Trim());
                if (area == null)
                {
                    _lbAreaText.Text = "????";
                }
                else
                {
                    _lbAreaText.Text = area.ToString();
                }
            }
            catch (Exception)
            {
                _lbAreaText.Text = "????";
            }
        }

        private CountryInfo _thisCountry;
        private OperationDetail _opDetail;

        // 设置选项
        private void _btnSet_Click(object sender, EventArgs e)
        {
            OptionDialog dlg = new OptionDialog();
            dlg.CountryInfo = _thisCountry;
            dlg.OperationDetail = _opDetail;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _thisCountry = dlg.CountryInfo;
                _opDetail = dlg.OperationDetail;
                _Init();
                _Query();
            }
        }

        protected override void OnCommandButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
