using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics.Contracts;
using Common.Contracts.Entities;
using Common.Utility;

namespace Common.Mobile
{
    /// <summary>
    /// 运营与国家对应关系的管理器
    /// </summary>
    public class OperationCountryManager
    {
        public OperationCountryManager(XmlDocument xDoc)
            : this(() => OperationCountry.LoadFromXmlDocument(xDoc))
        {
            Contract.Requires(xDoc != null);
        }

        public OperationCountryManager(string xmlFile)
            : this(() => OperationCountry.LoadFromXmlFile(xmlFile))
        {
            Contract.Requires(xmlFile != null);
        }

        private OperationCountryManager(Func<OperationCountry[]> ocLoader)
        {
            _ocLoader = ocLoader;

            _operationCountryWrapper = new Lazy<OperationCountryWrapper>(_LoadOperationCountryWrapper);
        }

        private readonly Func<OperationCountry[]> _ocLoader;

        private OperationCountryWrapper _LoadOperationCountryWrapper()
        {
            OperationCountry[] ocs = _ocLoader();
            return new OperationCountryWrapper() {
                Countries = ocs,
                Dict = ocs.ToDictionary(oc => oc.CountryInfo)
            };
        }

        class OperationCountryWrapper
        {
            public OperationCountry[] Countries;

            public Dictionary<CountryInfo, OperationCountry> Dict;
        }

        private readonly Lazy<OperationCountryWrapper> _operationCountryWrapper;

        /// <summary>
        /// 获取全部的OperationCountry
        /// </summary>
        /// <returns></returns>
        public OperationCountry[] GetAllOperationCountries()
        {
            return _operationCountryWrapper.Value.Countries;
        }

        /// <summary>
        /// 获取指定国家的的OperationCountry
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public OperationCountry GetOperationCountry(CountryInfo cInfo)
        {
            return _operationCountryWrapper.Value.Dict.GetOrDefault(cInfo);
        }
    }
}
