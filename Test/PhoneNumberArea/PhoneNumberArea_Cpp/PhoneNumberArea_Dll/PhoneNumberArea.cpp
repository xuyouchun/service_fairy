// PhoneNumberArea.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "PhoneNumberArea.h";

// 手机号的区域信息
struct phone_number_area {
	unsigned short area;	// 号码段，即手机的4-7位
	unsigned char province;	// 省（索引，从provinces中查找对应的字符串）
	unsigned char city;		// 市（索引，从cities中查找对应的字符串)
};

// 所有省份
static const char *provinces[] = { "江苏", "河北", "福建", "北京", "上海", "山东", "四川", "湖北", "陕西", "山西", "河南", "吉林", "内蒙古", "云南", "江西", "湖南", "广西", "宁夏", "广东", "浙江", "重庆", "辽宁", "安徽", "黑龙江", "西藏", "新疆", "甘肃", "青海", "天津", "贵州", "海南" };

// 所有城市，第一维为省份索引，第二维为城市
static const char *cities[][22] = {
	{ "无锡", "南京", "镇江", "泰州", "盐城", "连云港", "徐州", "南通", "扬州", "常州", "苏州", "淮安", "宿迁" },
	{ "邯郸", "石家庄", "保定", "张家口", "邢台", "沧州", "唐山", "衡水", "承德", "廊坊", "秦皇岛" },
	{ "福州", "厦门", "泉州", "漳州", "南平", "莆田", "宁德", "龙岩", "三明" },
	{ "北京" },
	{ "上海" },
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
	{ "重庆", "黔江", "万州", "涪陵" },
	{ "铁岭", "锦州", "丹东", "鞍山", "葫芦岛", "沈阳", "朝阳", "营口", "抚顺", "辽阳", "阜新", "盘锦", "本溪", "大连" },
	{ "安庆", "宣城", "阜阳／亳州", "滁州", "宿州", "巢湖", "芜湖", "阜阳", "六安", "黄山", "淮北", "马鞍山", "合肥", "蚌埠", "淮南", "铜陵", "池州", "亳州" },
	{ "齐齐哈尔", "哈尔滨", "佳木斯", "牡丹江", "绥化", "大兴安岭", "伊春", "鹤岗", "大庆", "双鸭山", "鸡西", "七台河", "黑河" },
	{ "拉萨", "日喀则", "林芝", "昌都", "那曲", "山南", "阿里" },
	{ "克拉玛依", "塔城", "和田", "乌鲁木齐", "哈密", "石河子", "奎屯", "昌吉", "博乐", "伊犁", "库尔勒", "阿克苏", "阿勒泰", "喀什", "吐鲁番", "克州", "阿图什", "博州", "巴州", "博尔塔拉" },
	{ "兰州", "陇南", "临夏", "白银", "定西", "平凉", "庆阳", "武威", "张掖", "酒泉嘉峪关", "天水", "甘南", "金昌", "酒泉", "西峰", "金昌武威", "嘉峪关" },
	{ "海南", "西宁", "海东", "格尔木", "海北", "黄南", "果洛", "玉树", "海西" },
	{ "天津" },
	{ "都匀", "遵义", "安顺", "凯里", "铜仁", "毕节", "六盘水", "兴义", "贵阳", "黔南", "黔东南", "黔西南" },
	{ "海口" }
};

// 使用折半算法查找指定手机号码段的区域信息
// @areas [in] 按顺序排列的区域信息列表
// @size  [in] 区域信息的个数
// @area  [in] 手机号码段，即手机号码的4-7位
// returns 该号码段的区域信息，如果未查找到，则返回NULL
static phone_number_area* find_area(phone_number_area *areas, int size, unsigned short area)
{
	// 折半查找算法
	int begin = 0, end = size - 1;
	while(begin <= end)
	{
		int mid = (begin + end) / 2;
		phone_number_area* a = (phone_number_area*)(areas + mid);
		if(a->area <= area && (mid >= end || area < (a+1)->area))
			return a;

		if(a->area > area)
			end = mid - 1;
		else
			begin = mid + 1;
	}

	return NULL;
}

// 从指定的字符串中，获取指定的一节
static int get_part(const char *s, int start, int count)
{
	char str[20];
	strncpy(str, s + start, count);
	return atoi(str);
}

