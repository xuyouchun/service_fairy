--Create DataBase Log;
Use [Log];

/*****************  创建表 ******************/

Create Table SmsLog_F_Main_0000 (
	ID			Int Primary Key Not Null,
	[Phone]		Varchar(64),
	[Message]	NVarchar(140),
	[Service]	Varchar(64),
	[Response]	NVarchar(512),
	[Time]		DateTime
);

Create Table SmsLog_F_Main_0001 (
	ID			Int Primary Key Not Null,
	[Phone]		Varchar(64),
	[Message]	NVarchar(140),
	[Service]	Varchar(64),
	[Response]	NVarchar(512),
	[Time]		DateTime
);

/*

Select * From SmsLog_F_Main_0000;
Select * From SmsLog_F_Main_0001;

*/

Go

/*****************  写入元数据 ******************/

Use UnionTableMeta;
Begin Tran myTran;

-- 创建连接
Declare @ConnectionID As Int;
Insert Into Connection (Name, DataSource, DbName, UserName, [Password], [Desc]) Values 
	('Log', '117.79.130.229', 'Log', 'hefengxin', 'zBr6s336', '')
Set @ConnectionID = @@IDENTITY;

-- 创建表
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKey, PrimaryKeyType, RouteKey, RouteKeyType, DefaultGroup)
	 Values('SmsLog', 'ID', 'Int32', 'ID', 'Int32', 'Main');
Set @TableID = @@IDENTITY;

-- 创建字段组
Declare @GroupId_SmsLogMain As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Main', @TableID, 2, 'Mod', '');
Set @GroupId_SmsLogMain = @@IDENTITY;

-- 创建字段
Declare @FieldId_Phone As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_SmsLogMain, 'Phone', 'Text');
Set @FieldId_Phone = @@IDENTITY;

Declare @FieldId_Message As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_SmsLogMain, 'Message', 'UnicodeText');
Set @FieldId_Message = @@IDENTITY;

Declare @FieldId_Service As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_SmsLogMain, 'Service', 'Text');
Set @FieldId_Service = @@IDENTITY;

Declare @FieldId_Response As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_SmsLogMain, 'Response', 'UnicodeText');
Set @FieldId_Response = @@IDENTITY;

Declare @FieldId_Time As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_SmsLogMain, 'Time', 'DateTime');
Set @FieldId_Time = @@IDENTITY;

-- 创建分表
Insert Into PartialTable (FieldGroupID, [Index], [ConnectionID]) Values
	(@GroupId_SmsLogMain, 0, @ConnectionId),
	(@GroupId_SmsLogMain, 1, @ConnectionId);

Commit Tran myTran;

