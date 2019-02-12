// PhoneNumberArea_Dll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "PhoneNumberArea.h"
#include "PhoneNumberArea_Dll.h"

int __stdcall get_area_by_phone_number_2(const char *phone_number, char* buffer)
{
	const char *operation, *province, *city;
	if(get_area_by_phone_number(phone_number, &operation, &province, &city))
	{
		strcpy(buffer, operation);
		strcat(buffer, "|");
		strcat(buffer, province);
		strcat(buffer, "|");
		strcat(buffer, city);
		return strlen(buffer);
	}

	return 0;
}