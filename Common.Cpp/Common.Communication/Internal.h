#pragma once

namespace NS_COMMON_COMMUNICATION {

	// 消息解码编码器
	INTERFACE IMessageEncoder
	{
		// 编码，将msg类的数据编码到stream流中
		virtual void Encode(const Message &msg, std::ostream &stream) = 0;

		// 解码，将stream流中的数据解析为Message类
		virtual bool Decode(const std::istream &stream, Message *pMsg) = 0;
	};
}