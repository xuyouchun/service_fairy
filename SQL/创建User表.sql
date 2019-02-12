--Create DataBase BhUser;
Go

Use BhUser;

/*****************  创建表 ******************/

If OBJECT_ID('User_F_Basic_0000') Is Not Null Begin
	Drop Table User_F_Basic_0000;
	Drop Table User_F_Basic_0001;
	Drop Table User_F_Basic_0002;
	Drop Table User_F_Basic_0003;
End;

-- 基本信息表：Basic
Create Table User_F_Basic_0000 (
	UserID			Int Primary Key,		-- 用户ID
	UserName		Varchar(128),			-- 用户名
	FirstName		NVarchar(128),			-- 姓氏
	LastName		NVarchar(128),			-- 名字
	NickName		NVarchar(128),			-- 昵称
	CreationTime	DateTime				-- 创建时间
);

Go

-- 基本信息表：Basic
Create Table User_F_Basic_0001 (
	UserID			Int Primary Key,		-- 用户ID
	UserName		Varchar(128),			-- 用户名
	FirstName		NVarchar(128),			-- 姓氏
	LastName		NVarchar(128),			-- 名字
	NickName		NVarchar(128),			-- 昵称
	CreationTime	DateTime				-- 创建时间
);

Go

-- 基本信息表：Basic
Create Table User_F_Basic_0002 (
	UserID			Int Primary Key,		-- 用户ID
	UserName		Varchar(128),			-- 用户名
	FirstName		NVarchar(128),			-- 姓氏
	LastName		NVarchar(128),			-- 名字
	NickName		NVarchar(128),			-- 昵称
	CreationTime	DateTime				-- 创建时间
);

Go

-- 基本信息表：Basic
Create Table User_F_Basic_0003 (
	UserID			Int Primary Key,		-- 用户ID
	UserName		Varchar(128),			-- 用户名
	FirstName		NVarchar(128),			-- 姓氏
	LastName		NVarchar(128),			-- 名字
	NickName		NVarchar(128),			-- 昵称
	CreationTime	DateTime				-- 创建时间
);

Go

If OBJECT_ID('User_F_Security_0000') Is Not Null Begin
	Drop Table User_F_Security_0000;
	Drop Table User_F_Security_0001;
	Drop Table User_F_Security_0002;
	Drop Table User_F_Security_0003;
End;

-- 安全信息表：Security
Create Table User_F_Security_0000 (
	UserID			Int Primary Key,		-- 用户ID
	[Password]		Varchar(64),			-- 密码
	[Sid]			Varchar(128)			-- 安全码
);

Go

-- 安全信息表：Security
Create Table User_F_Security_0001 (
	UserID			Int Primary Key,		-- 用户ID
	[Password]		Varchar(64),			-- 密码
	[Sid]			Varchar(128)			-- 安全码
);

Go

-- 安全信息表：Security
Create Table User_F_Security_0002 (
	UserID			Int Primary Key,		-- 用户ID
	[Password]		Varchar(64),			-- 密码
	[Sid]			Varchar(128)			-- 安全码
);

Go

-- 安全信息表：Security
Create Table User_F_Security_0003 (
	UserID			Int Primary Key,		-- 用户ID
	[Password]		Varchar(64),			-- 密码
	[Sid]			Varchar(128)			-- 安全码
);

Go

If OBJECT_ID('User_F_Detail_0000') Is Not Null Begin
	Drop Table User_F_Detail_0000;
	Drop Table User_F_Detail_0001;
	Drop Table User_F_Detail_0002;
	Drop Table User_F_Detail_0003;
End;

-- 详细信息表：Detail
Create Table User_F_Detail_0000 (
	UserID			Int Primary Key,		-- 用户ID
	Age				Int						-- 年龄
);

Go

-- 详细信息表：Detail
Create Table User_F_Detail_0001 (
	UserID			Int Primary Key,		-- 用户ID
	Age				Int,					-- 年龄
);

Go

-- 详细信息表：Detail
Create Table User_F_Detail_0002 (
	UserID			Int Primary Key,		-- 用户ID
	Age				Int,					-- 年龄
);

Go

-- 详细信息表：Detail
Create Table User_F_Detail_0003 (
	UserID			Int Primary Key,		-- 用户ID
	Age				Int,					-- 年龄
);

Go


If OBJECT_ID('User_F_Status_0000') Is Not Null Begin
	Drop Table User_F_Status_0000;
	Drop Table User_F_Status_0001;
	Drop Table User_F_Status_0002;
	Drop Table User_F_Status_0003;
End;

-- 状态表：Status
Create Table User_F_Status_0000 (
	UserID			Int Primary Key,		-- 用户ID
	[Status]		NVarchar(512),			-- 在线状态描述
	StatusChangedTime	DateTime,			-- 状态变化时间
	LastOnlineTime	DateTime,				-- 上次在线时间
);

Go

-- 状态表：Status
Create Table User_F_Status_0001 (
	UserID			Int Primary Key,		-- 用户ID
	[Status]		NVarchar(512),			-- 在线状态描述
	StatusChangedTime	DateTime,			-- 状态变化时间
	LastOnlineTime	DateTime,				-- 上次在线时间
);

