using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WinForm
{
    /// <summary>
    /// 用于展示对象的控件
    /// </summary>
    public class ObjectViewControl : XUserControl
    {
        private object _data;

        /// <summary>
        /// 数据
        /// </summary>
        public object Data
        {
            get { return _data; }
            set
            {
                _data = value;
                SetData(value);
            }
        }

        /// <summary>
        /// 将数据显示在界面上
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetData(object data)
        {

        }

        /// <summary>
        /// 将界面上的数据
        /// </summary>
        public virtual object GetData()
        {
            return null;
        }
    }
}
