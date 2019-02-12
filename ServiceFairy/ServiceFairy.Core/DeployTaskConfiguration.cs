using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ServiceFairy
{
    /// <summary>
    /// 部署任务配置
    /// </summary>
    public class DeployTaskConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("tasks")]
        public TaskConfigurationElementCollection Tasks
        {
            get { return base["tasks"] as TaskConfigurationElementCollection; }
            set { base["tasks"] = value; }
        }
    }

    public class TaskConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TaskConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TaskConfigurationElement)element).ID;
        }

        protected override string ElementName
        {
            get
            {
                return "task";
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

    public class TaskConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("id")]
        public string ID
        {
            get { return base["id"] as string; }
            set { base["id"] = value; }
        }

        [ConfigurationProperty("name")]
        public string Name
        {
            get { return base["name"] as string; }
            set { base["name"] = value; }
        }
    }
}
