#pragma once

#include "..\Common\Common.h"

#define NS_COMMON_COMMUNICATION cmun

FUNC_HELLO(Common_Communication);


// 类型定义
namespace NS_COMMON_COMMUNICATION
{
	/********************  ServiceStatusCode  ******************/

	// 状态码
	enum ServiceStatusCode
	{

	};


	/**********************  ServiceResult  ********************/

	// 服务应答结果
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

	// 通信数据实体
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

	// 消息
	struct Message
	{
	public:
		Message();
		Message(string method, string action, CommunicateData *pData)
			: Method(method), Action(action), PData(pData) {}

		~Message();

	public:
		string Method;  // 方法
		string Action;  // Action

		CommunicateData *PData;  // 通信数据
	};
}



