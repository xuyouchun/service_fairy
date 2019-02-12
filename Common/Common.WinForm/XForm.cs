using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Common.WinForm
{
    public partial class XForm : Form
    {
        public XForm()
        {
            InitializeComponent();

            RegisterApplySettingsActions(new Action[]{
                _applySetCurSizeAsMinSize
            });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FormLoaded = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            FormShown = true;
        }

        #region Class ValidatorItem ...

        class ValidatorItem
        {
            public IValidator Validator { get; set; }

            public Control Control { get; set; }

            public string PropertyName { get; set; }
        }

        #endregion

        private readonly List<ValidatorItem> _validatorItems = new List<ValidatorItem>();

        /// <summary>
        /// 添加一个验证器
        /// </summary>
        /// <param name="control"></param>
        /// <param name="validator"></param>
        /// <param name="propertyName"></param>
        public void AddValidator(Control control, IValidator validator, string propertyName = null)
        {
            Contract.Requires(control != null && validator != null);

            _validatorItems.Add(new ValidatorItem() { Control = control, Validator = validator, PropertyName = propertyName });
        }

        /// <summary>
        /// 添加一个验证器
        /// </summary>
        /// <param name="control"></param>
        /// <param name="validateFunc"></param>
        /// <param name="failedMsg"></param>
        /// <param name="succeedMsg"></param>
        /// <param name="propertyName"></param>
        public void AddValidator(Control control, Func<object, bool> validateFunc, string failedMsg, string succeedMsg = "", string propertyName = null)
        {
            Contract.Requires(control != null && validateFunc != null);

            AddValidator(control, Validators.Create(validateFunc, failedMsg, succeedMsg), propertyName);
        }

        /// <summary>
        /// 添加一个验证器
        /// </summary>
        /// <param name="control"></param>
        /// <param name="validateFunc"></param>
        /// <param name="propertyName"></param>
        public void AddValidator(Control control, Func<object, string> validateFunc, string propertyName = null)
        {
            Contract.Requires(control != null && validateFunc != null);

            AddValidator(control, Validators.Create(validateFunc), propertyName);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (e.Cancel)
                return;

            foreach (ValidatorItem v in _validatorItems)
            {
                try
                {
                    ValidateResult r = _Validate(v);
                    if (!r)
                    {
                        MessageBox.Show(this, r.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorDialog.Show(this, ex);
                    e.Cancel = true;
                }
            }
        }

        private ValidateResult _Validate(ValidatorItem v)
        {
            object value;
            if (string.IsNullOrEmpty(v.PropertyName))
                value = WinFormUtility.GetControlValue(v.Control);
            else
            {
                PropertyInfo pInfo = v.Control.GetType().GetProperty(v.PropertyName);
                if (pInfo == null)
                    throw new InvalidOperationException("属性不存在:" + v.PropertyName);

                value = pInfo.GetValue(v.Control, null);
            }

            return v.Validator.Validate(value);
        }

        [Browsable(true), Description("将当前尺寸设置为最小尺寸"), DefaultValue(false), Category("XForm")]
        public bool SetCurSizeAsMinSize
        {
            get { return _setCurSizeAsMinSize; }
            set
            {
                _setCurSizeAsMinSize = value;
                _applySetCurSizeAsMinSize();
            }
        }

        private void _applySetCurSizeAsMinSize()
        {
            MinimumSize = _setCurSizeAsMinSize ? this.Size : Size.Empty;
        }

        private bool _setCurSizeAsMinSize;

        private bool _formLoaded = false;

        /// <summary>
        /// 是否已经加完完毕
        /// </summary>
        [Browsable(false)]
        public bool FormLoaded
        {
            get { return _formLoaded; }
            private set
            {
                if (_formLoaded = value && !DesignMode)
                    _ApplySettings();
            }
        }

        private bool _formShown = false;

        /// <summary>
        /// 窗体是否已经显示
        /// </summary>
        [Browsable(false)]
        public bool FormShown
        {
            get { return _formShown; }
            set
            {
                if (_formShown = value || DesignMode)
                    _ApplySettings();
            }
        }

        #region ApplySettings ...

        private readonly List<Action> _applySettings = new List<Action>();

        protected void RegisterApplySettingsActions(Action[] actions)
        {
            _applySettings.AddRange(actions);
        }

        private void _ApplySettings()
        {
            _applySettings.ForEach(action => action());
        }

        protected void ApplySetting(Action action)
        {
            if (FormLoaded || DesignMode)
                action();
        }

        #endregion

    }
}
