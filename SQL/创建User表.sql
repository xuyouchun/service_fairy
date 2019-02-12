--Create DataBase BhUser;
Go

Use BhUser;

/*****************  ������ ******************/

If OBJECT_ID('User_F_Basic_0000') Is Not Null Begin
	Drop Table User_F_Basic_0000;
	Drop Table User_F_Basic_0001;
	Drop Table User_F_Basic_0002;
	Drop Table User_F_Basic_0003;
End;

-- ������Ϣ��Basic
Create Table User_F_Basic_0000 (
	UserID			Int Primary Key,		-- �û�ID
	UserName		Varchar(128),			-- �û���
	FirstName		NVarchar(128),			-- ����
	LastName		NVarchar(128),			-- ����
	NickName		NVarchar(128),			-- �ǳ�
	CreationTime	DateTime				-- ����ʱ��
);

Go

-- ������Ϣ��Basic
Create Table User_F_Basic_0001 (
	UserID			Int Primary Key,		-- �û�ID
	UserName		Varchar(128),			-- �û���
	FirstName		NVarchar(128),			-- ����
	LastName		NVarchar(128),			-- ����
	NickName		NVarchar(128),			-- �ǳ�
	CreationTime	DateTime				-- ����ʱ��
);

Go

-- ������Ϣ��Basic
Create Table User_F_Basic_0002 (
	UserID			Int Primary Key,		-- �û�ID
	UserName		Varchar(128),			-- �û���
	FirstName		NVarchar(128),			-- ����
	LastName		NVarchar(128),			-- ����
	NickName		NVarchar(128),			-- �ǳ�
	CreationTime	DateTime				-- ����ʱ��
);

Go

-- ������Ϣ��Basic
Create Table User_F_Basic_0003 (
	UserID			Int Primary Key,		-- �û�ID
	UserName		Varchar(128),			-- �û���
	FirstName		NVarchar(128),			-- ����
	LastName		NVarchar(128),			-- ����
	NickName		NVarchar(128),			-- �ǳ�
	CreationTime	DateTime				-- ����ʱ��
);

Go

If OBJECT_ID('User_F_Security_0000') Is Not Null Begin
	Drop Table User_F_Security_0000;
	Drop Table User_F_Security_0001;
	Drop Table User_F_Security_0002;
	Drop Table User_F_Security_0003;
End;

-- ��ȫ��Ϣ��Security
Create Table User_F_Security_0000 (
	UserID			Int Primary Key,		-- �û�ID
	[Password]		Varchar(64),			-- ����
	[Sid]			Varchar(128)			-- ��ȫ��
);

Go

-- ��ȫ��Ϣ��Security
Create Table User_F_Security_0001 (
	UserID			Int Primary Key,		-- �û�ID
	[Password]		Varchar(64),			-- ����
	[Sid]			Varchar(128)			-- ��ȫ��
);

Go

-- ��ȫ��Ϣ��Security
Create Table User_F_Security_0002 (
	UserID			Int Primary Key,		-- �û�ID
	[Password]		Varchar(64),			-- ����
	[Sid]			Varchar(128)			-- ��ȫ��
);

Go

-- ��ȫ��Ϣ��Security
Create Table User_F_Security_0003 (
	UserID			Int Primary Key,		-- �û�ID
	[Password]		Varchar(64),			-- ����
	[Sid]			Varchar(128)			-- ��ȫ��
);

Go

If OBJECT_ID('User_F_Detail_0000') Is Not Null Begin
	Drop Table User_F_Detail_0000;
	Drop Table User_F_Detail_0001;
	Drop Table User_F_Detail_0002;
	Drop Table User_F_Detail_0003;
End;

-- ��ϸ��Ϣ��Detail
Create Table User_F_Detail_0000 (
	UserID			Int Primary Key,		-- �û�ID
	Age				Int						-- ����
);

Go

-- ��ϸ��Ϣ��Detail
Create Table User_F_Detail_0001 (
	UserID			Int Primary Key,		-- �û�ID
	Age				Int,					-- ����
);