Go

-- 状态表：Status
Create Table User_F_Status_0002 (
	UserID			Int Primary Key,		-- 用户ID
	[Status]		NVarchar(512),			-- 在线状态描述
	StatusChangedTime	DateTime,			-- 状态变化时间
	LastOnlineTime	DateTime,				-- 上次在线时间
);

Go

-- 状态表：Status
Create Table User_F_Status_0003 (
	UserID			Int Primary Key,		-- 用户ID
	[Status]		NVarchar(512),			-- 在线状态描述
	StatusChangedTime	DateTime,			-- 状态变化时间
	LastOnlineTime	DateTime,				-- 上次在线时间
);

Go

If OBJECT_ID('User_I_Login') Is Not Null Begin
	Drop Table User_I_Login;
End;

-- 镜像表1：用户登录，User_I_Login
Create Table User_I_Login (
	UserID				Int Primary Key,	-- 用户ID
	[Basic.UserName]	Varchar(32),		-- 用户名
	[Security.Password]	Varchar(32),		-- 密码
	[Security.Sid]		Varchar(128)		-- Sid
);

Go

-- 镜像表1的索引：UserName与Password（用于登录的密码验证）
Create Index Index_User_I_Login_UserName_Password On User_I_Login (
	[Basic.UserName] Asc,
	[Security.Password] Asc
);

Go

-- 镜像表1的索引：UserName（根据用户名查找信息）
Create Unique Index Index_User_I_Login_UserName On User_I_Login (
	[Basic.UserName] Asc
);

Go

-- 镜像表1的索引：Sid（安全码）
Create Index Index_User_I_Login_Sid On User_I_Login (
	[Security.Sid]	Asc
);

Go


/*****************  初始化元数据 ******************/

Use UnionTableMeta;
Begin Tran myTran;

Exec DeleteTable 'User';

-- 创建连接
Declare @ConnectionID As Int;
Select @ConnectionID = ID From Connection Where Name = 'BhUser';
If @ConnectionID Is Null Begin
	Insert Into Connection (Name, DataSource, DbName, UserName, [Password], [Desc]) Values 
		('BhUser', '117.79.130.229', 'BhUser', 'hefengxin', 'zBr6s336', '')
	Set @ConnectionID = @@IDENTITY;
End;

-- 创建表
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKey, PrimaryKeyType, RouteKey, RouteKeyType, DefaultGroup)
	Values('User', 'UserID', 'Int32', 'UserID', 'Int32', 'Basic');
Set @TableID = @@IDENTITY;

-- 创建字段组
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

-- 创建字段（基本信息）
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

-- 创建字段（安全信息）
Declare @FieldId_Password As Int;
Declare @FieldId_Sid As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Password', 'Text');
Set @FieldId_Password = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Sid', 'Text');
Set @FieldId_Sid = @@IDENTITY;

-- 创建字段（详细信息）
Declare @FieldId_Age As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Detail, 'Age', 'Int32');
Set @FieldId_Age = @@IDENTITY;

-- 创建字段（状态信息）
Declare @FieldId_Status As Int;
Declare @FieldId_LastOnlineTime As Int;
Declare @FieldId_StatusChangedTime As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Status, 'Status', 'UnicodeText');
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Status, 'LastOnlineTime', 'DateTime');
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Status, 'StatusChangedTime', 'DateTime');

-- 创建分表
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

-- 创建镜像表（用作索引）
Declare @ImageTableID_Login As Int;
Insert Into ImageTable ([Name], TableID, ConnectionID,  [Desc]) Values
	('Login', @TableID, @ConnectionID, '登录镜像表');
Set @ImageTableID_Login = @@IDENTITY;

-- 创建镜像字段表（用作索引）
Declare @ImageTableFieldID_UserName As Int;
Declare @ImageTableFieldID_Password As Int;
Declare @ImageTableFieldID_Sid As Int;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_UserName);
Set @ImageTableFieldID_UserName = @@IDENTITY;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_Password);
Set @ImageTableFieldID_Password = @@IDENTITY;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_Sid);
Set @ImageTableFieldID_Sid = @@IDENTITY;

-- 创建索引表：User_Index_Login:UserName
Declare @IndexID_Login_UserName As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login:UserName');
Set @IndexID_Login_UserName = @@IDENTITY;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login_UserName, @ImageTableFieldID_UserName);
	
-- 创建索引表：User_Index_Login:UserName,Password
Declare @IndexID_Login_UserName_Password As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login:UserName,Password');
Set @IndexID_Login_UserName_Password = @@IDENTITY;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login_UserName_Password, @ImageTableFieldID_UserName),
	(@IndexID_Login_UserName_Password, @ImageTableFieldID_Password);

-- 创建索引表：User_Index_Login:Sid
Declare @IndexID_Login_Sid As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login:Sid');
Set @IndexID_Login_Sid = @@IDENTITY;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login_Sid, @ImageTableFieldID_Sid);

Commit Tran myTran;

--Rollback Tran myTran;
