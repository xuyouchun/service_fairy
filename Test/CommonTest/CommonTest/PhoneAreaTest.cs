using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Mobile.Area;
using Common.Contracts.Entities;

namespace CommonTest
{
    public class PhoneAreaTest
    {
        public static void Test()
        {
            CountryInfo thisCountry = CountryInfo.FromIsoCode("cn");
            StreamTablePhoneAreaDataProvider dataProvider = new StreamTablePhoneAreaDataProvider(@"D:\Work\Dev\Test\PhoneNumberArea\Data\data");
            PhoneAreaQuery query = PhoneAreaQuery.CreatePhoneAreaQuery(thisCountry, "0", dataProvider);
            PhoneArea area = query.GetPhoneArea("008613717674043");
            Console.WriteLine(area);
        }
    }
}
