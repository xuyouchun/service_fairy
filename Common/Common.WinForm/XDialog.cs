using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Utility;

namespace Common.WinForm
{
    public partial class XDialog : XForm
    {
        public XDialog()
        {
            InitializeComponent();

            RegisterApplySettingsActions(new Action[] {
                _applyButtonModel
            });
        }

        [Browsable(true), Description("按钮的呈现方式"), DefaultValue(XDialogButtonModel.OkCancel), Category("XForm")]
        public XDialogButtonModel ButtonModel
        {
            get { return _buttonModel; }
            set
            {
                _buttonModel = value;
                _applyButtonModel();
            }
        }

        private XDialogButtonModel _buttonModel = XDialogButtonModel.OkCancel;
        private Button[] _commandButtons = new Button[0];

        private void _applyButtonModel()
        {
            _commandButtons.ForEach(btn => this.Controls.Remove(btn));
            ButtonGroup bg = _buttonGroup[_buttonModel];
            _commandButtons = bg.ButtonCreator();

            if (_commandButtons.Length > 0)
            {
                int x = this.ClientSize.Width - _commandButtons.Sum(btn => btn.Width + 5) - 6;
                int y = this.ClientSize.Height - _commandButtons[0].Height - 12;
                int tabIndex = int.MaxValue - 100;
                foreach (Button btn in _commandButtons)
                {
                    btn.Left = x;
                    btn.Top = y;
                    btn.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    btn.TabIndex = tabIndex++;
                    this.Controls.Add(btn);
                    x += btn.Width + 5;

                    btn.Click += new EventHandler(btn_Click);
                }

                this.AcceptButton = bg.AcceptButtonIndex < _commandButtons.Length ? _commandButtons[bg.AcceptButtonIndex] : null;
                this.CancelButton = bg.CancelButtonIndex < _commandButtons.Length ? _commandButtons[bg.CancelButtonIndex] : null;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            OnCommandButtonClick(sender, e);
        }

        /// <summary>
        /// 按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCommandButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = ((Button)sender).DialogResult;
        }

        #region ButtonGroup ...

        class ButtonGroup
        {
            public Func<Button[]> ButtonCreator { get; set; }

            public int AcceptButtonIndex { get; set; }

            public int CancelButtonIndex { get; set; }
        }

        private static readonly Dictionary<XDialogButtonModel, ButtonGroup> _buttonGroup = new Dictionary<XDialogButtonModel, ButtonGroup>() {
                { XDialogButtonModel.None, new ButtonGroup() { ButtonCreator = () => new Button[0] } },
                { XDialogButtonModel.Ok, new ButtonGroup() {
                    ButtonCreator = () => new Button[] {
                        new Button { Text = "确定", DialogResult = DialogResult.OK, CausesValidation = true }
                } } },
                { XDialogButtonModel.OkCancel, new ButtonGroup() {
                    ButtonCreator = () => new [] {
                        new Button { Text = "确定", DialogResult = DialogResult.OK, CausesValidation = true },
                        new Button { Text = "取消", DialogResult = DialogResult.Cancel }
                    }, CancelButtonIndex = 1 } },
                { XDialogButtonModel.Cancel, new ButtonGroup() {
                    ButtonCreator = () => new [] {
                        new Button { Text = "取消", DialogResult = DialogResult.Cancel }
                    }, CancelButtonIndex = 0 } },
                { XDialogButtonModel.Close, new ButtonGroup() {
                    ButtonCreator = () => new [] {
                        new Button { Text = "关闭", DialogResult = DialogResult.OK }
                    }
                } },
                { XDialogButtonModel.YesNo, new ButtonGroup() {
                    ButtonCreator = () => new [] {
                        new Button { Text = "是", DialogResult = DialogResult.Yes, CausesValidation = true },
                        new Button { Text = "否", DialogResult = DialogResult.No, CausesValidation = true }
                    }
                } },
                { XDialogButtonModel.YesNoCancel, new ButtonGroup() {
                    ButtonCreator = () => new [] {
                        new Button { Text = "是", DialogResult = DialogResult.Yes, CausesValidation = true },
                        new Button { Text = "否", DialogResult = DialogResult.No, CausesValidation = true },
                        new Button { Text = "取消", DialogResult = DialogResult.Cancel }
                    }
                } },
        };

        #endregion

        private void _btnAccept_Click(object sender, EventArgs e)
        {
            if (OnAccept != null)
                OnAccept(this, EventArgs.Empty);
        }

        [Browsable(true), Category("XForm"), Description("点击应用按钮")]
        public event EventHandler OnAccept;

        [Browsable(true), Category("XForm"), Description("是否显示“应用”按钮"), DefaultValue(false)]
        public bool ShowAcceptButton
        {
            get { return _showAcceptButton; }
            set
            {
                _showAcceptButton = value;
                _AcceptShowAdditionalButtons();
            }
        }

        private bool _showAcceptButton = false;
        private Button _applyButton;

        private void _AcceptShowAdditionalButtons()
        {
            
        }

        private IEnumerable<Button> _GetAdditionalButtons()
        {
            if (_showAcceptButton)
            {
                if (_applyButton == null)
                {
                    _applyButton = new Button() { Text = "应用", CausesValidation = true, Left = 12 };
                    _applyButton.Top = this.ClientSize.Height - _applyButton.Height - 12;
                    _applyButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                    _applyButton.Click += new EventHandler(_btnAccept_Click);
                }

                yield return _applyButton;
            }
        }

        private readonly List<Button> _buttons = new List<Button>();

        /// <summary>
        /// 添加一个按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onClick"></param>
        /// <param name="width"></param>
        public Button AddButton(string text, EventHandler onClick = null, int width = 0)
        {
            Button button = new Button() { Text = text };
            if (width > 0)
                button.Width = width;

            if (onClick != null)
                button.Click += onClick;

            _buttons.Add(button);
            _AcceptShowAdditionalButtons();

            return button;
        }
    }

    /// <summary>
    /// 按钮呈现方式
    /// </summary>
    public enum XDialogButtonModel
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// 确定
        /// </summary>
        Ok,

        /// <summary>
        /// 确定/取消
        /// </summary>
        OkCancel,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel,

        /// <summary>
        /// 关闭
        /// </summary>
        Close,

        /// <summary>
        /// 是/否
        /// </summary>
        YesNo,

        /// <summary>
        /// 是/否/取消
        /// </summary>
        YesNoCancel,
    }

}
