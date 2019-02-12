using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Xml;
using Common.Contracts.Entities;

namespace Common.Mobile
{
    /// 运营商与国家的组合
    /// </summary>
    public class OperationCountry
    {
        public OperationCountry(CountryInfo countryInfo, OperationDetail[] operationDetails)
        {
            CountryInfo = countryInfo;
            OperationDetails = operationDetails;
        }

        /// <summary>
        /// 国家
        /// </summary>
        public CountryInfo CountryInfo { get; private set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public OperationDetail[] OperationDetails { get; private set; }

        /// <summary>
        /// 根据MNC获取运营商
        /// </summary>
        /// <param name="mnc"></param>
        /// <returns></returns>
        public OperationDetail GetOperationDetail(string mnc)
        {
            return OperationDetails.FirstOrDefault(od => od.Mnc == mnc);
        }

        /// <summary>
        /// 获取默认的运营商
        /// </summary>
        /// <returns></returns>
        public OperationDetail GetDefaultOperationDetail()
        {
            return OperationDetails.First();
        }

        /// <summary>
        /// 从xml中加载
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static OperationCountry[] LoadFromXmlDocument(XmlDocument xDoc)
        {
            Contract.Requires(xDoc != null);

            List<OperationCountry> items = new List<OperationCountry>();
            foreach (XmlElement countryElement in xDoc.SelectNodes("/root/countries/country"))
            {
                string isoCode = countryElement.GetAttribute("isoCode");
                List<OperationDetail> ods = new List<OperationDetail>();
                foreach (XmlElement opElement in countryElement.SelectNodes("operations/operation"))
                {
                    string mnc = opElement.GetAttribute("mnc"), brand = opElement.GetAttribute("brand");
                    string opName = opElement.GetAttribute("op_name"), state = opElement.GetAttribute("state");
                    string remarks = opElement.GetAttribute("remarks");

                    OperationDetail od = new OperationDetail(mnc, brand, opName, state, remarks);
                    ods.Add(od);
                }

                CountryInfo cInfo = CountryInfo.FromIsoCode(isoCode);
                if (cInfo != null)
                    items.Add(new OperationCountry(cInfo, ods.ToArray()));
            }

            return items.ToArray();
        }

        /// <summary>
        /// 从XML中加载
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static OperationCountry[] LoadFromXml(string xml)
        {
            Contract.Requires(xml != null);

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            return LoadFromXmlDocument(xDoc);
        }

        /// <summary>
        /// 从XML文件中加载
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static OperationCountry[] LoadFromXmlFile(string xmlFile)
        {
            Contract.Requires(xmlFile != null);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlFile);
            return LoadFromXmlDocument(xDoc);
        }
    }
}