Go

-- ��ϸ��Ϣ��Detail
Create Table User_F_Detail_0002 (
	UserID			Int Primary Key,		-- �û�ID
	Age				Int,					-- ����
);

Go

-- ��ϸ��Ϣ��Detail
Create Table User_F_Detail_0003 (
	UserID			Int Primary Key,		-- �û�ID
	Age				Int,					-- ����
);

Go


If OBJECT_ID('User_F_Status_0000') Is Not Null Begin
	Drop Table User_F_Status_0000;
	Drop Table User_F_Status_0001;
	Drop Table User_F_Status_0002;
	Drop Table User_F_Status_0003;
End;

-- ״̬��Status
Create Table User_F_Status_0000 (
	UserID			Int Primary Key,		-- �û�ID
	[Status]		NVarchar(512),			-- ����״̬����
	StatusChangedTime	DateTime,			-- ״̬�仯ʱ��
	LastOnlineTime	DateTime,				-- �ϴ�����ʱ��
);

Go

-- ״̬��Status
Create Table User_F_Status_0001 (
	UserID			Int Primary Key,		-- �û�ID
	[Status]		NVarchar(512),			-- ����״̬����
	StatusChangedTime	DateTime,			-- ״̬�仯ʱ��
	LastOnlineTime	DateTime,				-- �ϴ�����ʱ��
);

Go

-- ״̬��Status
Create Table User_F_Status_0002 (
	UserID			Int Primary Key,		-- �û�ID
	[Status]		NVarchar(512),			-- ����״̬����
	StatusChangedTime	DateTime,			-- ״̬�仯ʱ��
	LastOnlineTime	DateTime,				-- �ϴ�����ʱ��
);

Go

-- ״̬��Status
Create Table User_F_Status_0003 (
	UserID			Int Primary Key,		-- �û�ID
	[Status]		NVarchar(512),			-- ����״̬����
	StatusChangedTime	DateTime,			-- ״̬�仯ʱ��
	LastOnlineTime	DateTime,				-- �ϴ�����ʱ��
);

Go

If OBJECT_ID('User_I_Login') Is Not Null Begin
	Drop Table User_I_Login;
End;

-- �����1���û���¼��User_I_Login
Create Table User_I_Login (
	UserID				Int Primary Key,	-- �û�ID
	[Basic.UserName]	Varchar(32),		-- �û���
	[Security.Password]	Varchar(32),		-- ����
	[Security.Sid]		Varchar(128)		-- Sid
);

Go

-- �����1��������UserName��Password�����ڵ�¼��������֤��
Create Index Index_User_I_Login_UserName_Password On User_I_Login (
	[Basic.UserName] Asc,
	[Security.Password] Asc
);

Go

-- �����1��������UserName�������û���������Ϣ��
Create Unique Index Index_User_I_Login_UserName On User_I_Login (
	[Basic.UserName] Asc
);

Go

-- �����1��������Sid����ȫ�룩
Create Index Index_User_I_Login_Sid On User_I_Login (
	[Security.Sid]	Asc
);

Go


/*****************  ��ʼ��Ԫ���� ******************/

Use UnionTableMeta;
Begin Tran myTran;

Exec DeleteTable 'User';

-- ��������
Declare @ConnectionID As Int;
Select @ConnectionID = ID From Connection Where Name = 'BhUser';
If @ConnectionID Is Null Begin
	Insert Into Connection (Name, DataSource, DbName, UserName, [Password], [Desc]) Values 
		('BhUser', '117.79.130.229', 'BhUser', 'hefengxin', 'zBr6s336', '')
	Set @ConnectionID = @@IDENTITY;
End;

-- ������
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKey, PrimaryKeyType, RouteKey, RouteKeyType, DefaultGroup)
	Values('User', 'UserID', 'Int32', 'UserID', 'Int32', 'Basic');
Set @TableID = @@IDENTITY;

