using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Entities
{
    /// <summary>
    /// 国家名称
    /// </summary>
    public static class CountryNames
    {
        /// <summary>
        /// 阿富汗 (Afghanistan)
        /// </summary>
        [Country("Afghanistan", "AF", "AFG", "阿富汗", "93", "412", -3.5f)]
        public const string Afghanistan = "Afghanistan";

        /// <summary>
        /// 阿拉斯加 (Alaska(U.S.A))
        /// </summary>
        [Country("Alaska(U.S.A)", "", "", "阿拉斯加", "1907", "", 3f)]
        public const string Alaska = "Alaska(U.S.A)";

        /// <summary>
        /// 阿尔巴尼亚 (Albania)
        /// </summary>
        [Country("Albania", "AL", "ALB", "阿尔巴尼亚", "355", "276", -7f)]
        public const string Albania = "Albania";

        /// <summary>
        /// 阿尔及利亚 (Algeria)
        /// </summary>
        [Country("Algeria", "DZ", "DZA", "阿尔及利亚", "213", "603", -8f)]
        public const string Algeria = "Algeria";

        /// <summary>
        /// 安道尔 (Andorra)
        /// </summary>
        [Country("Andorra", "AD", "AND", "安道尔", "376", "213", -8f)]
        public const string Andorra = "Andorra";

        /// <summary>
        /// 安哥拉 (Angola)
        /// </summary>
        [Country("Angola", "AO", "AGO", "安哥拉", "244", "631", -7f)]
        public const string Angola = "Angola";

        /// <summary>
        /// 安圭拉 (Anguilla I.)
        /// </summary>
        [Country("Anguilla I.", "AI", "AIA", "安圭拉", "1264", "365", -12f)]
        public const string Anguilla = "Anguilla I.";

        /// <summary>
        /// 安提瓜和巴布达 (Antigua and Barbuda)
        /// </summary>
        [Country("Antigua and Barbuda", "AG", "ATG", "安提瓜和巴布达", "1268", "344", -12f)]
        public const string AntiguaAndBarbuda = "Antigua and Barbuda";

        /// <summary>
        /// 阿根廷 (Argentina)
        /// </summary>
        [Country("Argentina", "AR", "ARG", "阿根廷", "54", "722", -11f)]
        public const string Argentina = "Argentina";

        /// <summary>
        /// 亚美尼亚 (Armenia)
        /// </summary>
        [Country("Armenia", "AM", "ARM", "亚美尼亚", "374", "283", 0f)]
        public const string Armenia = "Armenia";

        /// <summary>
        /// 阿鲁巴 (Aruba I.)
        /// </summary>
        [Country("Aruba I.", "AW", "ABW", "阿鲁巴", "297", "363", -12f)]
        public const string Aruba = "Aruba I.";

        /// <summary>
        /// 阿森松(英) (Ascension)
        /// </summary>
        [Country("Ascension", "AC", "ASC", "阿森松(英)", "247", "", -8f)]
        public const string Ascension = "Ascension";

        /// <summary>
        /// 澳大利亚 (Australia)
        /// </summary>
        [Country("Australia", "AU", "AUS", "澳大利亚", "61", "505", 2f)]
        public const string Australia = "Australia";

        /// <summary>
        /// 奥地利 (Austria)
        /// </summary>
        [Country("Austria", "AT", "AUT", "奥地利", "43", "232", -7f)]
        public const string Austria = "Austria";

        /// <summary>
        /// 阿塞拜疆 (Azerbaijan)
        /// </summary>
        [Country("Azerbaijan", "AZ", "AZE", "阿塞拜疆", "994", "400", -5f)]
        public const string Azerbaijan = "Azerbaijan";

        /// <summary>
        /// 巴林 (Bahrain)
        /// </summary>
        [Country("Bahrain", "BH", "BHR", "巴林", "973", "426", -5f)]
        public const string Bahrain = "Bahrain";

        /// <summary>
        /// 孟加拉国 (Bangladesh)
        /// </summary>
        [Country("Bangladesh", "BD", "BGD", "孟加拉国", "880", "470", -2f)]
        public const string Bangladesh = "Bangladesh";

        /// <summary>
        /// 巴巴多斯 (Barbados)
        /// </summary>
        [Country("Barbados", "BB", "BRB", "巴巴多斯", "1246", "342", -12f)]
        public const string Barbados = "Barbados";

        /// <summary>
        /// 白俄罗斯 (Belarus)
        /// </summary>
        [Country("Belarus", "BY", "BLR", "白俄罗斯", "375", "257", -5f)]
        public const string Belarus = "Belarus";

        /// <summary>
        /// 比利时 (Belgium)
        /// </summary>
        [Country("Belgium", "BE", "BEL", "比利时", "32", "206", -7f)]
        public const string Belgium = "Belgium";

        /// <summary>
        /// 伯利兹 (Belize)
        /// </summary>
        [Country("Belize", "BZ", "BLZ", "伯利兹", "501", "702", -14f)]
        public const string Belize = "Belize";

        /// <summary>
        /// 贝宁 (Benin)
        /// </summary>
        [Country("Benin", "BJ", "BEN", "贝宁", "229", "616", -7f)]
        public const string Benin = "Benin";

        /// <summary>
        /// 百慕大 (Bermuda Is.)
        /// </summary>
        [Country("Bermuda Is.", "BM", "BMU", "百慕大", "1441", "350", -12f)]
        public const string Bermuda = "Bermuda Is.";

        /// <summary>
        /// 不丹 (Bhutan)
        /// </summary>
        [Country("Bhutan", "BT", "BTN", "不丹", "975", "402", 0f)]
        public const string Bhutan = "Bhutan";

        /// <summary>
        /// 玻利维亚 (Bolivia)
        /// </summary>
        [Country("Bolivia", "BO", "BOL", "玻利维亚", "591", "736", -12f)]
        public const string Bolivia = "Bolivia";

        /// <summary>
        /// 波黑 (Bosnia And Herzegovina)
        /// </summary>
        [Country("Bosnia And Herzegovina", "BA", "BIH", "波黑", "387", "218", 0f)]
        public const string BosniaAndHerzegovina = "Bosnia And Herzegovina";

        /// <summary>
        /// 博茨瓦纳 (Botswana)
        /// </summary>
        [Country("Botswana", "BW", "BWA", "博茨瓦纳", "267", "652", -6f)]
        public const string Botswana = "Botswana";

        /// <summary>
        /// 巴西 (Brazil)
        /// </summary>
        [Country("Brazil", "BR", "BRA", "巴西", "55", "724", -11f)]
        public const string Brazil = "Brazil";

        /// <summary>
        /// 保加利亚 (Bulgaria)
        /// </summary>
        [Country("Bulgaria", "BG", "BGR", "保加利亚", "359", "284", -6f)]
        public const string Bulgaria = "Bulgaria";

        /// <summary>
        /// 布基纳法索 (Burkina Faso)
        /// </summary>
        [Country("Burkina Faso", "BF", "BFA", "布基纳法索", "226", "613", -8f)]
        public const string BurkinaFaso = "Burkina Faso";

        /// <summary>
        /// 布隆迪 (Burundi)
        /// </summary>
        [Country("Burundi", "BI", "BDI", "布隆迪", "257", "642", -6f)]
        public const string Burundi = "Burundi";

        /// <summary>
        /// 喀麦隆 (Cameroon)
        /// </summary>
        [Country("Cameroon", "CM", "CMR", "喀麦隆", "237", "624", -7f)]
        public const string Cameroon = "Cameroon";

        /// <summary>
        /// 加拿大 (Canada)
        /// </summary>
        [Country("Canada", "CA", "CAN", "加拿大", "1", "302", -13f)]
        public const string Canada = "Canada";

        /// <summary>
        /// 加那利群岛 (Canaries Is.)
        /// </summary>
        [Country("Canaries Is.", "IC", "", "加那利群岛", "34", "", -8f)]
        public const string Canaries = "Canaries Is.";

        /// <summary>
        /// 佛得角 (Cape Verde)
        /// </summary>
        [Country("Cape Verde", "CV", "CPV", "佛得角", "238", "625", -9f)]
        public const string CapeVerde = "Cape Verde";

        /// <summary>
        /// 开曼群岛 (Cayman Is.)
        /// </summary>
        [Country("Cayman Is.", "KY", "CYM", "开曼群岛", "1345", "346", -13f)]
        public const string Cayman = "Cayman Is.";

        /// <summary>
        /// 中非 (Central Africa)
        /// </summary>
        [Country("Central Africa", "CF", "CAF", "中非", "236", "623", -7f)]
        public const string CentralAfrica = "Central Africa";

        /// <summary>
        /// 乍得 (Chad)
        /// </summary>
        [Country("Chad", "TD", "TCD", "乍得", "235", "622", -7f)]
        public const string Chad = "Chad";

        /// <summary>
        /// 智利 (Chile)
        /// </summary>
        [Country("Chile", "CL", "CHL", "智利", "56", "730", -12f)]
        public const string Chile = "Chile";

        /// <summary>
        /// 圣诞岛 (Christmas I.)
        /// </summary>
        [Country("Christmas I.", "CX", "CXR", "圣诞岛", "618", "", -1.3f)]
        public const string Christmas = "Christmas I.";

        /// <summary>
        /// 科科斯岛 (Cocos I.)
        /// </summary>
        [Country("Cocos I.", "CC", "CCK", "科科斯岛", "61891", "", -13f)]
        public const string Cocos = "Cocos I.";

        /// <summary>
        /// 哥伦比亚 (Colombia)
        /// </summary>
        [Country("Colombia", "CO", "COL", "哥伦比亚", "57", "732", 0f)]
        public const string Colombia = "Colombia";

        /// <summary>
        /// 巴哈马 (Commonwealth of The Bahamas)
        /// </summary>
        [Country("Commonwealth of The Bahamas", "BS", "BHS", "巴哈马", "1809", "364", 0f)]
        public const string CommonwealthOfTheBahamas = "Commonwealth of The Bahamas";

        /// <summary>
        /// 多米尼克 (Commonwealth of Dominica)
        /// </summary>
        [Country("Commonwealth of Dominica", "DM", "DMA", "多米尼克", "1809", "366", 0f)]
        public const string CommonwealthOfDominica = "Commonwealth of Dominica";

        /// <summary>
        /// 科摩罗 (Comoro)
        /// </summary>
        [Country("Comoro", "KM", "COM", "科摩罗", "269", "654", -5f)]
        public const string Comoro = "Comoro";

        /// <summary>
        /// 刚果（布） (Congo)
        /// </summary>
        [Country("Congo", "CG", "COG", "刚果（布）", "242", "629", -7f)]
        public const string Congo = "Congo";

        /// <summary>
        /// 库克群岛 (Cook IS.)
        /// </summary>
        [Country("Cook IS.", "CK", "COK", "库克群岛", "682", "548", -18.3f)]
        public const string Cook = "Cook IS.";

        /// <summary>
        /// 哥斯达黎加 (Costa Rica)
        /// </summary>
        [Country("Costa Rica", "CR", "CRI", "哥斯达黎加", "506", "712", -14f)]
        public const string CostaRica = "Costa Rica";

        /// <summary>
        /// 克罗地亚 (Croatia)
        /// </summary>
        [Country("Croatia", "HR", "HRV", "克罗地亚", "385", "219", -7f)]
        public const string Croatia = "Croatia";

        /// <summary>
        /// 古巴 (Cuba)
        /// </summary>
        [Country("Cuba", "CU", "CUB", "古巴", "53", "368", -13f)]
        public const string Cuba = "Cuba";

        /// <summary>
        /// 塞浦路斯 (Cyprus)
        /// </summary>
        [Country("Cyprus", "CY", "CYP", "塞浦路斯", "357", "280", -6f)]
        public const string Cyprus = "Cyprus";

        /// <summary>
        /// 捷克 (Czech)
        /// </summary>
        [Country("Czech", "CZ", "CZE", "捷克", "420", "230", -7f)]
        public const string Czech = "Czech";

        /// <summary>
        /// 丹麦 (Denmark)
        /// </summary>
        [Country("Denmark", "DK", "DNK", "丹麦", "45", "238", -7f)]
        public const string Denmark = "Denmark";

        /// <summary>
        /// 迪戈加西亚岛 (Diego Garcia I.)
        /// </summary>
        [Country("Diego Garcia I.", "DG", "DGA", "迪戈加西亚岛", "246", "", 0f)]
        public const string DiegoGarcia = "Diego Garcia I.";

        /// <summary>
        /// 吉布提 (Djibouti)
        /// </summary>
        [Country("Djibouti", "DJ", "DJI", "吉布提", "253", "638", -5f)]
        public const string Djibouti = "Djibouti";

        /// <summary>
        /// 多米尼加 (Dominican Rep.)
        /// </summary>
        [Country("Dominican Rep.", "DO", "DOM", "多米尼加", "1809", "370", -13f)]
        public const string DominicanRep = "Dominican Rep.";

        /// <summary>
        /// 厄瓜多尔 (Ecuador)
        /// </summary>
        [Country("Ecuador", "EC", "ECU", "厄瓜多尔", "593", "740", -13f)]
        public const string Ecuador = "Ecuador";

        /// <summary>
        /// 埃及 (Egypt)
        /// </summary>
        [Country("Egypt", "EG", "EGY", "埃及", "20", "602", -6f)]
        public const string Egypt = "Egypt";

        /// <summary>
        /// 萨尔瓦多 (El Salvador)
        /// </summary>
        [Country("El Salvador", "SV", "SLV", "萨尔瓦多", "503", "706", -14f)]
        public const string ElSalvador = "El Salvador";

        /// <summary>
        /// 赤道几内亚 (Equatorial Guinea)
        /// </summary>
        [Country("Equatorial Guinea", "GQ", "GNQ", "赤道几内亚", "240", "627", -8f)]
        public const string EquatorialGuinea = "Equatorial Guinea";

        /// <summary>
        /// 厄立特里亚 (Eritrea)
        /// </summary>
        [Country("Eritrea", "ER", "ERI", "厄立特里亚", "291", "657", 0f)]
        public const string Eritrea = "Eritrea";

        /// <summary>
        /// 爱沙尼亚 (Estonia)
        /// </summary>
        [Country("Estonia", "EE", "EST", "爱沙尼亚", "372", "248", -5f)]
        public const string Estonia = "Estonia";

        /// <summary>
        /// 埃塞俄比亚 (Ethiopia)
        /// </summary>
        [Country("Ethiopia", "ET", "ETH", "埃塞俄比亚", "251", "636", -5f)]
        public const string Ethiopia = "Ethiopia";

        /// <summary>
        /// 福克兰群岛（马尔维纳斯） (Falkland Is.)
        /// </summary>
        [Country("Falkland Is.", "FK", "FLK", "福克兰群岛（马尔维纳斯）", "500", "", -11f)]
        public const string Falkland = "Falkland Is.";

        /// <summary>
        /// 法罗群岛 (Faroe Is.)
        /// </summary>
        [Country("Faroe Is.", "FO", "FRO", "法罗群岛", "298", "288", 0f)]
        public const string Faroe = "Faroe Is.";

        /// <summary>
        /// 斐济 (Fiji)
        /// </summary>
        [Country("Fiji", "FJ", "FJI", "斐济", "679", "542", 4f)]
        public const string Fiji = "Fiji";

        /// <summary>
        /// 芬兰 (Finland)
        /// </summary>
        [Country("Finland", "FI", "FIN", "芬兰", "358", "244", -6f)]
        public const string Finland = "Finland";

        /// <summary>
        /// 法国 (France)
        /// </summary>
        [Country("France", "FR", "FRA", "法国", "33", "208", -8f)]
        public const string France = "France";

        /// <summary>
        /// 法属圭亚那 (French Guiana)
        /// </summary>
        [Country("French Guiana", "GF", "GUF", "法属圭亚那", "594", "", -12f)]
        public const string FrenchGuiana = "French Guiana";

        /// <summary>
        /// 法属波利尼西亚 (French Polynesia)
        /// </summary>
        [Country("French Polynesia", "PF", "PYF", "法属波利尼西亚", "689", "547", 3f)]
        public const string FrenchPolynesia = "French Polynesia";

        /// <summary>
        /// 加蓬 (Gabon)
        /// </summary>
        [Country("Gabon", "GA", "GAB", "加蓬", "241", "628", -7f)]
        public const string Gabon = "Gabon";

        /// <summary>
        /// 冈比亚 (Gambia)
        /// </summary>
        [Country("Gambia", "GM", "GMB", "冈比亚", "220", "607", -8f)]
        public const string Gambia = "Gambia";

        /// <summary>
        /// 格鲁吉亚 (Georgia)
        /// </summary>
        [Country("Georgia", "GE", "GEO", "格鲁吉亚", "995", "282", 0f)]
        public const string Georgia = "Georgia";

        /// <summary>
        /// 德国 (Germany)
        /// </summary>
        [Country("Germany", "DE", "DEU", "德国", "49", "262", -7f)]
        public const string Germany = "Germany";

        /// <summary>
        /// 加纳 (Ghana)
        /// </summary>
        [Country("Ghana", "GH", "GHA", "加纳", "233", "620", -8f)]
        public const string Ghana = "Ghana";

        /// <summary>
        /// 直布罗陀 (Gibraltar)
        /// </summary>
        [Country("Gibraltar", "GI", "GIB", "直布罗陀", "350", "266", -8f)]
        public const string Gibraltar = "Gibraltar";

        /// <summary>
        /// 希腊 (Greece)
        /// </summary>
        [Country("Greece", "GR", "GRC", "希腊", "30", "202", -6f)]
        public const string Greece = "Greece";

        /// <summary>
        /// 格陵兰 (Greenland)
        /// </summary>
        [Country("Greenland", "GL", "GRL", "格陵兰", "299", "290", 0f)]
        public const string Greenland = "Greenland";

        /// <summary>
        /// 格林纳达 (Grenada)
        /// </summary>
        [Country("Grenada", "GD", "GRD", "格林纳达", "1809", "352", -14f)]
        public const string Grenada = "Grenada";

        /// <summary>
        /// 瓜德罗普 (Guadeloupe I.)
        /// </summary>
        [Country("Guadeloupe I.", "GP", "GLP", "瓜德罗普", "590", "340", 0f)]
        public const string Guadeloupe = "Guadeloupe I.";

        /// <summary>
        /// 关岛(美) (Guam)
        /// </summary>
        [Country("Guam", "GU", "GUM", "关岛(美)", "1671", "310", 2f)]
        public const string Guam = "Guam";

        /// <summary>
        /// 危地马拉 (Guatemala)
        /// </summary>
        [Country("Guatemala", "GT", "GTM", "危地马拉", "502", "704", -14f)]
        public const string Guatemala = "Guatemala";

        /// <summary>
        /// 几内亚 (Guinea)
        /// </summary>
        [Country("Guinea", "GN", "GIN", "几内亚", "224", "611", -8f)]
        public const string Guinea = "Guinea";

        /// <summary>
        /// 几内亚比绍 (Guinea-Bissau)
        /// </summary>
        [Country("Guinea-Bissau", "GW", "GNB", "几内亚比绍", "245", "632", 0f)]
        public const string GuineaBissau = "Guinea-Bissau";

        /// <summary>
        /// 圭亚那 (Guyana)
        /// </summary>
        [Country("Guyana", "GY", "GUY", "圭亚那", "592", "738", -11f)]
        public const string Guyana = "Guyana";

        /// <summary>
        /// 海地 (Haiti)
        /// </summary>
        [Country("Haiti", "HT", "HTI", "海地", "509", "372", -13f)]
        public const string Haiti = "Haiti";

        /// <summary>
        /// 夏威夷 (Hawaii)
        /// </summary>
        [Country("Hawaii", "", "", "夏威夷", "1808", "", 0f)]
        public const string Hawaii = "Hawaii";

        /// <summary>
        /// 洪都拉斯 (Honduras)
        /// </summary>
        [Country("Honduras", "HN", "HND", "洪都拉斯", "504", "708", -14f)]
        public const string Honduras = "Honduras";

        /// <summary>
        /// 匈牙利 (Hungary)
        /// </summary>
        [Country("Hungary", "HU", "HUN", "匈牙利", "36", "216", -7f)]
        public const string Hungary = "Hungary";

        /// <summary>
        /// 冰岛 (Iceland)
        /// </summary>
        [Country("Iceland", "IS", "ISL", "冰岛", "354", "274", -9f)]
        public const string Iceland = "Iceland";

        /// <summary>
        /// 印度 (India)
        /// </summary>
        [Country("India", "IN", "IND", "印度", "91", "404", -2.3f)]
        public const string India = "India";

        /// <summary>
        /// 印度尼西亚 (Indonesia)
        /// </summary>
        [Country("Indonesia", "RI", "IDN", "印度尼西亚", "62", "", -0.3f)]
        public const string Indonesia = "Indonesia";

        /// <summary>
        /// 伊朗 (Iran)
        /// </summary>
        [Country("Iran", "IR", "IRN", "伊朗", "98", "432", -4.3f)]
        public const string Iran = "Iran";

        /// <summary>
        /// 伊拉克 (Iraq)
        /// </summary>
        [Country("Iraq", "IQ", "IRQ", "伊拉克", "964", "418", -5f)]
        public const string Iraq = "Iraq";

        /// <summary>
        /// 爱尔兰 (Ireland)
        /// </summary>
        [Country("Ireland", "IE", "IRL", "爱尔兰", "353", "272", -8f)]
        public const string Ireland = "Ireland";

        /// <summary>
        /// 以色列 (Israel)
        /// </summary>
        [Country("Israel", "IL", "ISR", "以色列", "972", "425", -6f)]
        public const string Israel = "Israel";

        /// <summary>
        /// 意大利 (Italy)
        /// </summary>
        [Country("Italy", "IT", "ITA", "意大利", "39", "222", -7f)]
        public const string Italy = "Italy";

        /// <summary>
        /// 科特迪瓦 (Ivory Coast)
        /// </summary>
        [Country("Ivory Coast", "CI", "CIV", "科特迪瓦", "225", "612", -8f)]
        public const string IvoryCoast = "Ivory Coast";

        /// <summary>
        /// 牙买加 (Jamaica)
        /// </summary>
        [Country("Jamaica", "JM", "JAM", "牙买加", "1876", "338", -12f)]
        public const string Jamaica = "Jamaica";

        /// <summary>
        /// 日本 (Japan)
        /// </summary>
        [Country("Japan", "JP", "JPN", "日本", "81", "440", 1f)]
        public const string Japan = "Japan";

        /// <summary>
        /// 约旦 (Jordan)
        /// </summary>
        [Country("Jordan", "JO", "JOR", "约旦", "962", "416", -6f)]
        public const string Jordan = "Jordan";

        /// <summary>
        /// 柬埔寨 (Kampuchea)
        /// </summary>
        [Country("Kampuchea", "KH", "KHM", "柬埔寨", "855", "456", -1f)]
        public const string Kampuchea = "Kampuchea";

        /// <summary>
        /// 哈萨克斯坦 (Kazakhstan)
        /// </summary>
        [Country("Kazakhstan", "KZ", "KAZ", "哈萨克斯坦", "7", "401", -5f)]
        public const string Kazakhstan = "Kazakhstan";

        /// <summary>
        /// 肯尼亚 (Kenya)
        /// </summary>
        [Country("Kenya", "KE", "KEN", "肯尼亚", "254", "639", -5f)]
        public const string Kenya = "Kenya";

        /// <summary>
        /// 基里巴斯 (Kiribati)
        /// </summary>
        [Country("Kiribati", "KI", "KIR", "基里巴斯", "686", "545", 4f)]
        public const string Kiribati = "Kiribati";

        /// <summary>
        /// 朝鲜 (Korea(dpr of))
        /// </summary>
        [Country("Korea(dpr of)", "KP", "PRK", "朝鲜", "85", "467", 1f)]
        public const string Korea_Dpr = "Korea(dpr of)";

        /// <summary>
        /// 韩国 (Korea(republic of))
        /// </summary>
        [Country("Korea(republic of)", "KR", "KOR", "韩国", "82", "450", 1f)]
        public const string Korea_Republic = "Korea(republic of)";

        /// <summary>
        /// 科威特 (Kuwait)
        /// </summary>
        [Country("Kuwait", "KW", "KWT", "科威特", "965", "419", -5f)]
        public const string Kuwait = "Kuwait";

        /// <summary>
        /// 吉尔吉斯斯坦 (Kyrgyzstan)
        /// </summary>
        [Country("Kyrgyzstan", "KG", "KGZ", "吉尔吉斯斯坦", "996", "437", -5f)]
        public const string Kyrgyzstan = "Kyrgyzstan";

        /// <summary>
        /// 老挝 (Laos)
        /// </summary>
        [Country("Laos", "LA", "LAO", "老挝", "856", "457", -1f)]
        public const string Laos = "Laos";

        /// <summary>
        /// 拉脱维亚 (Latvia)
        /// </summary>
        [Country("Latvia", "LV", "LVA", "拉脱维亚", "371", "247", -5f)]
        public const string Latvia = "Latvia";

        /// <summary>
        /// 黎巴嫩 (Lebanon)
        /// </summary>
        [Country("Lebanon", "LB", "LBN", "黎巴嫩", "961", "415", -6f)]
        public const string Lebanon = "Lebanon";

        /// <summary>
        /// 莱索托 (Lesotho)
        /// </summary>
        [Country("Lesotho", "LS", "LSO", "莱索托", "266", "651", -6f)]
        public const string Lesotho = "Lesotho";

        /// <summary>
        /// 利比里亚 (Liberia)
        /// </summary>
        [Country("Liberia", "LR", "LBR", "利比里亚", "231", "618", -8f)]
        public const string Liberia = "Liberia";

        /// <summary>
        /// 利比亚 (Libya)
        /// </summary>
        [Country("Libya", "LY", "LBY", "利比亚", "218", "606", -6f)]
        public const string Libya = "Libya";

        /// <summary>
        /// 列支敦士登 (Liechtenstein)
        /// </summary>
        [Country("Liechtenstein", "LI", "LIE", "列支敦士登", "423", "295", -7f)]
        public const string Liechtenstein = "Liechtenstein";

        /// <summary>
        /// 立陶宛 (Lithuania)
        /// </summary>
        [Country("Lithuania", "LT", "LTU", "立陶宛", "370", "246", -5f)]
        public const string Lithuania = "Lithuania";

        /// <summary>
        /// 卢森堡 (Luxembourg)
        /// </summary>
        [Country("Luxembourg", "LU", "LUX", "卢森堡", "352", "270", -7f)]
        public const string Luxembourg = "Luxembourg";

        /// <summary>
        /// 马其顿 (Macedonia)
        /// </summary>
        [Country("Macedonia", "MK", "MKD", "马其顿", "389", "294", 0f)]
        public const string Macedonia = "Macedonia";

        /// <summary>
        /// 马达加斯加 (Madagascar)
        /// </summary>
        [Country("Madagascar", "MG", "MDG", "马达加斯加", "261", "646", -5f)]
        public const string Madagascar = "Madagascar";

        /// <summary>
        /// 马拉维 (Malawi)
        /// </summary>
        [Country("Malawi", "MW", "MWI", "马拉维", "265", "650", -6f)]
        public const string Malawi = "Malawi";

        /// <summary>
        /// 马来西亚 (Malaysia)
        /// </summary>
        [Country("Malaysia", "MY", "MYS", "马来西亚", "60", "502", -0.3f)]
        public const string Malaysia = "Malaysia";

        /// <summary>
        /// 马尔代夫 (Maldive)
        /// </summary>
        [Country("Maldive", "MV", "MDV", "马尔代夫", "960", "472", -2.3f)]
        public const string Maldive = "Maldive";

        /// <summary>
        /// 马里 (Mali)
        /// </summary>
        [Country("Mali", "ML", "MLI", "马里", "223", "610", -8f)]
        public const string Mali = "Mali";

        /// <summary>
        /// 马耳他 (Malta)
        /// </summary>
        [Country("Malta", "MT", "MLT", "马耳他", "356", "278", -7f)]
        public const string Malta = "Malta";

        /// <summary>
        /// 马里亚纳群岛 (Mariana Is.)
        /// </summary>
        [Country("Mariana Is.", "MP", "MNP", "马里亚纳群岛", "670", "", 2f)]
        public const string Mariana = "Mariana Is.";

        /// <summary>
        /// 马绍尔群岛 (Marshall Is.)
        /// </summary>
        [Country("Marshall Is.", "MH", "MHL", "马绍尔群岛", "692", "", 4f)]
        public const string Marshall = "Marshall Is.";

        /// <summary>
        /// 马提尼克 (Martinique)
        /// </summary>
        [Country("Martinique", "MQ", "MTQ", "马提尼克", "596", "340", -12f)]
        public const string Martinique = "Martinique";

        /// <summary>
        /// 毛里塔尼亚 (Mauritania)
        /// </summary>
        [Country("Mauritania", "MR", "MRT", "毛里塔尼亚", "222", "609", 0f)]
        public const string Mauritania = "Mauritania";

        /// <summary>
        /// 毛里求斯 (Mauritius)
        /// </summary>
        [Country("Mauritius", "MU", "MUS", "毛里求斯", "230", "617", -4f)]
        public const string Mauritius = "Mauritius";

        /// <summary>
        /// 马约特 (Mayotte I.)
        /// </summary>
        [Country("Mayotte I.", "YT", "MYT", "马约特", "269", "", 0f)]
        public const string Mayotte = "Mayotte I.";

        /// <summary>
        /// 墨西哥 (Mexico)
        /// </summary>
        [Country("Mexico", "MX", "MEX", "墨西哥", "52", "334", -15f)]
        public const string Mexico = "Mexico";

        /// <summary>
        /// 密克罗尼西亚 (Micronesia)
        /// </summary>
        [Country("Micronesia", "FM", "FSM", "密克罗尼西亚", "691", "550", 1f)]
        public const string Micronesia = "Micronesia";

        /// <summary>
        /// 中途岛(美) (Midway I.)
        /// </summary>
        [Country("Midway I.", "", "", "中途岛(美)", "1808", "", -19f)]
        public const string Midway = "Midway I.";

        /// <summary>
        /// 摩尔多瓦 (Moldova)
        /// </summary>
        [Country("Moldova", "MD", "MDA", "摩尔多瓦", "373", "259", -5f)]
        public const string Moldova = "Moldova";

        /// <summary>
        /// 摩纳哥 (Monaco)
        /// </summary>
        [Country("Monaco", "MC", "MCO", "摩纳哥", "377", "212", -7f)]
        public const string Monaco = "Monaco";

        /// <summary>
        /// 蒙古 (Mongolia)
        /// </summary>
        [Country("Mongolia", "MN", "MNG", "蒙古", "976", "428", 0f)]
        public const string Mongolia = "Mongolia";

        /// <summary>
        /// 蒙特塞拉特 (Montserrat I.)
        /// </summary>
        [Country("Montserrat I.", "MS", "MSR", "蒙特塞拉特", "1664", "", -12f)]
        public const string Montserrat = "Montserrat I.";

        /// <summary>
        /// 摩洛哥 (Morocco)
        /// </summary>
        [Country("Morocco", "MA", "MAR", "摩洛哥", "212", "604", -6f)]
        public const string Morocco = "Morocco";

        /// <summary>
        /// 莫桑比克 (Mozambique)
        /// </summary>
        [Country("Mozambique", "MZ", "MOZ", "莫桑比克", "258", "643", -6f)]
        public const string Mozambique = "Mozambique";

        /// <summary>
        /// 缅甸 (Myanmar)
        /// </summary>
        [Country("Myanmar", "MM", "MMR", "缅甸", "95", "414", -1.3f)]
        public const string Myanmar = "Myanmar";

        /// <summary>
        /// 纳米比亚 (Namibia)
        /// </summary>
        [Country("Namibia", "NA", "NAM", "纳米比亚", "264", "649", -7f)]
        public const string Namibia = "Namibia";

        /// <summary>
        /// 瑙鲁 (Nauru)
        /// </summary>
        [Country("Nauru", "NR", "NRU", "瑙鲁", "674", "536", 4f)]
        public const string Nauru = "Nauru";

        /// <summary>
        /// 尼泊尔 (Nepal)
        /// </summary>
        [Country("Nepal", "NP", "NPL", "尼泊尔", "977", "429", -2.3f)]
        public const string Nepal = "Nepal";

        /// <summary>
        /// 荷兰 (Netherlands)
        /// </summary>
        [Country("Netherlands", "NL", "NLD", "荷兰", "31", "204", -7f)]
        public const string Netherlands = "Netherlands";

        /// <summary>
        /// 荷属安的列斯 (Netherlandsantilles Is.)
        /// </summary>
        [Country("Netherlandsantilles Is.", "AN", "ANT", "荷属安的列斯", "599", "362", -12f)]
        public const string Netherlandsantilles = "Netherlandsantilles Is.";

        /// <summary>
        /// 新喀里多尼亚 (New Caledonia Is.)
        /// </summary>
        [Country("New Caledonia Is.", "NC", "NCL", "新喀里多尼亚", "687", "546", 3f)]
        public const string NewCaledonia = "New Caledonia Is.";

        /// <summary>
        /// 新西兰 (New Zealand)
        /// </summary>
        [Country("New Zealand", "NZ", "NZL", "新西兰", "64", "530", 4f)]
        public const string NewZealand = "New Zealand";

        /// <summary>
        /// 尼加拉瓜 (Nicaragua)
        /// </summary>
        [Country("Nicaragua", "NI", "NIC", "尼加拉瓜", "505", "710", -14f)]
        public const string Nicaragua = "Nicaragua";

        /// <summary>
        /// 尼日尔 (Niger)
        /// </summary>
        [Country("Niger", "NE", "NER", "尼日尔", "227", "614", -8f)]
        public const string Niger = "Niger";

        /// <summary>
        /// 尼日利亚 (Nigeria)
        /// </summary>
        [Country("Nigeria", "NG", "NGA", "尼日利亚", "234", "621", -7f)]
        public const string Nigeria = "Nigeria";

        /// <summary>
        /// 纽埃 (Niue I.)
        /// </summary>
        [Country("Niue I.", "NU", "NIU", "纽埃", "683", "555", -19f)]
        public const string Niue = "Niue I.";

        /// <summary>
        /// 诺福克岛 (Norfolk I.)
        /// </summary>
        [Country("Norfolk I.", "NF", "NFK", "诺福克岛", "6723", "505", 3.3f)]
        public const string Norfolk = "Norfolk I.";

        /// <summary>
        /// 挪威 (Norway)
        /// </summary>
        [Country("Norway", "NO", "NOR", "挪威", "47", "242", -7f)]
        public const string Norway = "Norway";

        /// <summary>
        /// 阿曼 (Oman)
        /// </summary>
        [Country("Oman", "OM", "OMN", "阿曼", "968", "422", -4f)]
        public const string Oman = "Oman";

        /// <summary>
        /// 帕劳 (Palau)
        /// </summary>
        [Country("Palau", "PW", "PLW", "帕劳", "680", "552", 0f)]
        public const string Palau = "Palau";

        /// <summary>
        /// 巴拿马 (Panama)
        /// </summary>
        [Country("Panama", "PA", "PAN", "巴拿马", "507", "714", -13f)]
        public const string Panama = "Panama";

        /// <summary>
        /// 巴布亚新几内亚 (Papua New Guinea)
        /// </summary>
        [Country("Papua New Guinea", "PG", "PNG", "巴布亚新几内亚", "675", "537", 2f)]
        public const string PapuaNewGuinea = "Papua New Guinea";

        /// <summary>
        /// 巴拉圭 (Paraguay)
        /// </summary>
        [Country("Paraguay", "PY", "PRY", "巴拉圭", "595", "744", -12f)]
        public const string Paraguay = "Paraguay";

        /// <summary>
        /// 秘鲁 (Peru)
        /// </summary>
        [Country("Peru", "PE", "PER", "秘鲁", "51", "716", -13f)]
        public const string Peru = "Peru";

        /// <summary>
        /// 菲律宾 (Philippines)
        /// </summary>
        [Country("Philippines", "PH", "PHL", "菲律宾", "63", "515", 0f)]
        public const string Philippines = "Philippines";

        /// <summary>
        /// 波兰 (Poland)
        /// </summary>
        [Country("Poland", "PL", "POL", "波兰", "48", "260", -7f)]
        public const string Poland = "Poland";

        /// <summary>
        /// 葡萄牙 (Portugal)
        /// </summary>
        [Country("Portugal", "PT", "PRT", "葡萄牙", "351", "268", -8f)]
        public const string Portugal = "Portugal";

        /// <summary>
        /// 巴基斯坦 (Pakistan)
        /// </summary>
        [Country("Pakistan", "PK", "PAK", "巴基斯坦", "92", "410", -2.3f)]
        public const string Pakistan = "Pakistan";

        /// <summary>
        /// 波多黎各(美) (Puerto Rico)
        /// </summary>
        [Country("Puerto Rico", "", "", "波多黎各(美)", "1787", "", -12f)]
        public const string PuertoRico = "Puerto Rico";

        /// <summary>
        /// 卡塔尔 (Qatar)
        /// </summary>
        [Country("Qatar", "QA", "QAT", "卡塔尔", "974", "427", -5f)]
        public const string Qatar = "Qatar";

        /// <summary>
        /// 留尼汪 (Reunion I.)
        /// </summary>
        [Country("Reunion I.", "RE", "REU", "留尼汪", "262", "647", -4f)]
        public const string Reunion = "Reunion I.";

        /// <summary>
        /// 罗马尼亚 (Romania)
        /// </summary>
        [Country("Romania", "RO", "ROU", "罗马尼亚", "40", "226", -6f)]
        public const string Romania = "Romania";

        /// <summary>
        /// 俄罗斯 (Russia)
        /// </summary>
        [Country("Russia", "RU", "RUS", "俄罗斯", "7", "250", -5f)]
        public const string Russia = "Russia";

        /// <summary>
        /// 卢旺达 (Rwanda)
        /// </summary>
        [Country("Rwanda", "RW", "RWA", "卢旺达", "250", "635", -6f)]
        public const string Rwanda = "Rwanda";

        /// <summary>
        /// 美属萨摩亚 (Samoa,Eastern)
        /// </summary>
        [Country("Samoa,Eastern", "AS", "ASM", "美属萨摩亚", "684", "544", -19f)]
        public const string SamoaEastern = "Samoa,Eastern";

        /// <summary>
        /// 萨摩亚 (Samoa,Western)
        /// </summary>
        [Country("Samoa,Western", "WS", "WSM", "萨摩亚", "685", "549", -19f)]
        public const string SamoaWestern = "Samoa,Western";

        /// <summary>
        /// 圣马力诺 (San.Marino)
        /// </summary>
        [Country("San.Marino", "SM", "SMR", "圣马力诺", "378", "292", -7f)]
        public const string SanMarino = "San.Marino";

        /// <summary>
        /// 圣皮埃尔和密克隆 (San.Pierre And Miquelon I.)
        /// </summary>
        [Country("San.Pierre And Miquelon I.", "PM", "SPM", "圣皮埃尔和密克隆", "508", "308", -2f)]
        public const string SanPierreAndMiquelon = "San.Pierre And Miquelon I.";

        /// <summary>
        /// 圣多美和普林西比 (San.Tome And Principe)
        /// </summary>
        [Country("San.Tome And Principe", "ST", "STP", "圣多美和普林西比", "239", "626", -8f)]
        public const string SanTomeAndPrincipe = "San.Tome And Principe";

        /// <summary>
        /// 沙特阿拉伯 (Saudi Arabia)
        /// </summary>
        [Country("Saudi Arabia", "SA", "SAU", "沙特阿拉伯", "966", "420", -5f)]
        public const string SaudiArabia = "Saudi Arabia";

        /// <summary>
        /// 塞内加尔 (Senegal)
        /// </summary>
        [Country("Senegal", "SN", "SEN", "塞内加尔", "221", "608", -8f)]
        public const string Senegal = "Senegal";

        /// <summary>
        /// 塞舌尔 (Seychelles)
        /// </summary>
        [Country("Seychelles", "SC", "SYC", "塞舌尔", "248", "633", -4f)]
        public const string Seychelles = "Seychelles";

        /// <summary>
        /// 新加坡 (Singapore)
        /// </summary>
        [Country("Singapore", "SG", "SGP", "新加坡", "65", "525", 0.3f)]
        public const string Singapore = "Singapore";

        /// <summary>
        /// 斯洛伐克 (Slovak)
        /// </summary>
        [Country("Slovak", "SK", "SVK", "斯洛伐克", "421", "231", -7f)]
        public const string Slovak = "Slovak";

        /// <summary>
        /// 斯洛文尼亚 (Slovenia)
        /// </summary>
        [Country("Slovenia", "SI", "SVN", "斯洛文尼亚", "386", "293", -7f)]
        public const string Slovenia = "Slovenia";

        /// <summary>
        /// 所罗门群岛 (Solomon Is.)
        /// </summary>
        [Country("Solomon Is.", "SB", "SLB", "所罗门群岛", "677", "540", 3f)]
        public const string Solomon = "Solomon Is.";

        /// <summary>
        /// 索马里 (Somali)
        /// </summary>
        [Country("Somali", "SO", "SOM", "索马里", "252", "637", -5f)]
        public const string Somali = "Somali";

        /// <summary>
        /// 南非 (South Africa)
        /// </summary>
        [Country("South Africa", "ZA", "ZAF", "南非", "27", "655", -6f)]
        public const string SouthAfrica = "South Africa";

        /// <summary>
        /// 西班牙 (Spain)
        /// </summary>
        [Country("Spain", "ES", "ESP", "西班牙", "34", "214", -8f)]
        public const string Spain = "Spain";

        /// <summary>
        /// 斯里兰卡 (Sri Lanka)
        /// </summary>
        [Country("Sri Lanka", "LK", "LKA", "斯里兰卡", "94", "413", 0f)]
        public const string SriLanka = "Sri Lanka";

        /// <summary>
        /// 波多黎各 (St.Christopher and Nevis)
        /// </summary>
        [Country("St.Christopher and Nevis", "PR", "PRI", "波多黎各", "1809", "330", -12.3f)]
        public const string StChristopherAndNevis = "St.Christopher and Nevis";

        /// <summary>
        /// 圣赫勒拿 (St.Helena)
        /// </summary>
        [Country("St.Helena", "SH", "SHN", "圣赫勒拿", "290", "", -8f)]
        public const string StHelena = "St.Helena";

        /// <summary>
        /// 圣卢西亚 (St.Lucia)
        /// </summary>
        [Country("St.Lucia", "LC", "LCA", "圣卢西亚", "1758", "358", -12f)]
        public const string StLucia = "St.Lucia";

        /// <summary>
        /// 圣文森特和格林纳丁斯 (St.Vincent I.)
        /// </summary>
        [Country("St.Vincent I.", "VC", "VCT", "圣文森特和格林纳丁斯", "1784", "360", -12f)]
        public const string StVincent = "St.Vincent I.";

        /// <summary>
        /// 苏丹 (Sudan)
        /// </summary>
        [Country("Sudan", "SD", "SDN", "苏丹", "249", "634", -6f)]
        public const string Sudan = "Sudan";

        /// <summary>
        /// 苏里南 (Suriname)
        /// </summary>
        [Country("Suriname", "SR", "SUR", "苏里南", "597", "746", -11.3f)]
        public const string Suriname = "Suriname";

        /// <summary>
        /// 斯威士兰 (Swaziland)
        /// </summary>
        [Country("Swaziland", "SZ", "SWZ", "斯威士兰", "268", "653", -6f)]
        public const string Swaziland = "Swaziland";

        /// <summary>
        /// 瑞典 (Sweden)
        /// </summary>
        [Country("Sweden", "SE", "SWE", "瑞典", "46", "240", -7f)]
        public const string Sweden = "Sweden";

        /// <summary>
        /// 瑞士 (Switzerland)
        /// </summary>
        [Country("Switzerland", "CH", "CHE", "瑞士", "41", "228", -7f)]
        public const string Switzerland = "Switzerland";

        /// <summary>
        /// 叙利亚 (Syria)
        /// </summary>
        [Country("Syria", "SY", "SYR", "叙利亚", "963", "417", -6f)]
        public const string Syria = "Syria";

        /// <summary>
        /// 塔吉克斯坦 (Tajikistan)
        /// </summary>
        [Country("Tajikistan", "TJ", "TJK", "塔吉克斯坦", "992", "436", -5f)]
        public const string Tajikistan = "Tajikistan";

        /// <summary>
        /// 坦桑尼亚 (Tanzania)
        /// </summary>
        [Country("Tanzania", "TZ", "TZA", "坦桑尼亚", "255", "640", -5f)]
        public const string Tanzania = "Tanzania";

        /// <summary>
        /// 泰国 (Thailand)
        /// </summary>
        [Country("Thailand", "TH", "THA", "泰国", "66", "520", -1f)]
        public const string Thailand = "Thailand";

        /// <summary>
        /// 阿拉伯联合酋长国 (The United Arab Emirates)
        /// </summary>
        [Country("The United Arab Emirates", "AE", "ARE", "阿拉伯联合酋长国", "971", "424", -5f)]
        public const string TheUnitedArabEmirates = "The United Arab Emirates";

        /// <summary>
        /// 多哥 (Togo)
        /// </summary>
        [Country("Togo", "TG", "TGO", "多哥", "228", "615", -8f)]
        public const string Togo = "Togo";

        /// <summary>
        /// 托克劳 (Tokelau Is.)
        /// </summary>
        [Country("Tokelau Is.", "TK", "TKL", "托克劳", "690", "", -19f)]
        public const string Tokelau = "Tokelau Is.";

        /// <summary>
        /// 汤加 (Tonga)
        /// </summary>
        [Country("Tonga", "TO", "TON", "汤加", "676", "539", 5f)]
        public const string Tonga = "Tonga";

        /// <summary>
        /// 特立尼达和多巴哥 (Trinidad and Tobago)
        /// </summary>
        [Country("Trinidad and Tobago", "TT", "TTO", "特立尼达和多巴哥", "1809", "374", -12f)]
        public const string TrinidadAndTobago = "Trinidad and Tobago";

        /// <summary>
        /// 突尼斯 (Tunisia)
        /// </summary>
        [Country("Tunisia", "TN", "TUN", "突尼斯", "216", "605", -7f)]
        public const string Tunisia = "Tunisia";

        /// <summary>
        /// 土耳其 (Turkey)
        /// </summary>
        [Country("Turkey", "TR", "TUR", "土耳其", "90", "286", -6f)]
        public const string Turkey = "Turkey";

        /// <summary>
        /// 土库曼斯坦 (Turkmenistan)
        /// </summary>
        [Country("Turkmenistan", "TM", "TKM", "土库曼斯坦", "993", "438", -5f)]
        public const string Turkmenistan = "Turkmenistan";

        /// <summary>
        /// 特克斯和凯科斯群岛 (Turks and Caicos Is.)
        /// </summary>
        [Country("Turks and Caicos Is.", "TC", "TCA", "特克斯和凯科斯群岛", "1809", "376", -13f)]
        public const string TurksAndCaicos = "Turks and Caicos Is.";

        /// <summary>
        /// 图瓦卢 (Tuvalu)
        /// </summary>
        [Country("Tuvalu", "TV", "TUV", "图瓦卢", "688", "553", 4f)]
        public const string Tuvalu = "Tuvalu";

        /// <summary>
        /// 美国 (U.S.A)
        /// </summary>
        [Country("U.S.A", "US", "USA", "美国", "1", "310", -13f)]
        public const string USA = "U.S.A";

        /// <summary>
        /// 乌干达 (Uganda)
        /// </summary>
        [Country("Uganda", "UG", "UGA", "乌干达", "256", "641", -5f)]
        public const string Uganda = "Uganda";

        /// <summary>
        /// 乌克兰 (Ukraine)
        /// </summary>
        [Country("Ukraine", "UA", "UKR", "乌克兰", "380", "255", -5f)]
        public const string Ukraine = "Ukraine";

        /// <summary>
        /// 英国 (United Kingdom)
        /// </summary>
        [Country("United Kingdom", "GB", "GBR", "英国", "44", "234", -8f)]
        public const string UnitedKingdom = "United Kingdom";

        /// <summary>
        /// 乌拉圭 (Uruguay)
        /// </summary>
        [Country("Uruguay", "UY", "URY", "乌拉圭", "598", "748", -10.3f)]
        public const string Uruguay = "Uruguay";

        /// <summary>
        /// 乌兹别克斯坦 (Uzbekistan)
        /// </summary>
        [Country("Uzbekistan", "UZ", "UZB", "乌兹别克斯坦", "998", "434", -5f)]
        public const string Uzbekistan = "Uzbekistan";

        /// <summary>
        /// 瓦努阿图 (Vanuatu)
        /// </summary>
        [Country("Vanuatu", "VU", "VUT", "瓦努阿图", "678", "541", 3f)]
        public const string Vanuatu = "Vanuatu";

        /// <summary>
        /// 梵蒂冈 (Vatican)
        /// </summary>
        [Country("Vatican", "VA", "VAT", "梵蒂冈", "379", "225", -7f)]
        public const string Vatican = "Vatican";

        /// <summary>
        /// 委内瑞拉 (Venezuela)
        /// </summary>
        [Country("Venezuela", "VE", "VEN", "委内瑞拉", "58", "734", -12.3f)]
        public const string Venezuela = "Venezuela";

        /// <summary>
        /// 越南 (Vietnam)
        /// </summary>
        [Country("Vietnam", "VN", "VNM", "越南", "84", "452", -1f)]
        public const string Vietnam = "Vietnam";

        /// <summary>
        /// 维尔京群岛(英) (Virgin Is.)
        /// </summary>
        [Country("Virgin Is.", "VG", "VGB", "维尔京群岛(英)", "1809", "348", -12f)]
        public const string Virgin = "Virgin Is.";

        /// <summary>
        /// 维尔京群岛和圣罗克伊 (Virgin Is. and St.Croix I.)
        /// </summary>
        [Country("Virgin Is. and St.Croix I.", "VI", "VIR", "维尔京群岛和圣罗克伊", "1809", "", 0f)]
        public const string VirginAndStCroix = "Virgin Is. and St.Croix I.";

        /// <summary>
        /// 威克岛(美) (Wake I.)
        /// </summary>
        [Country("Wake I.", "UM", "UMI", "威克岛(美)", "1808", "", 4f)]
        public const string Wake = "Wake I.";

        /// <summary>
        /// 瓦利斯和富图纳群岛 (Wallis And Futuna Is.)
        /// </summary>
        [Country("Wallis And Futuna Is.", "WF", "WLF", "瓦利斯和富图纳群岛", "681", "", 4f)]
        public const string WallisAndFutuna = "Wallis And Futuna Is.";

        /// <summary>
        /// 西撒哈拉 (Western sahara)
        /// </summary>
        [Country("Western sahara", "EH", "ESH", "西撒哈拉", "967", "", 0f)]
        public const string WesternSahara = "Western sahara";

        /// <summary>
        /// 也门 (Yemen)
        /// </summary>
        [Country("Yemen", "YE", "YEM", "也门", "967", "421", -5f)]
        public const string Yemen = "Yemen";

        /// <summary>
        /// 塞尔维亚 (Yugoslavia)
        /// </summary>
        [Country("Yugoslavia", "RS", "SRB", "塞尔维亚", "381", "220", -7f)]
        public const string Yugoslavia = "Yugoslavia";

        /// <summary>
        /// 刚果（金） (Zaire)
        /// </summary>
        [Country("Zaire", "CD", "COD", "刚果（金）", "243", "630", -7f)]
        public const string Zaire = "Zaire";

        /// <summary>
        /// 赞比亚 (Zambia)
        /// </summary>
        [Country("Zambia", "ZM", "ZMB", "赞比亚", "260", "645", -6f)]
        public const string Zambia = "Zambia";

        /// <summary>
        /// 桑给巴尔 (Zanzibar)
        /// </summary>
        [Country("Zanzibar", "", "", "桑给巴尔", "259", "", 0f)]
        public const string Zanzibar = "Zanzibar";

        /// <summary>
        /// 津巴布韦 (Zimbabwe)
        /// </summary>
        [Country("Zimbabwe", "ZW", "ZWE", "津巴布韦", "263", "648", -6f)]
        public const string Zimbabwe = "Zimbabwe";

        /// <summary>
        /// 中国 (China)
        /// </summary>
        [Country("China", "CN", "CHN", "中国", "86", "460", 0f)]
        public const string China = "China";
    }
}
