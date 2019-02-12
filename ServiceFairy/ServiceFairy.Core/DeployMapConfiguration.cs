using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ServiceFairy
{
    public class DeployMapConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("clients")]
        [ConfigurationCollection(typeof(ClientConfigurationElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public ClientConfigurationElementCollection Clients
        {
            get { return base["clients"] as ClientConfigurationElementCollection; }
            set { base["clients"] = value; }
        }

        public class ClientConfigurationElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new ClientConfigurationElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((ClientConfigurationElement)element).ClientID;
            }

            protected override string ElementName
            {
                get
                {
                    return "client";
                }
            }

            public override ConfigurationElementCollectionType CollectionType
            {
                get
                {
                    return ConfigurationElementCollectionType.BasicMap;
                }
            }
        }

        public class ClientConfigurationElement : ConfigurationElement
        {
            [ConfigurationProperty("clientId", IsRequired = true)]
            public string ClientID
            {
                get { return base["clientId"] as string; }
                set { base["clientId"] = value; }
            }

            [ConfigurationProperty("services")]
            [ConfigurationCollection(typeof(ServiceConfigurationElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
            public ServiceConfigurationElementCollection Services
            {
                get { return base["services"] as ServiceConfigurationElementCollection; }
                set { base["services"] = value; }
            }

            [ConfigurationProperty("communicates")]
            [ConfigurationCollection(typeof(ServiceConfigurationElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
            public CommunicateConfigurationelementCollection Communicates
            {
                get { return base["communicates"] as CommunicateConfigurationelementCollection; }
                set { base["communicates"] = value; }
            }
        }

        public class ServiceConfigurationElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new ServiceConfigurationElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                ServiceConfigurationElement e = (ServiceConfigurationElement)element;
                return e.Name + " " + e.Version;
            }

            protected override string ElementName
            {
                get
                {
                    return "service";
                }
            }

            public override ConfigurationElementCollectionType CollectionType
            {
                get
                {
                    return ConfigurationElementCollectionType.BasicMap;
                }
            }
        }

        public class ServiceConfigurationElement : ConfigurationElement
        {
            [ConfigurationProperty("name", IsRequired = true)]
            public string Name
            {
                get { return base["name"] as string; }
                set { base["name"] = value; }
            }

            [ConfigurationProperty("version", DefaultValue = "1.0")]
            public string Version
            {
                get { return base["version"] as string; }
                set { base["version"] = value; }
            }
        }

        public class CommunicateConfigurationelementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new CommunicateConfigurationElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                CommunicateConfigurationElement e = (CommunicateConfigurationElement)element;
                return e.Address + ":" + e.Port;
            }

            public override ConfigurationElementCollectionType CollectionType
            {
                get
                {
                    return ConfigurationElementCollectionType.BasicMap;
                }
            }

            protected override string ElementName
            {
                get
                {
                    return "communicate";
                }
            }
        }

        public class CommunicateConfigurationElement : ConfigurationElement
        {
            [ConfigurationProperty("address", IsRequired = true)]
            public string Address
            {
                get { return base["address"] as string; }
                set { base["address"] = value; }
            }

            [ConfigurationProperty("port", IsRequired = true)]
            public string Port
            {
                get { return base["port"] as string; }
                set { base["port"] = value; }
            }

            [ConfigurationProperty("type", IsRequired = true)]
            public string Type
            {
                get { return base["type"] as string; }
                set { base["type"] = value; }
            }
        }
    }
}