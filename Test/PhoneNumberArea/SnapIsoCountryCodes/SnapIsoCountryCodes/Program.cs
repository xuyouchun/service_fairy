using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Common.Utility;
using System.Xml;
using Common.Contracts.Entities;

namespace SnapIsoCountryCodes
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConvertChinaToXml.Execute();
            //ConvertXmlToStreamTable.Execute();
            //SnapMcc.ToXml();
            GenerateNationalCodeTable.Execute();

            //InsertMccToCountryNames.Execute();

            //SnapNationalAreaCodes.Execute();

            //CountryInfo cInfo = CountryInfo.FromMcc("460");
        }
    }
}