// 根据手机号码的前三位，查询运营商，查询不成功时返回NULL
// @operation [in] 运营商，即手机号码前三位
static const char *get_operation(int operation)
{
	typedef const unsigned char op_type;
	const char *mobile_name = "移动", *union_name = "联通", *telecom_name = "电信";
	static struct op_part {	op_type start, end;	const char *name; } parts[] = {    // 按顺序排序运营商号码信息，分别为起始值、结束值、运营商名称
		{ 130, 132, union_name },		{ 133, 133, telecom_name },		{ 134, 139, mobile_name },		{ 144, 144, union_name },
		{ 145, 145, union_name },		{ 147, 147, mobile_name },		{ 150, 152, mobile_name },		{ 153, 153, telecom_name },
		{ 155, 156, union_name },		{ 157, 159, mobile_name },		{ 180, 180, telecom_name },		{ 182, 183, mobile_name },
		{ 185, 186, union_name },		{ 187, 188, mobile_name },		{ 189, 189, telecom_name }
	};
	
	// 使用折半查询来查找运营商
	int begin = 0, end = sizeof(parts) / sizeof(parts[0]) - 1;
	while(begin <= end)
	{
		int mid = (begin + end) / 2;
		op_part &part = parts[mid];
		if(operation >= part.start && operation <= part.end)
			return part.name;

		if(operation < part.start)
			end = mid - 1;
		else
			begin = mid + 1;
	}

	return NULL;
}

// 根据手机号创建其bin文件所在的路径
// @phone_number [in] 手机号
// @path [out] 文件路径
// remarks 文件名以手机号前3位+".bin"命名
static void create_path(const char *phone_number, char *path)
{
	static const char file_dir[] = "D:\\Work\\Data\\PhoneNumberAreaData\\2011-12-16\\bin\\";  // 此处需要配置文件所在的路径
	const int file_dir_length = sizeof(file_dir) / sizeof(file_dir[0]) - 1;

	strcpy(path, file_dir);
	strncpy(path + file_dir_length, phone_number, 3);
	strcpy(path + file_dir_length + 3, ".bin");
}

// 读取指定文件的所有字节
// @path [in] 文件路径
// @buffer [out] 用于存储文件内容的缓存区
// returns 缓存区大小
// remarks 需要手动删除buffer
static int read_from_file(const char *path, unsigned char **buffer)
{
	FILE *pFile = fopen(path, "rb");
	if(pFile == NULL)
		return 0;

	long file_length = filelength(fileno(pFile));
	*buffer = (unsigned char*)malloc(file_length);
	if(*buffer == NULL)
		return 0;

	fread_s(*buffer, file_length, 1, file_length, pFile);
	fclose(pFile);
	return file_length;
}

// 根据手机号码，获取运营商、归属地
// @phone_number	[in]	手机号码
// @operation		[out]	运营商
// @province		[out]	省份
// @city			[out]	城市
// returns	是否成功
bool __stdcall get_area_by_phone_number(const char *phone_number, const char **operation, const char **province, const char **city)
{
	// 读取手机号的3-7位，并转化为unsigned short型，该4位表示手机所在的区域
	unsigned short area = get_part(phone_number, 3, 4);

	// 根据手机号获取其bin文件所在的路径，该文件以“手机号前三位+.bin”来命名
	char path[255];
	create_path(phone_number, path);

	// 将该bin文件读入内存中
	unsigned char *buffer;
	int length = read_from_file(path, &buffer);
	if(length == NULL)
		return false;

	// 使用折半查找法，查找该手机号所在的区域
	phone_number_area *result = find_area((phone_number_area*)buffer, length / sizeof(phone_number_area), area);
	if(result != NULL) {
		*province = provinces[result->province];
		*city = cities[result->province][result->city];
	}

	// 释放bin文件的内存
	free(buffer);

	// 根据手机号前三位，获取它的运营商
	*operation = get_operation(get_part(phone_number, 0, 3));
	return true;
}


int _tmain(int argc, _TCHAR* argv[])
{
	const char *operation, *province, *city;  // 运营商，省，市
	const char *phone_number = "13717674043";
	if(get_area_by_phone_number(phone_number, &operation, &province, &city))
	{
		std::cout << phone_number << " " << operation << " " << province << " " << city << std::endl;
	}
	
	return 0;
}

