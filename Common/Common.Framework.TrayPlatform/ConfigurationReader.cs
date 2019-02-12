using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package;
using Common.Contracts.Service;
using Common.Collection;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 配置文件读取器
    /// </summary>
    public class ConfigurationReader : IConfiguration, IDisposable
    {
        public ConfigurationReader(ITrayConfiguration trayConfiguration)
        {
            Contract.Requires(trayConfiguration != null);

            _trayConfiguration = trayConfiguration;
            _notifier = new ChangedNotify(this);
            _trayConfiguration.RegisterChangedNotify(_notifier);
        }

        private readonly ITrayConfiguration _trayConfiguration;
        private readonly object _thisLocker = new object();
        private readonly ChangedNotify _notifier;

        private IConfiguration _configuration;
        private string _configurationContent;

        private IConfiguration _GetConfiguration()
        {
            IConfiguration cfg = _configuration;
            if (cfg != null)
                return cfg;

            lock (_thisLocker)
            {
                if (_configuration == null)
                {
                    cfg = _CreateConfigurationReader(_configurationContent ?? _trayConfiguration.GetConfiguration());
                    _configuration = cfg;
                }

                return cfg;
            }
        }

        /// <summary>
        /// 获取配置文件的值
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public string Get(string configName)
        {
            Contract.Requires(configName != null);

            return _GetConfiguration().Get(configName) ?? System.Configuration.ConfigurationManager.AppSettings.Get(configName);
        }

        /// <summary>
        /// 获取配置项并转换为指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(string configName, T defaultValue)
        {
            return Get(configName).ToType<T>(defaultValue);
        }

        /// <summary>
        /// 获取全部的键名
        /// </summary>
        /// <returns></returns>
        public string[] GetAllKeys()
        {
            return _GetConfiguration().GetAllKeys();
        }

        private static IConfiguration _CreateConfigurationReader(string configuration)
        {
            if (string.IsNullOrEmpty(configuration))
                return new EmptyConfigurationReader();

            char firstChar = StringUtility.GetFirstNotWhiteSpaceChar(configuration);
            return firstChar == '<' ? (IConfiguration)new XmlConfigurationReader(configuration) : new KeyValueSettingsReader(configuration);
        }

        #region Class EmptyConfigurationReader ...

        class EmptyConfigurationReader : IConfiguration
        {
            public string Get(string name)
            {
                return null;
            }

            public string[] GetAllKeys()
            {
                return new string[0];
            }
        }

        #endregion

        #region Class ChangedNotify ...

        class ChangedNotify : MarshalByRefObjectEx, ITrayConfigurationChangedNotify
        {
            public ChangedNotify(ConfigurationReader r)
            {
                _r = r;
            }

            private readonly ConfigurationReader _r;

            public void Notify(string old, string @new)
            {
                lock (_r._thisLocker)
                {
                    _r._configurationContent = @new;
                    _r._configuration = null;

                    if (_r._dict.Count > 0)
                    {
                        IConfiguration oldCfg = _CreateConfigurationReader(old), newCfg = _CreateConfigurationReader(@new);
                        foreach (var item in _r._dict)
                        {
                            string key = item.Key;
                            if (item.Value.Count == 0)
                                continue;

                            string oldValue = oldCfg.Get(key), newValue = newCfg.Get(key);
                            CollectionChangedType changedType =
                                oldValue == null ? (newValue == null ? CollectionChangedType.NoChange : CollectionChangedType.Add) :
                                (newValue == null ? CollectionChangedType.Remove : (oldValue != newValue) ? CollectionChangedType.Update : CollectionChangedType.NoChange);

                            if (changedType != CollectionChangedType.NoChange)
                            {
                                foreach (var handler in item.Value)
                                {
                                    handler(this, new ConfigurationChangedEventArgs(key, oldValue, newValue, changedType));
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 注册一个变化事件
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="handler"></param>
        public void RegisterConfigModifiedEvent(string[] keys, EventHandler<ConfigurationChangedEventArgs> handler)
        {
            Contract.Requires(keys != null && handler != null);

            lock (_dict)
            {
                foreach (string key in keys)
                {
                    _dict.GetOrSet(key).Add(handler);
                }
            }
        }

        private readonly Dictionary<string, HashSet<EventHandler<ConfigurationChangedEventArgs>>> _dict
            = new Dictionary<string, HashSet<EventHandler<ConfigurationChangedEventArgs>>>();

        /// <summary>
        /// 取消注册一个变化事件
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="handler"></param>
        public void UnregisterConfigModifiedEvent(string[] keys, EventHandler<ConfigurationChangedEventArgs> handler)
        {
            Contract.Requires(keys != null && handler != null);

            lock (_dict)
            {
                foreach (string key in keys)
                {
                    _dict.GetOrSet(key).Remove(handler);
                }
            }
        }

        /// <summary>
        /// 配置文件变化事件
        /// </summary>
        public event EventHandler<ConfigurationChangedEventArgs> Changed;

        private void _RaiseChangedEvent(string name, string oldValue, string newValue, CollectionChangedType changedType)
        {
            var eh = Changed;
            if (eh != null)
            {
                eh(this, new ConfigurationChangedEventArgs(name, oldValue, newValue, changedType));
            }
        }

        public void Dispose()
        {
            _trayConfiguration.UnregisterChangedNotify(_notifier);
        }
    }

    /// <summary>
    /// 配置信息改变
    /// </summary>
    public class ConfigurationChangedEventArgs : EventArgs
    {
        internal ConfigurationChangedEventArgs(string name, string oldValue, string newValue, CollectionChangedType changedType)
        {
            Name = name;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedType = changedType;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 原值
        /// </summary>
        public string OldValue { get; private set; }

        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; private set; }

        /// <summary>
        /// 变化类型
        /// </summary>
        public CollectionChangedType ChangedType { get; private set; }
    }

}
