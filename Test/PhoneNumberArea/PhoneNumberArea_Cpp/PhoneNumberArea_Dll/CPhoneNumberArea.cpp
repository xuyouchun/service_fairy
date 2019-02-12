
#include "stdafx.h"
#include "CPhoneNumberArea.h"

typedef unsigned char byte;
const std::string basePath = "D:\\Work\\Data\\PhoneNumberAreaData\\2011-12-16\\PhoneNumberArea\\bin\\";

// 所有省份
static const char *provinces[] = { "江苏", "河北", "福建", "北京", "上海", "山东", "四川", "湖北", "陕西", "山西", "河南", "吉林", "内蒙古", "云南", "江西", "湖南", "广西", "宁夏", "广东", "浙江", "重庆", "辽宁", "安徽", "黑龙江", "西藏", "新疆", "甘肃", "青海", "天津", "贵州", "海南" };

// 所有城市，第一维为省份索引，第二维为城市
static const char *cities[][22] = {
	{ "无锡", "南京", "镇江", "泰州", "盐城", "连云港", "徐州", "南通", "扬州", "常州", "苏州", "淮安", "宿迁" },
	{ "邯郸", "石家庄", "保定", "张家口", "邢台", "沧州", "唐山", "衡水", "承德", "廊坊", "秦皇岛" },
	{ "福州", "厦门", "泉州", "漳州", "南平", "莆田", "宁德", "龙岩", "三明" },
	{ "" },  // 北京
	{ "" },  // 上海
	{ "济南", "菏泽", "潍坊", "济宁", "德州", "青岛", "淄博", "烟台", "泰安", "临沂", "滨州", "东营", "威海", "枣庄", "日照", "聊城", "莱芜" },
	{ "自贡", "绵阳", "成都", "广元", "达州", "雅安", "眉山", "南充", "泸州", "攀枝花", "德阳", "遂宁", "宜宾", "资阳", "乐山", "广安", "巴中", "内江", "凉山", "阿坝", "甘孜", "西昌" },
	{ "恩施", "武汉", "潜江", "荆门", "随州", "黄冈", "孝感", "十堰", "荆州", "黄石", "襄阳", "鄂州", "咸宁", "宜昌", "仙桃", "江汉天门仙桃潜江" },
	{ "延安", "渭南", "榆林", "咸阳", "西安", "安康", "商洛", "汉中", "宝鸡", "西安／咸阳", "商州", "铜川" },
	{ "太原", "大同", "晋中", "临汾", "运城", "朔州", "忻州", "阳泉", "长治", "晋城", "吕梁" },
	{ "商丘", "郑州", "安阳", "新乡", "许昌", "平顶山", "信阳", "南阳", "开封", "洛阳", "周口", "焦作", "鹤壁", "濮阳", "漯河", "驻马店", "三门峡", "潢川" },
	{ "长春", "四平", "白城", "松原", "通化", "梅河口", "吉林", "辽源", "延吉", "白山", "珲春", "延边" },
	{ "呼和浩特", "乌兰浩特", "通辽", "赤峰", "巴彦淖尔", "海拉尔", "包头", "鄂尔多斯", "临河", "锡林浩特", "东胜", "集宁", "乌海", "阿拉善盟", "乌兰察布", "兴安盟", "呼伦贝尔", "锡林郭勒盟" },
	{ "文山", "大理", "曲靖", "昭通", "临沧", "红河", "昆明", "保山", "普洱", "玉溪", "楚雄", "怒江", "迪庆", "丽江", "德宏", "西双版纳" },
	{ "南昌", "鹰潭", "九江", "上饶", "抚州", "宜春", "吉安", "赣州", "新余", "景德镇", "萍乡" },
	{ "岳阳", "长沙", "湘潭", "株洲", "衡阳", "郴州", "常德", "益阳", "娄底", "邵阳", "吉首", "张家界", "怀化", "永州", "湘西" },
	{ "南宁", "河池", "柳州", "桂林", "梧州", "玉林", "百色", "钦州", "北海", "防城港", "贺州", "贵港", "崇左", "来宾" },
	{ "银川", "石嘴山", "吴忠", "固原", "中卫" },
	{ "深圳", "佛山", "汕尾", "韶关", "广州", "梅州", "阳江", "珠海", "中山", "云浮", "汕头", "东莞", "惠州", "茂名", "清远", "湛江", "潮州", "肇庆", "揭阳", "江门", "河源" },
	{ "金华", "杭州", "宁波", "嘉兴", "绍兴", "衢州", "舟山", "台州", "湖州", "丽水", "温州" },
	{ "", "黔江", "万州", "涪陵" },  // 重庆
	{ "铁岭", "锦州", "丹东", "鞍山", "葫芦岛", "沈阳", "朝阳", "营口", "抚顺", "辽阳", "阜新", "盘锦", "本溪", "大连" },
	{ "安庆", "宣城", "阜阳／亳州", "滁州", "宿州", "巢湖", "芜湖", "阜阳", "六安", "黄山", "淮北", "马鞍山", "合肥", "蚌埠", "淮南", "铜陵", "池州", "亳州" },
	{ "齐齐哈尔", "哈尔滨", "佳木斯", "牡丹江", "绥化", "大兴安岭", "伊春", "鹤岗", "大庆", "双鸭山", "鸡西", "七台河", "黑河" },
	{ "拉萨", "日喀则", "林芝", "昌都", "那曲", "山南", "阿里" },
	{ "克拉玛依", "塔城", "和田", "乌鲁木齐", "哈密", "石河子", "奎屯", "昌吉", "博乐", "伊犁", "库尔勒", "阿克苏", "阿勒泰", "喀什", "吐鲁番", "克州", "阿图什", "博州", "巴州", "博尔塔拉" },
	{ "兰州", "陇南", "临夏", "白银", "定西", "平凉", "庆阳", "武威", "张掖", "酒泉嘉峪关", "天水", "甘南", "金昌", "酒泉", "西峰", "金昌武威", "嘉峪关" },
	{ "海南", "西宁", "海东", "格尔木", "海北", "黄南", "果洛", "玉树", "海西" },
	{ "" },  // 天津
	{ "都匀", "遵义", "安顺", "凯里", "铜仁", "毕节", "六盘水", "兴义", "贵阳", "黔南", "黔东南", "黔西南" },
	{ "海口" }
};

