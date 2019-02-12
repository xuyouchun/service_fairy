// PhoneNumberArea.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "PhoneNumberArea.h";

// �ֻ��ŵ�������Ϣ
struct phone_number_area {
	unsigned short area;	// ����Σ����ֻ���4-7λ
	unsigned char province;	// ʡ����������provinces�в��Ҷ�Ӧ���ַ�����
	unsigned char city;		// �У���������cities�в��Ҷ�Ӧ���ַ���)
};

// ����ʡ��
static const char *provinces[] = { "����", "�ӱ�", "����", "����", "�Ϻ�", "ɽ��", "�Ĵ�", "����", "����", "ɽ��", "����", "����", "���ɹ�", "����", "����", "����", "����", "����", "�㶫", "�㽭", "����", "����", "����", "������", "����", "�½�", "����", "�ຣ", "���", "����", "����" };

// ���г��У���һάΪʡ���������ڶ�άΪ����
static const char *cities[][22] = {
	{ "����", "�Ͼ�", "��", "̩��", "�γ�", "���Ƹ�", "����", "��ͨ", "����", "����", "����", "����", "��Ǩ" },
	{ "����", "ʯ��ׯ", "����", "�żҿ�", "��̨", "����", "��ɽ", "��ˮ", "�е�", "�ȷ�", "�ػʵ�" },
	{ "����", "����", "Ȫ��", "����", "��ƽ", "����", "����", "����", "����" },
	{ "����" },
	{ "�Ϻ�" },
	{ "����", "����", "Ϋ��", "����", "����", "�ൺ", "�Ͳ�", "��̨", "̩��", "����", "����", "��Ӫ", "����", "��ׯ", "����", "�ĳ�", "����" },
	{ "�Թ�", "����", "�ɶ�", "��Ԫ", "����", "�Ű�", "üɽ", "�ϳ�", "����", "��֦��", "����", "����", "�˱�", "����", "��ɽ", "�㰲", "����", "�ڽ�", "��ɽ", "����", "����", "����" },
	{ "��ʩ", "�人", "Ǳ��", "����", "����", "�Ƹ�", "Т��", "ʮ��", "����", "��ʯ", "����", "����", "����", "�˲�", "����", "������������Ǳ��" },
	{ "�Ӱ�", "μ��", "����", "����", "����", "����", "����", "����", "����", "����������", "����", "ͭ��" },
	{ "̫ԭ", "��ͬ", "����", "�ٷ�", "�˳�", "˷��", "����", "��Ȫ", "����", "����", "����" },
	{ "����", "֣��", "����", "����", "���", "ƽ��ɽ", "����", "����", "����", "����", "�ܿ�", "����", "�ױ�", "���", "���", "פ���", "����Ͽ", "�괨" },
	{ "����", "��ƽ", "�׳�", "��ԭ", "ͨ��", "÷�ӿ�", "����", "��Դ", "�Ӽ�", "��ɽ", "����", "�ӱ�" },
	{ "���ͺ���", "��������", "ͨ��", "���", "�����׶�", "������", "��ͷ", "������˹", "�ٺ�", "���ֺ���", "��ʤ", "����", "�ں�", "��������", "�����첼", "�˰���", "���ױ���", "���ֹ�����" },
	{ "��ɽ", "����", "����", "��ͨ", "�ٲ�", "���", "����", "��ɽ", "�ն�", "��Ϫ", "����", "ŭ��", "����", "����", "�º�", "��˫����" },
	{ "�ϲ�", "ӥ̶", "�Ž�", "����", "����", "�˴�", "����", "����", "����", "������", "Ƽ��" },
	{ "����", "��ɳ", "��̶", "����", "����", "����", "����", "����", "¦��", "����", "����", "�żҽ�", "����", "����", "����" },
	{ "����", "�ӳ�", "����", "����", "����", "����", "��ɫ", "����", "����", "���Ǹ�", "����", "���", "����", "����" },
	{ "����", "ʯ��ɽ", "����", "��ԭ", "����" },
	{ "����", "��ɽ", "��β", "�ع�", "����", "÷��", "����", "�麣", "��ɽ", "�Ƹ�", "��ͷ", "��ݸ", "����", "ï��", "��Զ", "տ��", "����", "����", "����", "����", "��Դ" },
	{ "��", "����", "����", "����", "����", "����", "��ɽ", "̨��", "����", "��ˮ", "����" },
	{ "����", "ǭ��", "����", "����" },
	{ "����", "����", "����", "��ɽ", "��«��", "����", "����", "Ӫ��", "��˳", "����", "����", "�̽�", "��Ϫ", "����" },
	{ "����", "����", "����������", "����", "����", "����", "�ߺ�", "����", "����", "��ɽ", "����", "��ɽ", "�Ϸ�", "����", "����", "ͭ��", "����", "����" },
	{ "�������", "������", "��ľ˹", "ĵ����", "�绯", "���˰���", "����", "�׸�", "����", "˫Ѽɽ", "����", "��̨��", "�ں�" },
	{ "����", "�տ���", "��֥", "����", "����", "ɽ��", "����" },
	{ "��������", "����", "����", "��³ľ��", "����", "ʯ����", "����", "����", "����", "����", "�����", "������", "����̩", "��ʲ", "��³��", "����", "��ͼʲ", "����", "����", "��������" },
	{ "����", "¤��", "����", "����", "����", "ƽ��", "����", "����", "��Ҵ", "��Ȫ������", "��ˮ", "����", "���", "��Ȫ", "����", "�������", "������" },
	{ "����", "����", "����", "���ľ", "����", "����", "����", "����", "����" },
	{ "���" },
	{ "����", "����", "��˳", "����", "ͭ��", "�Ͻ�", "����ˮ", "����", "����", "ǭ��", "ǭ����", "ǭ����" },
	{ "����" }
};

