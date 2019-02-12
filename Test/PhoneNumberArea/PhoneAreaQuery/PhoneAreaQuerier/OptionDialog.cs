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
using Common.Mobile;

namespace PhoneAreaQuerier
{
    public partial class OptionDialog : XDialog
    {
        public OptionDialog()
        {
            InitializeComponent();

            _country.SuspendLayout();

            foreach (CountryInfo countryInfo in CountryInfo.GetAll())
            {
                _country.Items.Add(countryInfo);
            }

            _country.ResumeLayout();
        }

        private void OptionDialog_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 所选国家
        /// </summary>
        public CountryInfo CountryInfo
        {
            get { return _country.SelectedItem as CountryInfo; }
            set { _country.SelectedItem = value; }
        }

        /// <summary>
        /// 所选运营商
        /// </summary>
        public OperationDetail OperationDetail
        {
            get { return _operationList.SelectedItem as OperationDetail; }
            set { _operationList.SelectedItem = value; }
        }

        private void _country_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountryInfo countryInfo = _country.SelectedItem as CountryInfo;
            _operationList.Items.Clear();

            if (countryInfo == null)
            {
                _lbCnName.Text = _lbEnName.Text = _lbMcc.Text = lb7.Text = _lbNationalCode.Text = _lbIsoCode.Text;
            }
            else
            {
                _lbCnName.Text = countryInfo.ChineseName;
                _lbEnName.Text = countryInfo.EnglishName;
                _lbMcc.Text = countryInfo.Mcc;
                _lbNationalCode.Text = countryInfo.NationalCode;
                _lbIsoCode.Text = countryInfo.IsoCode + " / " + countryInfo.IsoCode3;

                OperationCountry oc = Settings.GetOperationCountry(countryInfo);
                if (oc != null)
                {
                    _operationList.SuspendLayout();
                    foreach (OperationDetail od in oc.OperationDetails)
                    {
                        _operationList.Items.Add(od);
                    }

                    if (_operationList.Items.Count > 0)
                        _operationList.SelectedIndex = 0;

                    _operationList.ResumeLayout();
                }
            }
        }

        private void _operationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperationDetail od = _operationList.SelectedItem as OperationDetail;
            if (od == null)
            {
                _lbMnc.Text = _lbBrand.Text = _lbOpName.Text = _lbState.Text = _lbRemarks.Text = "";
            }
            else
            {
                _lbMnc.Text = od.Mnc;
                _lbBrand.Text = od.Brand;
                _lbOpName.Text = od.OpName;
                _lbState.Text = od.State;
                _lbRemarks.Text = od.Remarks;
            }
        }
    }
}
