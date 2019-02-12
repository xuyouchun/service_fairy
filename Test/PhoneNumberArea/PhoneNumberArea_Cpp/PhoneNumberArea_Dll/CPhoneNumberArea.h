
#ifndef PHONE_NUMBER_AREA_HEADER
#define PHONE_NUMBER_AREA_HEADER

// 手机归属地算法封装
class __declspec(dllexport) CPhoneNumberArea
{
private:
	CPhoneNumberArea(const std::string& operation);

public:
	~CPhoneNumberArea();

public:
	static std::string GetArea(const std::string& phoneNumber);

private:
	unsigned char *_buffer;
	int _bufferLength;
	const std::string _operation;

	bool _LoadBytes();
	std::string _SearchArea(int area);
	std::string _SearchArea(const std::string& phoneNumber);
};

#endif //PHONE_NUMBER_AREA_HEADER
