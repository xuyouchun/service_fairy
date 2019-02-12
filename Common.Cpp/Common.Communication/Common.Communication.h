#pragma once

#include "..\Common\Common.h"

#define NS_COMMON_COMMUNICATION cmun

FUNC_HELLO(Common_Communication);


// ���Ͷ���
namespace NS_COMMON_COMMUNICATION
{
	/********************  ServiceStatusCode  ******************/

	// ״̬��
	enum ServiceStatusCode
	{

	};


	/**********************  ServiceResult  ********************/

	// ����Ӧ����
	struct ServiceResult
	{
	public:
		ServiceResult();
		ServiceResult(ServiceStatusCode statusCode, short statusReason, string statusDesc)
			: StatusCode(statusCode), StatusReason(statusReason), StatusDesc(statusDesc) { }

		~ServiceResult();

	public:
		ServiceStatusCode StatusCode;
		short StatusReason;
		const string StatusDesc;
	};



	/*********************  CommunicateData  *******************/

	// ͨ������ʵ��
	struct CommunicateData
	{
	public:
		CommunicateData();
		CommunicateData(BYTE *pBuffer, int bufferLength, cmn::DataFormat format)
			: PBuffer(pBuffer), BufferLength(bufferLength), Format(format) { }

		~CommunicateData();

	public:
		BYTE *PBuffer;
		int BufferLength;
		cmn::DataFormat Format;

		ServiceStatusCode StatusCode;
		short StatusReasion;
		const string StatusDesc;
	};



	/*************************  Message  ***********************/

	// ��Ϣ
	struct Message
	{
	public:
		Message();
		Message(string method, string action, CommunicateData *pData)
			: Method(method), Action(action), PData(pData) {}

		~Message();

	public:
		string Method;  // ����
		string Action;  // Action

		CommunicateData *PData;  // ͨ������
	};
}



