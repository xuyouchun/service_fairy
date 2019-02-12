#pragma once

namespace NS_COMMON_COMMUNICATION {

	// ��Ϣ���������
	INTERFACE IMessageEncoder
	{
		// ���룬��msg������ݱ��뵽stream����
		virtual void Encode(const Message &msg, std::ostream &stream) = 0;

		// ���룬��stream���е����ݽ���ΪMessage��
		virtual bool Decode(const std::istream &stream, Message *pMsg) = 0;
	};
}