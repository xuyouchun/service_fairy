Create DataBase BhUser;
Go

Use BhUser;

/*****************  创建表 ******************/

-- 基本信息表：Basic
Create Table User_F_Basic_0000 (
	ID				Int Primary Key Not Null,		-- 用户ID
	UserName		Varchar(32) Not Null,			-- 用户名
	RealName		NVarchar(32) Not Null,			-- 真实姓名
	NickName		NVarchar(32) Not Null,			-- 昵称
	[Status]		TinyInt		Not Null,			-- 状态
	CreationTime	DateTime	Not Null			-- 创建时间
);

Go

Select * Into User_F_Basic_0001 From User_F_Basic_0000;
Select * Into User_F_Basic_0002 From User_F_Basic_0000;
Select * Into User_F_Basic_0003 From User_F_Basic_0000;

Go

-- 安全信息表：Security
Create Table User_F_Security_0000 (
	ID				Int Primary Key Not Null,		-- 用户ID
	[Password]		Varchar(32) Not Null,			-- 密码
	[Sid]			Char(36),						-- 安全码
);

Go

Select * Into User_F_Security_0001 From User_F_Security_0000;
Select * Into User_F_Security_0002 From User_F_Security_0000;
Select * Into User_F_Security_0003 From User_F_Security_0000;

-- 详细信息表：Detail
Create Table User_F_Detail_0000 (
	ID				Int Primary Key Not Null,		-- 用户ID
	Age				Int Not Null,					-- 年龄
);

Go

Select * Into User_F_Detail_0001 From User_F_Detail_0000;
Select * Into User_F_Detail_0002 From User_F_Detail_0000;
Select * Into User_F_Detail_0003 From User_F_Detail_0000;

-- 镜像表1：用户登录，User_I_Login
Create Table User_I_Login (
	ID					Int Primary Key Not Null,		-- 用户ID
	[Basic.UserName]	Varchar(32)	Not Null,			-- 用户名
	[Security.Password]	Varchar(32) Not Null			-- 密码
);

Go

-- 镜像表1的索引
Create Index Index_User_I_Login_UserName_Password On User_I_Login (
	[Basic.UserName] Asc,
	[Security.Password] Asc
);

Go

Create Unique Index Index_User_I_Login_UserName On User_I_Login (
	[Basic.UserName] Asc
);

Go

-- 镜像表2：安全码：User_I_Sid
Create Table User_I_Sid (
	ID					Int Primary Key Not Null,		-- 用户ID
	[Security.Sid]		Char(36) Not Null,				-- 安全码
);

-- 镜像表2的索引
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

/*****************  初始化元数据 ******************/

Use UnionTableMeta;
Begin Tran myTran;

-- 创建连接
Declare @ConnectionID As Int;
Insert Into Connection (Name, DataSource, DbName, UserName, [Password], [Desc]) Values 
	('BhUser', 'xuyc-pc', 'BhUser', 'xuyc', 'xuyc1', '')
Set @ConnectionID = @@IDENTITY;

-- 创建表
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKeyType) Values('User', 'Int32');
Set @TableID = @@IDENTITY;

-- 创建字段组
Declare @GroupId_Basic As Int;
Declare @GroupId_Security As Int;
Declare @GroupId_Detail As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Basic', @TableID, 4, 'Mod', '');
Set @GroupId_Basic = @@IDENTITY;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Security', @TableID, 4, 'Mod', '');
Set @GroupId_Security = @@IDENTITY;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Detail', @TableID, 4, 'Mod', '');
Set @GroupId_Detail = @@IDENTITY;


-- 创建字段（基本信息）
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

-- 创建字段（安全信息）
Declare @FieldId_Password As Int;
Declare @FieldId_Sid As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Password', 'String');
Set @FieldId_Password = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Sid', 'String');
Set @FieldId_Sid = @@IDENTITY;

-- 创建字段（详细信息）
Declare @FieldId_Age As Int;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Security, 'Detail', 'String');
Set @FieldId_Age = @@IDENTITY;

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
	(@GroupId_Detail, 3, @ConnectionID);

-- 创建镜像表（登录）
Declare @ImageTableID_Login As Int;
Insert Into ImageTable ([Name], TableID, ConnectionID,  [Desc]) Values ('Login', @TableID, @ConnectionID, '登录');
Set @ImageTableID_Login = @@IDENTITY;

-- 创建镜像字段表（登录）
Declare @ImageTableFieldID_UserName As Int;
Declare @ImageTableFieldID_Password As Int;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_UserName);
Set @ImageTableFieldID_UserName = @@IDENTITY;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Login, @FieldId_Password);
Set @ImageTableFieldID_Password = @@IDENTITY;

-- 创建镜像表（安全码）
Declare @imageTableID_Sid As Int;
Insert Into ImageTable ([Name], TableID, ConnectionID, [Desc]) Values ('Sid', @TableID, @ConnectionID, '安全码')
Set @imageTableID_Sid = @@IDENTITY;

-- 创建镜像字段表（安全码）
Declare @ImageTableFieldID_Sid As Int;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@imageTableID_Sid, @FieldId_Sid);
Set @ImageTableFieldID_Sid = @@IDENTITY;

-- 创建索引表（登录）
Declare @IndexID_Login As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Login, 'User_Index_Login');
Set @IndexID_Login = @@IDENTITY;

-- 创建索引字段表（登录）
Declare @IndexFieldID As Int;
Insert Into IndexField (IndexID, ImageTableFieldID) Values
	(@IndexID_Login, @ImageTableFieldID_UserName),
	(@IndexID_Login, @ImageTableFieldID_Password);

-- 创建索引表（安全码）
Declare @IndexID_Sid As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@imageTableID_Sid, 'User_Index_Sid');
Set @IndexID_Sid = @@IDENTITY;

-- 创建索引字段表（安全码）
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
