using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using System.Resources;
using System.Drawing;
using System.Diagnostics.Contracts;
using System.Reflection;
using Common.Collection;
using Common.Utility;
using Common.Package.Cache;
using System.Collections;
using Common.Contracts.Service;

namespace Common.Package.UIObject
{
    public class ResourceUIObjectImageLoader : MarshalByRefObjectEx, IUIObjectImageLoader
    {
        private ResourceUIObjectImageLoader(Assembly assembly, string resourceName)
        {
            _assembly = assembly;
            _resourceName = resourceName;
        }

        private static readonly Cache<Assembly, IgnoreCaseDictionary<object>> _imageDict = new Cache<Assembly, IgnoreCaseDictionary<object>>();

        private readonly Assembly _assembly;
        private readonly string _resourceName;
        private IUIObjectImageLoader _imageLoader;

        private IgnoreCaseDictionary<object> _LoadImageDict(Assembly assembly)
        {
            IgnoreCaseDictionary<object> dict = new IgnoreCaseDictionary<object>();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                {
                    ResourceReader resourceReader = new ResourceReader(assembly.GetManifestResourceStream(resourceName));
                    foreach (DictionaryEntry item in resourceReader)
                    {
                        object value = item.Value;
                        if (value is Icon || value is Image)
                            dict[item.Key.ToString()] = value;
                    }
                }
            }

            return dict;
        }

        private IUIObjectImageLoader _GetImageLoader()
        {
            if (_imageLoader != null)
                return _imageLoader;

            if (_assembly == null)
                return _imageLoader = EmptyUIObjectImageLoader.Instance;

            IgnoreCaseDictionary<object> imageDict = _imageDict.GetOrAddOfRelative(_assembly, TimeSpan.FromSeconds(30), _LoadImageDict);
            object img;
            if (imageDict != null && (imageDict.TryGetValue(_resourceName, out img)))
                return _imageLoader = UIObjectImageLoader.From(img);

            return _imageLoader = EmptyUIObjectImageLoader.Instance;
        }

        public UIObjectImage GetImage(Size size)
        {
            return _GetImageLoader().GetImage(size);
        }

        private static readonly Cache<Tuple<Assembly, string>, ResourceUIObjectImageLoader> _cache
            = new Cache<Tuple<Assembly, string>, ResourceUIObjectImageLoader>();

        public static ResourceUIObjectImageLoader Load(object memberOrTypeOrAssembly, string resourceName)
        {
            Contract.Requires(memberOrTypeOrAssembly != null && resourceName != null);

            Assembly assembly = _GetAssembly(memberOrTypeOrAssembly);
            var key = new Tuple<Assembly, string>(assembly, resourceName);
            return _cache.GetOrAddOfRelative(key, TimeSpan.FromMinutes(10),
                (key0) => new ResourceUIObjectImageLoader(assembly, resourceName));
        }

        private static Assembly _GetAssembly(object memberOrTypeOrAssembly)
        {
            Assembly assembly = memberOrTypeOrAssembly as Assembly;
            if (assembly == null)
            {
                Type type = memberOrTypeOrAssembly as Type;
                if (type != null)
                    return type.Assembly;

                MemberInfo mInfo = memberOrTypeOrAssembly as MemberInfo;
                if (mInfo != null)
                    return mInfo.DeclaringType.Assembly;
            }

            throw new ArgumentException();
        }
    }
}
