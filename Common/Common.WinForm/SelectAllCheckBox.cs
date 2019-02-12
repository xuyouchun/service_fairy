using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Utility;

namespace Common.WinForm
{
    public partial class SelectAllCheckBox : CheckBox
    {
        public SelectAllCheckBox()
        {
            InitializeComponent();
        }

        public SelectAllCheckBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            this.CheckedChanged += new EventHandler(SelectAllCheckBox_CheckedChanged);
        }

        /// <summary>
        /// 关联的ListBox
        /// </summary>
        [Browsable(true), Category("XControl")]
        public ListBox ListBox
        {
            get { return _listBox; }
            set
            {
                if (value != null)
                {
                    if (_listBox != null)
                    {
                        _listBox.SelectedIndexChanged -= new EventHandler(_listBox_SelectedIndexChanged);
                    }

                    _listBox = value;
                    _listBox.SelectedIndexChanged += new EventHandler(_listBox_SelectedIndexChanged);

                    UpdateState();
                }
            }
        }

        private ListBox _listBox;

        private bool _AllSelected()
        {
            if (_listBox == null)
                return false;

            return _listBox.SelectedItems.Count == _listBox.Items.Count;
        }

        private bool _AllDeselected()
        {
            if (_listBox == null)
                return false;

            return _listBox.SelectedItems.Count == 0;
        }

        /// <summary>
        /// 更新选择状态
        /// </summary>
        public void UpdateState()
        {
            _SuspendEventBinding();

            CheckState = _AllSelected() ? CheckState.Checked : _AllDeselected() ? CheckState.Unchecked : CheckState.Indeterminate;

            _ResumeEventBinding();
        }

        private void _listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateState();
        }

        private void _SuspendEventBinding()
        {
            if (_listBox != null)
                _listBox.SelectedIndexChanged -= new EventHandler(_listBox_SelectedIndexChanged);

            this.CheckedChanged -= new EventHandler(SelectAllCheckBox_CheckedChanged);
        }

        private void _ResumeEventBinding()
        {
            if (_listBox != null)
                _listBox.SelectedIndexChanged += new EventHandler(_listBox_SelectedIndexChanged);

            this.CheckedChanged += new EventHandler(SelectAllCheckBox_CheckedChanged);
        }

        private void SelectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_listBox == null)
                return;

            _SuspendEventBinding();
            _listBox.BeginUpdate();
            int top = _listBox.TopIndex;
            for (int index = 0; index < _listBox.Items.Count; index++)
            {
                _listBox.SetSelected(index, Checked);
            }
            _listBox.TopIndex = top;
            _listBox.EndUpdate();

            _ResumeEventBinding();
        }
    }
}
