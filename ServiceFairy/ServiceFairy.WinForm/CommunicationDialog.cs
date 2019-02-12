using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.Contracts.Service;
using Common.Utility;
using Common.Communication.Wcf;

namespace ServiceFairy.WinForm
{
    public partial class CommunicationDialog : XDialog
    {
        public CommunicationDialog(bool autoLoadIps = false, bool ipEditable = false, bool isDirectEnable = false)
        {
            InitializeComponent();

            _InitValues(autoLoadIps);
            _ctlIps.DropDownStyle = ipEditable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            _isDirectEnable = isDirectEnable;
            _SetDirectEnable();
        }

        private bool _isDirectEnable;

        private void _SetDirectEnable(bool enable = true)
        {
            _ctlDuplexDirect.Enabled = _ctlSimpleDirect.Enabled = (_isDirectEnable && enable);
        }

        private void _InitValues(bool autoLoadIps)
        {
            foreach (CommunicationType type in Enum.GetValues(typeof(CommunicationType)))
            {
                if (type != CommunicationType.Unknown)
                    _ctlProtocal.Items.Add(new CommunicationItem(type));
            }

            if (autoLoadIps)
            {
                SetDefault(NetworkUtility.GetAllEnableIP4Addresses().CastAsString(), null);
            }
        }

        #region Class CommunicationItem ...

        class CommunicationItem
        {
            public CommunicationItem(CommunicationType type)
            {
                Type = type;
                Desc = type.GetDesc();
            }

            public CommunicationType Type { get; private set; }

            public string Desc { get; private set; }

            public override string ToString()
            {
                return Desc;
            }

            public override int GetHashCode()
            {
                return Type.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof(CommunicationItem))
                    return false;

                CommunicationItem item = (CommunicationItem)obj;
                return item.Type == Type;
            }
        }

        #endregion

        public CommunicationOption CommunicationOption
        {
            get
            {
                if (_ctlProtocal.SelectedItem == null || (_ctlProtocal.SelectedItem == null && !string.IsNullOrWhiteSpace(_ctlProtocal.Text)))
                    return null;

                CommunicationType comType = ((CommunicationItem)_ctlProtocal.SelectedItem).Type;
                return new CommunicationOption(new ServiceAddress(
                    _ctlIps.SelectedItem.ToStringIgnoreNull(_ctlIps.Text),
                    (int)_ctlPort.Value),
                    comType, _ctlDuplexDirect.Checked && _SupportDuplex(comType)
                );
            }
            set
            {
                if (value != null)
                {
                    _ctlIps.Text = value.Address.Address;
                    _ctlPort.Value = _RevisePort(value.Address.Port);
                    _ctlProtocal.SelectedItem = _ctlProtocal.Items.Cast<CommunicationItem>().FirstOrDefault(c => c.Type == value.Type);
                    (value.Duplex ? _ctlDuplexDirect : _ctlSimpleDirect).Checked = true;
                }
                else
                {
                    _ctlIps.SelectedIndex = -1;
                    _ctlPort.Value = 0;
                    _ctlProtocal.SelectedIndex = -1;
                    _ctlSimpleDirect.Checked = true;
                }
            }
        }

        public void SetDefault(string[] ips, CommunicationOption def)
        {
            _ctlIps.Items.Add("127.0.0.1");
            _ctlIps.Items.AddRange(ips ?? new string[0]);

            CommunicationOption = def;
        }

        private int _RevisePort(int port)
        {
            if (port > _ctlPort.Maximum)
                return (int)_ctlPort.Maximum;

            if (port < _ctlPort.Minimum)
                return (int)_ctlPort.Minimum;

            return port;
        }

        private void _ctlProtocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommunicationType type = ((CommunicationItem)_ctlProtocal.SelectedItem).Type;
            _SetDirectEnable(_SupportDuplex(type));
        }

        private bool _SupportDuplex(CommunicationType type)
        {
            return type == CommunicationType.Tcp || type == CommunicationType.WTcp;
        }
    }
}
