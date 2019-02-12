using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;

namespace Common.WinForm.Docking
{
    /// <summary>
    /// 可停靠窗口的创建工厂
    /// </summary>
    public interface IDockingWindowFactory
    {
        /// <summary>
        /// 创建指定类型的窗口
        /// </summary>
        /// <param name="windowType"></param>
        /// <returns></returns>
        DockContentEx CreateWindow(Type windowType);
    }

    /// <summary>
    /// 默认的DockingWindowFactory，使用反射来创建
    /// </summary>
    public class DefaultDockingWindowFactory : IDockingWindowFactory
    {
        public DockContentEx CreateWindow(Type windowType)
        {
            return ObjectFactory.CreateObject(windowType) as DockContentEx;
        }
    }

    /// <summary>
    /// 可以指定创建方式的DockingWindowFactory
    /// </summary>
    public class DockingWindowFactory : IDockingWindowFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instanceDict"></param>
        /// <param name="typeDict"></param>
        public DockingWindowFactory(
            IDictionary<Type, DockContentEx> instanceDict,
            IDictionary<Type, Type> typeDict = null
        )
        {
            _instanceDict = instanceDict;
            _typeDict = typeDict;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="typeDict"></param>
        public DockingWindowFactory(IEnumerable<DockContentEx> instances, IDictionary<Type, Type> typeDict = null)
            : this(instances.ToDictionary(v=>v.GetType()), typeDict)
        {

        }

        private readonly IDictionary<Type, DockContentEx> _instanceDict;
        private readonly IDictionary<Type, Type> _typeDict;
        private static readonly DefaultDockingWindowFactory _defaultFactory = new DefaultDockingWindowFactory();

        /// <summary>
        /// 创建指定类型的窗口
        /// </summary>
        /// <param name="windowType"></param>
        /// <returns></returns>
        public DockContentEx CreateWindow(Type windowType)
        {
            DockContentEx dc;
            if (_instanceDict != null && _instanceDict.TryGetValue(windowType, out dc))
                return dc;

            Type type;
            if (_typeDict != null && _typeDict.TryGetValue(windowType, out type))
                return ObjectFactory.CreateObject(type) as DockContentEx;

            return _defaultFactory.CreateWindow(windowType);
        }
    }
}
