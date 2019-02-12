
#include "stdafx.h"
#include "..\PhoneNumberArea_Dll\PhoneNumberArea.h"
#include "..\PhoneNumberArea_Dll\CPhoneNumberArea.h"

int _tmain(int argc, _TCHAR* argv[])
{
	//const char *operation, *province, *city;
	//const char *phone_number = "13717674043";
	//if(get_area_by_phone_number(phone_number, &operation, &province, &city))
	//{
	//	std::cout << phone_number << " " << operation << " " << province << " " << city << std::endl;
	//}

	std::string s = CPhoneNumberArea::GetArea(std::string("13717674043"));
	std::cout << s.c_str() << std::endl;
	
	return 0;
}

