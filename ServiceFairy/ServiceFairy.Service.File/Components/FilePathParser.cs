using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.File.Components
{
    class FilePathParser
    {
        public FilePathParser(string path)
        {
            Contract.Requires(path != null);
            _Validate(path.IndexOf('\\') < 0, @"路径中不允许含有“\”，使用“/”代替");

            int index = path.LastIndexOf(':');
            if (index >= 0)
            {
                RoutePath = path.Substring(0, index);
                NamePath = path.Substring(index + 1);
            }
            else
            {
                RoutePath = path;
                NamePath = "";
            }
            
            _Validate(RoutePath.IndexOf("//") < 0, "RoutePath不允许含有为空的部分");
            _Validate(!RoutePath.StartsWith("/") && !RoutePath.EndsWith("/"), "RoutePath不允许以斜线起始或结束");
            _Validate(RoutePath.IndexOfAny(new char[] { ':', '*' }) < 0, "RoutePath不允许含有冒号或星号");
            _Validate(NamePath.IndexOf("//") < 0, "NamePath不允许含有为空的部分");
            _Validate(!NamePath.StartsWith("/") && !NamePath.EndsWith("/"), "NamePath不允许以斜线起始或结束");

            Path = path.Replace(":", "/");
        }

        private void _Validate(bool succeed, string errorMsg)
        {
            if (!succeed)
                throw new FormatException(errorMsg);
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 用于路由的路径
        /// </summary>
        public string RoutePath { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string NamePath { get; private set; }

        private int? _routeHashCode;

        public int RouteHashCode
        {
            get
            {
                return (int)(_routeHashCode ?? (_routeHashCode = RoutePath.ToLower().GetHashCode()));
            }
        }
    }
}
