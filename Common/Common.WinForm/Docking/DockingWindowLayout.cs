using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Contracts;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Runtime.Serialization;
using Common.Package.Serializer;
using Common.Contracts.Service;

namespace Common.WinForm.Docking
{
    /// <summary>
    /// 窗口布局
    /// </summary>
    public class DockingWindowLayout : MarshalByRefObjectEx
    {
        public DockingWindowLayout()
        {
            DockLeftPortion = DockRightPortion = DockTopPortion = DockBottomPortion = 0.2;
        }

        public DockingWindowLayout(DockingWindowLayoutState[] states)
            : this()
        {
            if (states != null)
                SetWindowStates(states);
        }

        private readonly List<DockingWindowLayoutState> _list = new List<DockingWindowLayoutState>();

        /// <summary>
        /// 获取窗口的状态
        /// </summary>
        /// <returns></returns>
        public DockingWindowLayoutState[] GetWindowStates()
        {
            return _list.ToArray();
        }

        /// <summary>
        /// 设置窗口的状态
        /// </summary>
        /// <param name="state"></param>
        public void SetWindowState(DockingWindowLayoutState state)
        {
            Contract.Requires(state != null);

            _list.Add(state);
        }

        /// <summary>
        /// 设置窗口的状态
        /// </summary>
        /// <param name="stats"></param>
        public void SetWindowStates(DockingWindowLayoutState[] stats)
        {
            Contract.Requires(stats != null);

            foreach (DockingWindowLayoutState state in stats)
            {
                SetWindowState(state);
            }
        }

        /// <summary>
        /// 保存布局到指定的流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        public void Save(Stream stream, DataFormat format = _defaultFormat)
        {
            Contract.Requires(stream != null);

            SerializerUtility.Serialize(format, GetWindowStates(), stream);
        }

        /// <summary>
        /// 从指定的流中加载布局
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DockingWindowLayout Load(Stream stream, DataFormat format = _defaultFormat)
        {
            Contract.Requires(stream != null);

            return new DockingWindowLayout(SerializerUtility.Deserialize<DockingWindowLayoutState[]>(_defaultFormat, stream));
        }

        /// <summary>
        /// 从指定的流中加载布局
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DockingWindowLayout Load(string filePath, DataFormat format = _defaultFormat)
        {
            Contract.Requires(filePath != null);

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Load(fs, format);
            }
        }

        private const DataFormat _defaultFormat = DataFormat.Xml;

        /// <summary>
        /// 停靠在左侧时的宽度
        /// </summary>
        public double DockLeftPortion { get; set; }

        /// <summary>
        /// 停靠在右侧时的宽度
        /// </summary>
        public double DockRightPortion { get; set; }

        /// <summary>
        /// 停靠在上侧时的高度
        /// </summary>
        public double DockTopPortion { get; set; }

        /// <summary>
        /// 停靠在下侧时的高度
        /// </summary>
        public double DockBottomPortion { get; set; }
    }
}
