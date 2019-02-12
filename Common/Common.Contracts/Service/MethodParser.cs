using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 请求方法的解析器
    /// </summary>
    public class MethodParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="method"></param>
        public MethodParser(string method)
        {
            _method = method ?? string.Empty;

            int k = _method.IndexOf('/');
            if (k < 0)
            {
                ParseService("", out _service, out _serviceVersion);
                ParseCommand(_method, out _command, out _commandVersion);
            }
            else
            {
                ParseService(_method.Substring(0, k), out _service, out _serviceVersion);
                ParseCommand(_method.Substring(k + 1), out _command, out _commandVersion);
            }
        }

        private static void _Parse(string s, out string name, out SVersion version)
        {
            int index;
            if (string.IsNullOrEmpty(s = s.Trim()))
            {
                name = string.Empty;
                version = SVersion.Empty;
            }
            else if ((index = s.IndexOf(' ')) > 0)
            {
                name = s.Substring(0, index);
                version = SVersion.Parse(s.Substring(index + 1));
            }
            else
            {
                name = s;
                version = SVersion.Empty;
            }
        }

        private readonly string _method;

        /// <summary>
        /// Action
        /// </summary>
        public string Method
        {
            get { return _method; }
        }

        private string _service;

        /// <summary>
        /// Service
        /// </summary>
        public string Service { get { return _service; } }

        private SVersion _serviceVersion;

        /// <summary>
        /// Service Version
        /// </summary>
        public SVersion ServiceVersion { get { return _serviceVersion; } }

        private string _command;

        /// <summary>
        /// Method
        /// </summary>
        public string Command { get { return _command; } }

        private SVersion _commandVersion;

        /// <summary>
        /// Command Version
        /// </summary>
        public SVersion CommandVersion { get { return _commandVersion; } }

        private ServiceDesc _serviceDesc;

        /// <summary>
        /// 服务描述
        /// </summary>
        public ServiceDesc ServiceDesc
        {
            get { return _serviceDesc ?? (_serviceDesc = new ServiceDesc(Service, ServiceVersion)); }
        }

        private CommandDesc _commandDesc;

        /// <summary>
        /// 指令描述
        /// </summary>
        public CommandDesc CommandDesc
        {
            get { return _commandDesc ?? (_commandDesc = new CommandDesc(Command, CommandVersion)); }
        }

        /// <summary>
        /// 获取服务名称
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetServiceName(string method)
        {
            return new MethodParser(method).Service;
        }

        /// <summary>
        /// 转换服务名称与版本号
        /// </summary>
        /// <param name="service"></param>
        /// <param name="serviceName"></param>
        /// <param name="serviceVersion"></param>
        /// <returns></returns>
        public static void ParseService(string service, out string serviceName, out SVersion serviceVersion)
        {
            _Parse(service ?? string.Empty, out serviceName, out serviceVersion);
        }

        /// <summary>
        /// 转换服务名称与版本号
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static ServiceDesc ParseService(string service)
        {
            string serviceName;
            SVersion serviceVersion;
            ParseService(service, out serviceName, out serviceVersion);
            return new ServiceDesc(serviceName, serviceVersion);
        }

        /// <summary>
        /// 转换指令与版本号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandName"></param>
        /// <param name="commandVersion"></param>
        /// <returns></returns>
        public static void ParseCommand(string command, out string commandName, out SVersion commandVersion)
        {
            _Parse(command ?? string.Empty, out commandName, out commandVersion);
        }

        /// <summary>
        /// 转换指令与版本号
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static CommandDesc ParseCommand(string command)
        {
            string commandName;
            SVersion commandVersion;
            ParseCommand(command, out commandName, out commandVersion);
            return new CommandDesc(commandName, commandVersion);
        }

        public override string ToString()
        {
            return ToString(false);
        }

        private string ToString(bool full)
        {
            if (string.IsNullOrEmpty(ServiceDesc.Name))
                return CommandDesc.ToString(full);

            return ServiceDesc.ToString(full) + "/" + CommandDesc.ToString(full);
        }
    }
}
