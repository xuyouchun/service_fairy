Create DataBase BhUser;
Go

Use BhUser;

/*****************  ������ ******************/

-- ������Ϣ��Basic
Create Table User_F_Basic_0000 (
	ID				Int Primary Key Not Null,		-- �û�ID
	UserName		Varchar(32) Not Null,			-- �û���
	RealName		NVarchar(32) Not Null,			-- ��ʵ����
	NickName		NVarchar(32) Not Null,			-- �ǳ�
	[Status]		TinyInt		Not Null,			-- ״̬
	CreationTime	DateTime	Not Null			-- ����ʱ��
);

Go

Select * Into User_F_Basic_0001 From User_F_Basic_0000;
Select * Into User_F_Basic_0002 From User_F_Basic_0000;
Select * Into User_F_Basic_0003 From User_F_Basic_0000;

Go

-- ��ȫ��Ϣ��Security
Create Table User_F_Security_0000 (
	ID				Int Primary Key Not Null,		-- �û�ID
	[Password]		Varchar(32) Not Null,			-- ����
	[Sid]			Char(36),						-- ��ȫ��
);

Go

Select * Into User_F_Security_0001 From User_F_Security_0000;
Select * Into User_F_Security_0002 From User_F_Security_0000;
Select * Into User_F_Security_0003 From User_F_Security_0000;

-- ��ϸ��Ϣ��Detail
Create Table User_F_Detail_0000 (
	ID				Int Primary Key Not Null,		-- �û�ID
	Age				Int Not Null,					-- ����
);

Go

Select * Into User_F_Detail_0001 From User_F_Detail_0000;
Select * Into User_F_Detail_0002 From User_F_Detail_0000;
Select * Into User_F_Detail_0003 From User_F_Detail_0000;

-- �����1���û���¼��User_I_Login
Create Table User_I_Login (
	ID					Int Primary Key Not Null,		-- �û�ID
	[Basic.UserName]	Varchar(32)	Not Null,			-- �û���
	[Security.Password]	Varchar(32) Not Null			-- ����
);

Go

-- �����1������
Create Index Index_User_I_Login_UserName_Password On User_I_Login (
	[Basic.UserName] Asc,
	[Security.Password] Asc
);

Go

Create Unique Index Index_User_I_Login_UserName On User_I_Login (
	[Basic.UserName] Asc
);

Go

-- �����2����ȫ�룺User_I_Sid
Create Table User_I_Sid (
	ID					Int Primary Key Not Null,		-- �û�ID
	[Security.Sid]		Char(36) Not Null,				-- ��ȫ��
);

-- �����2������
Create Unique Index Index_User_I_Sid On User_I_Sid (
	[Security.Sid]	Asc
);


/*

Use BhUser;
Drop Table User_F_Basic_0000;
Drop Table User_F_Basic_0001;
Drop Table User_F_Basic_0002;
Drop Table User_F_Basic_0003;

Drop Table User_F_Security_0000;
Drop Table User_F_Security_0001;
Drop Table User_F_Security_0002;
Drop Table User_F_Security_0003;

Drop Table User_F_Detail_0000;
Drop Table User_F_Detail_0001;
Drop Table User_F_Detail_0002;
Drop Table User_F_Detail_0003;

Drop Table User_I_Login;
Drop Table User_I_Sid;

*/

--Update Connection Set DataSource = '117.79.130.229', DbName = 'BhUser', UserName = 'hefengxin', Password='zBr6s336';

/*****************  ��ʼ��Ԫ���� ******************/

Use UnionTableMeta;
Begin Tran myTran;

-- ��������
Declare @ConnectionID As Int;
Insert Into Connection (Name, DataSource, DbName, UserName, [Password], [Desc]) Values 
	('BhUser', 'xuyc-pc', 'BhUser', 'xuyc', 'xuyc1', '')
Set @ConnectionID = @@IDENTITY;

-- ������
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKeyType) Values('User', 'Int32');
Set @TableID = @@IDENTITY;

