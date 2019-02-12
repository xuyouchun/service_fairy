#pragma once

#define DLL_EXPORT __declspec(dllexport)
#define DLL_IMPORT __declspec(dllimport)

#define EXTERN_C extern "C"
#define DLL_EXPORT_C EXTERN_C DLL_EXPORT

#define INTERFACE struct

#define FUNC_HELLO(projName) DLL_EXPORT_C const char * Hello_##projName();
#define FUNC_HELLO_IMPL(projName) const char * Hello_##projName() { return "Hello World!"; }

FUNC_HELLO(Common);

#define NS_COMMON cmn

#include <algorithm>
#include <string>
#include <iostream>

typedef std::wstring string;

// 安全删除对象
template<typename T>
inline void SafeDelete(T* &p)
{
	if(p != NULL) {
		delete p;
		p = NULL;
	}
}

class Object;

// 对象生命周期管理器
class ObjectContext {

public:
	// 执行对象初始化
	virtual void OnInit(Object *pObj) {

	}

	// 执行对象释放
	virtual void OnRelease(Object *pObj) {

	}

public:
	static ObjectContext * const PInstance;
};

// 支持生成期管理的对象基类
class Object {

public:
	// 带有生成期管理器的构造函数
	Object(ObjectContext *pObjCtx) : _pObjCtx((pObjCtx == NULL)? ObjectContext::PInstance : pObjCtx) {
		_pObjCtx->OnInit(this);
	}

	// 无参构造函数
	Object() : _pObjCtx(ObjectContext::PInstance) {
		_pObjCtx->OnInit(this);
	}

private:
	// 禁止拷贝构造
	Object(const Object &obj2);

public:
	virtual ~Object()
	{
		_pObjCtx->OnRelease(this);
	}

private:
	ObjectContext *_pObjCtx;

};



// 公共类型定义
namespace NS_COMMON
{
	// 数据格式
	enum DataFormat
	{
		// 未知
		Unknown,

		// 二进制编码
		Binary = 1,

		// Json编码
		Json = 2,

		// XML编码
		Xm = 3,
	};
};