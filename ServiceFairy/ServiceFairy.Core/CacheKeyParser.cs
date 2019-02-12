using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ServiceFairy
{
    public class CacheKeyParser
    {
        public CacheKeyParser(string key)
        {
            Contract.Requires(key != null);

            Key = key;

            int index = key.LastIndexOf(':');
            if (index >= 0)
            {
                RoutePath = key.Substring(0, index);
                NamePath = key.Substring(index + 1);
            }
            else
            {
                RoutePath = key;
                NamePath = "";
            }

            _Validate(RoutePath.IndexOf("//") < 0, "RoutePath不允许含有为空的部分");
            _Validate(!RoutePath.StartsWith("/") && !RoutePath.EndsWith("/"), "RoutePath不允许以斜线起始或结束");
            _Validate(RoutePath.IndexOfAny(new char[] { ':', '*' }) < 0, "RoutePath不允许含有冒号或星号");
            _Validate(NamePath.IndexOf("//") < 0, "NamePath不允许含有为空的部分");
            _Validate(!NamePath.StartsWith("/") && !NamePath.EndsWith("/"), "NamePath不允许以斜线起始或结束");
        }

        private void _Validate(bool succeed, string errorMsg)
        {
            if (!succeed)
                throw new FormatException(errorMsg);
        }

        /// <summary>
        /// Cache Key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Route Path
        /// </summary>
        public string RoutePath { get; private set; }

        /// <summary>
        /// Name Path
        /// </summary>
        public string NamePath { get; private set; }

        private int? _routeHashCode;

        public int RouteHashCode
        {
            get
            {
                return (int)(_routeHashCode ?? (_routeHashCode = RoutePath.GetHashCode()));
            }
        }

        private string[] _routePathParts;

        public string[] RoutePathParts
        {
            get
            {
                return _routePathParts ?? (_routePathParts = RoutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        private string[] _namePathParts;

        public string[] NamePathParts
        {
            get
            {
                return _namePathParts ?? (_namePathParts = NamePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }
    }
}
