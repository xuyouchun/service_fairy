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

// ��ȫɾ������
template<typename T>
inline void SafeDelete(T* &p)
{
	if(p != NULL) {
		delete p;
		p = NULL;
	}
}

class Object;

// �����������ڹ�����
class ObjectContext {

public:
	// ִ�ж����ʼ��
	virtual void OnInit(Object *pObj) {

	}

	// ִ�ж����ͷ�
	virtual void OnRelease(Object *pObj) {

	}

public:
	static ObjectContext * const PInstance;
};

// ֧�������ڹ���Ķ������
class Object {

public:
	// ���������ڹ������Ĺ��캯��
	Object(ObjectContext *pObjCtx) : _pObjCtx((pObjCtx == NULL)? ObjectContext::PInstance : pObjCtx) {
		_pObjCtx->OnInit(this);
	}

	// �޲ι��캯��
	Object() : _pObjCtx(ObjectContext::PInstance) {
		_pObjCtx->OnInit(this);
	}

private:
	// ��ֹ��������
	Object(const Object &obj2);

public:
	virtual ~Object()
	{
		_pObjCtx->OnRelease(this);
	}

private:
	ObjectContext *_pObjCtx;

};



// �������Ͷ���
namespace NS_COMMON
{
	// ���ݸ�ʽ
	enum DataFormat
	{
		// δ֪
		Unknown,

		// �����Ʊ���
		Binary = 1,

		// Json����
		Json = 2,

		// XML����
		Xm = 3,
	};
};