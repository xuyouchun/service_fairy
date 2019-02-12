using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using Common.Contracts.Service;

namespace Common.WinForm.Docking
{
    /// <summary>
    /// 停靠窗口的状态
    /// </summary>
    [Serializable, DataContract(Name = "DockingWindowState")]
    public class DockingWindowLayoutState : MarshalByRefObjectEx
    {
        public DockingWindowLayoutState()
        {

        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        [DataMember]
        public UniqueId UniqueId { get; set; }

        /// <summary>
        /// 父窗体唯一标识
        /// </summary>
        [DataMember]
        public UniqueId ParentUniqueId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 自动隐藏时的尺寸
        /// </summary>
        [DataMember]
        public double AutoHidePortion { get; set; }

        /// <summary>
        /// 窗体的类型，用于创建窗体
        /// </summary>
        [DataMember]
        public Type Type { get; set; }

        /// <summary>
        /// 停靠状态
        /// </summary>
        public DockState DockState { get; set; }

        /// <summary>
        /// 相关数据
        /// </summary>
        [DataMember]
        public string StateData { get; set; }

        /// <summary>
        /// 可停靠的区域
        /// </summary>
        [DataMember]
        public DockAreas DockAreas { get; set; }
    }
}
