using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Contracts.UIObject;
using Common.Package;
using Common.Package.UIObject;

namespace Common.WinForm.Docking
{
    public partial class DockContentEx : DockContent, IUIWindow, IDockContentOperation
    {
        public DockContentEx()
        {
            InitializeComponent();

            _defaultUIWindowInfo = new UIWindowInfo(Text, new UIObjectImageLoader(Icon.ToBitmap()));
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private readonly UIWindowInfo _defaultUIWindowInfo;

        /// <summary>
        /// 获取该窗口的唯一标识
        /// </summary>
        /// <returns></returns>
        public virtual UniqueId GetUniqueId()
        {
            return new UniqueId(this.GetType().ToString());
        }

        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="layoutState"></param>
        public virtual void SetLayoutState(DockingWindowLayoutState layoutState)
        {
            Contract.Requires(layoutState != null);

            if (layoutState.DockAreas != 0)
                this.DockAreas = layoutState.DockAreas;

            if (layoutState.AutoHidePortion > 0)
                this.AutoHidePortion = layoutState.AutoHidePortion;
        }

        /// <summary>
        /// 获取布局
        /// </summary>
        /// <returns></returns>
        public virtual DockingWindowLayoutState GetLayoutState()
        {
            return new DockingWindowLayoutState() {
                UniqueId = GetUniqueId(),
                Type = this.GetType(),
                StateData = GetStateData(),
                Title = Text,
                DockState = this.DockState,
                DockAreas = this.DockAreas,
            };
        }

        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <returns></returns>
        protected virtual string GetStateData()
        {
            return string.Empty;
        }

        /// <summary>
        /// 设置状态数据
        /// </summary>
        /// <param name="data"></param>
        protected virtual void SetStateData(string data)
        {
            
        }

        /// <summary>
        /// 设置窗口的信息
        /// </summary>
        /// <param name="info"></param>
        public void SetWindowInfo(UIWindowInfo info)
        {
            info = info ?? _defaultUIWindowInfo;

            if (info.Title != null)
                Text = info.Title;

            if (info.ImageLoader != null)
            {
                Image img = info.ImageLoader.GetImageOrTransparent(16);
                Icon = Icon.FromHandle(new Bitmap(img).GetHicon());
            }
        }

        void IUIWindow.Activate()
        {
            Visible = true;
            this.Activate();
        }

        bool IUIWindow.Visible
        {
            get { return this.VisibleState != DockState.Hidden; }
            set 
            {
                if (value)
                    this.Show();
                else
                    this.Hide();
            }
        }

        /// <summary>
        /// 关闭窗体的通知
        /// </summary>
        /// <returns></returns>
        public virtual bool ClosingNotify()
        {
            return true;
        }
    }
}