static const char *operation_names[] = { "移动", "联通", "电信" };
static byte mobile_name = 0, union_name = 1, telecom_name = 2;
static byte op_parts[] = { // 按顺序排序运营商号码信息，分别为起始值、结束值、运营商名称
	130, 132, union_name,		133, 133, telecom_name,		134, 139, mobile_name,		144, 144, union_name,
	145, 145, union_name,		147, 147, mobile_name,		150, 152, mobile_name,		153, 153, telecom_name,
	155, 156, union_name,		157, 159, mobile_name,		180, 180, telecom_name,		182, 183, mobile_name,
	185, 186, union_name,		187, 188, mobile_name,		189, 189, telecom_name
};

static int _LoadFile(std::string path, byte **ppBuf)
{
	FILE *pFile = fopen(path.c_str(), "rb");
	if(pFile == NULL) return 0;

	int len = filelength(fileno(pFile));
	*ppBuf = new byte[len];
	fread_s(*ppBuf, len, 1, len, pFile);

	fclose(pFile);
	return len;
}

CPhoneNumberArea::CPhoneNumberArea(const std::string& operation)
	: _operation(operation)
{

}


bool CPhoneNumberArea::_LoadBytes()
{
	if(_buffer != NULL)
		return true;

	std::string path = basePath + _operation + ".bin";
	_bufferLength = _LoadFile(path, &_buffer);
	return _buffer != NULL;
}

std::string CPhoneNumberArea::GetArea(const std::string& phoneNumber)
{
	if(phoneNumber.length() < 3)
		return "";

	CPhoneNumberArea a(phoneNumber.substr(0, 3));
	return a.GetArea(phoneNumber);
}

std::string CPhoneNumberArea::_SearchArea(const std::string& phoneNumber)
{
	if(phoneNumber.length() < 7 || !_LoadBytes())
		return "";

	unsigned short area = atoi(phoneNumber.substr(3, 4).c_str());
	return _SearchArea(area);
}

#define AREA(p) ( (unsigned short)( ((p)[1] << 8) | (p)[0] ) )
#define PROVENCE(p) ( (byte)(p)[2] )
#define CITY(p) ( (byte)(p)[3] )

// 折半查找算法
std::string CPhoneNumberArea::_SearchArea(int area)
{
	int begin = 0, end = (_bufferLength / 4) - 1;
	while(begin <= end)
	{
		int mid = (begin + end) / 2;
		byte* p = _buffer + (mid * 4);

		if(AREA(p) <= area && (mid >= end || area < AREA(p + 4)))
		{
			std::string s = provinces[PROVENCE(p)];
			s.append(cities[PROVENCE(p)][CITY(p)]);
			return s;
		}

		if(AREA(p) > area)
			end = mid - 1;
		else
			begin = mid + 1;
	}

	return "";
}

CPhoneNumberArea::~CPhoneNumberArea()
{
	if(_buffer != NULL)
	{
		delete _buffer;
		_buffer = NULL;
	}
}