// ʹ���۰��㷨����ָ���ֻ�����ε�������Ϣ
// @areas [in] ��˳�����е�������Ϣ�б�
// @size  [in] ������Ϣ�ĸ���
// @area  [in] �ֻ�����Σ����ֻ������4-7λ
// returns �ú���ε�������Ϣ�����δ���ҵ����򷵻�NULL
static phone_number_area* find_area(phone_number_area *areas, int size, unsigned short area)
{
	// �۰�����㷨
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

// ��ָ�����ַ����У���ȡָ����һ��
static int get_part(const char *s, int start, int count)
{
	char str[20];
	strncpy(str, s + start, count);
	return atoi(str);
}

// �����ֻ������ǰ��λ����ѯ��Ӫ�̣���ѯ���ɹ�ʱ����NULL
// @operation [in] ��Ӫ�̣����ֻ�����ǰ��λ
static const char *get_operation(int operation)
{
	typedef const unsigned char op_type;
	const char *mobile_name = "�ƶ�", *union_name = "��ͨ", *telecom_name = "����";
	static struct op_part {	op_type start, end;	const char *name; } parts[] = {    // ��˳��������Ӫ�̺�����Ϣ���ֱ�Ϊ��ʼֵ������ֵ����Ӫ������
		{ 130, 132, union_name },		{ 133, 133, telecom_name },		{ 134, 139, mobile_name },		{ 144, 144, union_name },
		{ 145, 145, union_name },		{ 147, 147, mobile_name },		{ 150, 152, mobile_name },		{ 153, 153, telecom_name },
		{ 155, 156, union_name },		{ 157, 159, mobile_name },		{ 180, 180, telecom_name },		{ 182, 183, mobile_name },
		{ 185, 186, union_name },		{ 187, 188, mobile_name },		{ 189, 189, telecom_name }
	};
	
	// ʹ���۰��ѯ��������Ӫ��
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

// �����ֻ��Ŵ�����bin�ļ����ڵ�·��
// @phone_number [in] �ֻ���
// @path [out] �ļ�·��
// remarks �ļ������ֻ���ǰ3λ+".bin"����
static void create_path(const char *phone_number, char *path)
{
	static const char file_dir[] = "D:\\Work\\Data\\PhoneNumberAreaData\\2011-12-16\\bin\\";  // �˴���Ҫ�����ļ����ڵ�·��
	const int file_dir_length = sizeof(file_dir) / sizeof(file_dir[0]) - 1;

	strcpy(path, file_dir);
	strncpy(path + file_dir_length, phone_number, 3);
	strcpy(path + file_dir_length + 3, ".bin");
}

// ��ȡָ���ļ��������ֽ�
// @path [in] �ļ�·��
// @buffer [out] ���ڴ洢�ļ����ݵĻ�����
// returns ��������С
// remarks ��Ҫ�ֶ�ɾ��buffer
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

// �����ֻ����룬��ȡ��Ӫ�̡�������
// @phone_number	[in]	�ֻ�����
// @operation		[out]	��Ӫ��
// @province		[out]	ʡ��
// @city			[out]	����
// returns	�Ƿ�ɹ�
bool __stdcall get_area_by_phone_number(const char *phone_number, const char **operation, const char **province, const char **city)
{
	// ��ȡ�ֻ��ŵ�3-7λ����ת��Ϊunsigned short�ͣ���4λ��ʾ�ֻ����ڵ�����
	unsigned short area = get_part(phone_number, 3, 4);

	// �����ֻ��Ż�ȡ��bin�ļ����ڵ�·�������ļ��ԡ��ֻ���ǰ��λ+.bin��������
	char path[255];
	create_path(phone_number, path);

	// ����bin�ļ������ڴ���
	unsigned char *buffer;
	int length = read_from_file(path, &buffer);
	if(length == NULL)
		return false;

	// ʹ���۰���ҷ������Ҹ��ֻ������ڵ�����
	phone_number_area *result = find_area((phone_number_area*)buffer, length / sizeof(phone_number_area), area);
	if(result != NULL) {
		*province = provinces[result->province];
		*city = cities[result->province][result->city];
	}

	// �ͷ�bin�ļ����ڴ�
	free(buffer);

	// �����ֻ���ǰ��λ����ȡ������Ӫ��
	*operation = get_operation(get_part(phone_number, 0, 3));
	return true;
}


int _tmain(int argc, _TCHAR* argv[])
{
	const char *operation, *province, *city;  // ��Ӫ�̣�ʡ����
	const char *phone_number = "13717674043";
	if(get_area_by_phone_number(phone_number, &operation, &province, &city))
	{
		std::cout << phone_number << " " << operation << " " << province << " " << city << std::endl;
	}
	
	return 0;
}