-- �����ֶ���
Declare @GroupId_Basic As Int;
Declare @GroupId_Security As Int;
Declare @GroupId_Detail As Int;
Declare @GroupId_Status As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Basic', @TableID, 4, 'Mod', '');
Set @GroupId_Basic = @@IDENTITY;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Security', @TableID, 4, 'Mod', '');
Set @GroupId_Security = @@IDENTITY;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Detail', @TableID, 4, 'Mod', '');
Set @GroupId_Detail = @@IDENTITY;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Status', @TableID, 4, 'Mod', '');
Set @GroupId_Status = @@IDENTITY;

-- �����ֶΣ�������Ϣ��
Declare @FieldId_UserName As Int;
Declare @FieldId_FirstName As Int;
Declare @FieldId_LastName As Int;
Declare @FieldId_NickName As Int;
Declare @FieldId_CreationTime As Int;

Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'UserName', 'Text');
Set @FieldId_UserName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'FirstName', 'UnicodeText');
Set @FieldId_FirstName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'LastName', 'UnicodeText');
Set @FieldId_LastName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'NickName', 'UnicodeText');
Set @FieldId_NickName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'CreationTime', 'DateTime');
Set @FieldId_CreationTime = @@IDENTITY;

-- �����ֶΣ���ȫ��Ϣ��
Declare @FieldId_Password As Int;
Declare @FieldId_Sid As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Password', 'Text');
Set @FieldId_Password = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Sid', 'Text');
Set @FieldId_Sid = @@IDENTITY;

-- �����ֶΣ���ϸ��Ϣ��
Declare @FieldId_Age As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Detail, 'Age', 'Int32');
Set @FieldId_Age = @@IDENTITY;

-- �����ֶΣ�״̬��Ϣ��
Declare @FieldId_Status As Int;
Declare @FieldId_LastOnlineTime As Int;
Declare @FieldId_StatusChangedTime As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Status, 'Status', 'UnicodeText');
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Status, 'LastOnlineTime', 'DateTime');
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Status, 'StatusChangedTime', 'DateTime');

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
	(@GroupId_Detail, 3, @ConnectionID),
	(@GroupId_Status, 0, @ConnectionID),
	(@GroupId_Status, 1, @ConnectionID),
	(@GroupId_Status, 2, @ConnectionID),
	(@GroupId_Status, 3, @ConnectionID);

-- �������������������
Declare @ImageTableID_Login As Int;
Insert Into ImageTable ([Name], TableID, ConnectionID,  [Desc]) Values
	('Login', @TableID, @ConnectionID, '��¼�����');
Set @ImageTableID_Login = @@IDENTITY;

-- ���������ֶα�����������
Declare @ImageTableFieldID_UserName As Int;
Declare @ImageTableFieldID_Password As Int;
Declare @ImageTableFieldID_Sid As Int;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_UserName);
Set @ImageTableFieldID_UserName = @@IDENTITY;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_Password);
Set @ImageTableFieldID_Password = @@IDENTITY;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_Sid);
Set @ImageTableFieldID_Sid = @@IDENTITY;

-- ����������User_Index_Login:UserName
Declare @IndexID_Login_UserName As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login:UserName');
Set @IndexID_Login_UserName = @@IDENTITY;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login_UserName, @ImageTableFieldID_UserName);
	
-- ����������User_Index_Login:UserName,Password
Declare @IndexID_Login_UserName_Password As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login:UserName,Password');
Set @IndexID_Login_UserName_Password = @@IDENTITY;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login_UserName_Password, @ImageTableFieldID_UserName),
	(@IndexID_Login_UserName_Password, @ImageTableFieldID_Password);

-- ����������User_Index_Login:Sid
Declare @IndexID_Login_Sid As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login:Sid');
Set @IndexID_Login_Sid = @@IDENTITY;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login_Sid, @ImageTableFieldID_Sid);

Commit Tran myTran;

--Rollback Tran myTran;
