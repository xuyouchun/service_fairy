using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Common.Utility;
using System.IO;

namespace Common.Contracts.Service
{
    [XmlRoot("settings")]
    public class ServiceDeployPackageSettings
    {
        [XmlElement("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        public static ServiceDeployPackageSettings Load(string file)
        {
            return XmlUtility.DeserailizeFromFile<ServiceDeployPackageSettings>(file, false);
        }

        public static ServiceDeployPackageSettings TryLoad(string file)
        {
            if (file == null || !File.Exists(file))
                return null;

            try
            {
                return Load(file);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public void Save(string file)
        {
            XmlUtility.SerializeToFile(this, file, false);
        }

        public string ToXml()
        {
            return XmlUtility.SerializeToString(this, false, System.Xml.Formatting.Indented);
        }

        public const string DefaultFileName = "settings.xml";
    }
}
