
#ifndef PHONE_NUMBER_AREA_84E9E5B7_0D3C_4D22_AE3E_98551ABFEAC3
#define PHONE_NUMBER_AREA_84E9E5B7_0D3C_4D22_AE3E_98551ABFEAC3


// 根据手机号码获取区域
extern "C" __declspec(dllexport) bool get_area_by_phone_number(const char *phone_number, const char **operation, const char **province, const char **city);

extern "C" __declspec(dllexport) bool get_area_by_phone_number_2(const char *phone_number, char* buffer);

extern "C" __declspec(dllexport) void get_area_by_phone_number_4(char *phone_number);

#endif  // #ifndef PHONE_NUMBER_AREA_84E9E5B7_0D3C_4D22_AE3E_98551ABFEAC3