-- �����ֶ���
Declare @GroupId_Basic As Int;
Declare @GroupId_Security As Int;
Declare @GroupId_Detail As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Basic', @TableID, 4, 'Mod', '');
Set @GroupId_Basic = @@IDENTITY;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Security', @TableID, 4, 'Mod', '');
Set @GroupId_Security = @@IDENTITY;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Detail', @TableID, 4, 'Mod', '');
Set @GroupId_Detail = @@IDENTITY;


-- �����ֶΣ�������Ϣ��
Declare @FieldId_UserName As Int;
Declare @FieldId_RealName As Int;
Declare @FieldId_NickName As Int;
Declare @FieldId_Status AS Int;
Declare @FieldId_CreationTime As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'UserName', 'String');
Set @FieldId_UserName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'RealName', 'String');
Set @FieldId_RealName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'NickName', 'String');
Set @FieldId_NickName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'Status', 'Int32');
Set @FieldId_Status = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'CreationTime', 'DateTime');
Set @FieldId_CreationTime = @@IDENTITY;

-- �����ֶΣ���ȫ��Ϣ��
Declare @FieldId_Password As Int;
Declare @FieldId_Sid As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Password', 'String');
Set @FieldId_Password = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Sid', 'String');
Set @FieldId_Sid = @@IDENTITY;

-- �����ֶΣ���ϸ��Ϣ��
Declare @FieldId_Age As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Detail', 'String');
Set @FieldId_Age = @@IDENTITY;

-- �����ֱ�
Insert Into PartialTable (FieldGroupID, [Index], [ConnectionID]) Values
	(@GroupId_Basic, 0, @ConnectionID),
	(@GroupId_Basic, 1, @ConnectionID),
	(@GroupId_Basic, 2, @ConnectionID),
	(@GroupId_Basic, 3, @ConnectionID),
	(@GroupId_Security, 0, @ConnectionID),
	(@GroupId_Security, 1, @ConnectionID),
	(@GroupId_Security, 2, @ConnectionID),
	(@GroupId_Security, 3, @ConnectionID),
	(@GroupId_Detail, 0, @ConnectionID),
	(@GroupId_Detail, 1, @ConnectionID),
	(@GroupId_Detail, 2, @ConnectionID),
	(@GroupId_Detail, 3, @ConnectionID);

-- �����������¼��
Declare @ImageTableID_Login As Int;
Insert Into ImageTable ([Name], TableID, ConnectionID,  [Desc]) Values ('Login', @TableID, @ConnectionID, '��¼');
Set @ImageTableID_Login = @@IDENTITY;

-- ���������ֶα���¼��
Declare @ImageTableFieldID_UserName As Int;
Declare @ImageTableFieldID_Password As Int;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_UserName);
Set @ImageTableFieldID_UserName = @@IDENTITY;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_Password);
Set @ImageTableFieldID_Password = @@IDENTITY;

-- �����������ȫ�룩
Declare @imageTableID_Sid As Int;
Insert Into ImageTable ([Name], TableID, ConnectionID, [Desc]) Values ('Sid', @TableID, @ConnectionID, '��ȫ��')
Set @imageTableID_Sid = @@IDENTITY;

-- ���������ֶα���ȫ�룩
Declare @ImageTableFieldID_Sid As Int;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@imageTableID_Sid, @FieldId_Sid);
Set @ImageTableFieldID_Sid = @@IDENTITY;

-- ������������¼��
Declare @IndexID_Login As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login');
Set @IndexID_Login = @@IDENTITY;

-- ���������ֶα���¼��
Declare @IndexFieldID As Int;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login, @ImageTableFieldID_UserName),
	(@IndexID_Login, @ImageTableFieldID_Password);

-- ������������ȫ�룩
Declare @IndexID_Sid As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@imageTableID_Sid, 'User_Index_Sid');
Set @IndexID_Sid = @@IDENTITY;

-- ���������ֶα���ȫ�룩
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Sid, @ImageTableFieldID_Sid);


Commit Tran myTran;

--Rollback Tran myTran;

/*
Use UnionTableMeta;
Truncate Table Connection;
Truncate Table UnionTable;
Truncate Table FieldGroup;
Truncate Table Field;
Truncate Table PartialTable;
Truncate Table ImageTable;
Truncate Table ImageTableField;
Truncate Table [Index];
Truncate Table IndexField;
*/
