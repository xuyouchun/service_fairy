using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.Utility;

namespace ServiceFairy.TrayManagement
{
    class RadioButtonManager
    {
        public RadioButtonManager(RadioButton[] radioButtons)
        {
            foreach (RadioButton radio in radioButtons)
            {
                radio.Click += new EventHandler(radio_Click);
                if (radio.Checked)
                    _current = radio;

                radio.AutoCheck = false;
                if (radio.Tag != null)
                    _radios.Add(radio.Tag, radio);
            }
        }

        public RadioButtonManager(Control parent)
            : this(parent.FindControls<RadioButton>().ToArray())
        {
            
        }

        private void radio_Click(object sender, EventArgs e)
        {
            _current = (RadioButton)sender;

            if (!_readonly)
            {
                _SetCurrentRadioButton((RadioButton)sender);
            }
            else
            {
                ClickWhenReadonly.RaiseEvent(this);
            }
        }

        private RadioButton _current;
        private readonly Dictionary<object, RadioButton> _radios = new Dictionary<object, RadioButton>();

        /// <summary>
        /// 获取所有的值
        /// </summary>
        /// <returns></returns>
        public object[] GetAllValues()
        {
            return _radios.Values.Select(radio => radio.Tag).ToArray();
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public object Value
        {
            get
            {
                return _current == null ? null : _current.Tag;
            }
            set
            {
                RadioButton rb = _radios.GetOrDefault(value);
                _SetCurrentRadioButton(rb);
            }
        }

        private void _SetCurrentRadioButton(RadioButton rb)
        {
            _radios.Values.ForEach(r => r.Checked = (r == rb));

            _current = rb;
            CurrentChanged.RaiseEvent(this);
        }

        private bool _readonly = false;

        /// <summary>
        /// 是否为有效状态
        /// </summary>
        public bool Readonly
        {
            get { return _readonly; }
            set
            {
                _readonly = value;
            }
        }

        /// <summary>
        /// 值变化
        /// </summary>
        public event EventHandler CurrentChanged;

        /// <summary>
        /// 在只读方式下点击
        /// </summary>
        public event EventHandler ClickWhenReadonly;
    }
}
