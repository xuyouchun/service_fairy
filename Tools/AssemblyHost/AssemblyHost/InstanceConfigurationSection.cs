using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace AssemblyHost
{
    public class InstanceConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(InstanceConfigurationElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public InstanceConfigurationElementCollection Instances
        {
            get { return base[""] as InstanceConfigurationElementCollection; }
            set { base[""] = value; }
        }
    }

    public class InstanceConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new InstanceConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InstanceConfigurationElement)element).Name;
        }

        protected override string ElementName
        {
            get
            {
                return "instance";
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        [ConfigurationProperty("defaultPath")]
        public string DefaultPath
        {
            get { return base["defaultPath"] as string; }
            set { base["defaultPath"] = value; }
        }

        [ConfigurationProperty("defaultAppConfig")]
        public string DefaultAppConfig
        {
            get { return base["defaultAppConfig"] as string; }
            set { base["defaultAppConfig"] = value; }
        }
    }

    public class InstanceConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return base["name"] as string; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return base["path"] as string; }
            set { base["path"] = value; }
        }

        [ConfigurationProperty("appConfig")]
        public string AppConfig
        {
            get { return base["appConfig"] as string; }
            set { base["appConfig"] = value; }
        }
    }
}
