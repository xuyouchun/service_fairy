using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ServiceFairy.Service.UI
{
    public class UIObjectConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("title", IsRequired = true)]
        public string Title
        {
            get { return this["title"] as string; }
            set { this["title"] = value; }
        }

        [ConfigurationProperty("desc")]
        public string Desc
        {
            get { return this["desc"] as string; }
            set { this["desc"] = value; }
        }

        [ConfigurationProperty("kind")]
        public string Kind
        {
            get { return this["kind"] as string; }
            set { this["kind"] = value; }
        }

        [ConfigurationProperty("help")]
        public string Help
        {
            get { return this["help"] as string; }
            set { this["help"] = value; }
        }

        [ConfigurationProperty("weight")]
        public string Weight
        {
            get { return this["weight"] as string; }
            set { this["weight"] = value; }
        }
    }
}
