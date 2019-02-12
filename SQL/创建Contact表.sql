Use BhUser;

/*****************  创建表 ******************/

If OBJECT_ID('Contact_F_Basic_0000') Is Not Null Begin
	Drop Table Contact_F_Basic_0000;
	Drop Table Contact_F_Basic_0001;
	Drop Table Contact_F_Basic_0002;
	Drop Table Contact_F_Basic_0003;
End;

-- 通信录表：Contact
Create Table Contact_F_Basic_0000 (
	ID				Int,
	UserID			Int,
	CtUserID		Int,
	CtUserName		Varchar(128),
);

Create Clustered Index Index_Contact_F_Basic_0000_UserID On Contact_F_Basic_0000 ( UserID Asc );
Create Index Index_Contact_F_Basic_0000_ID On Contact_F_Basic_0000 ( ID Asc );
Create Index Index_Contact_F_Basic_0000_CtUserName On Contact_F_Basic_0000 ( CtUserName Asc );

Go

Create Table Contact_F_Basic_0001 (
	ID				Int,
	UserID			Int,
	CtUserID		Int,
	CtUserName		Varchar(128),
);

Create Clustered Index Index_Contact_F_Basic_0001_UserID On Contact_F_Basic_0001 ( UserID Asc );
Create Index Index_Contact_F_Basic_0001_ID On Contact_F_Basic_0001 ( ID Asc );
Create Index Index_Contact_F_Basic_0001_CtUserName On Contact_F_Basic_0001 ( CtUserName Asc );

Go

Create Table Contact_F_Basic_0002 (
	ID				Int,
	UserID			Int,
	CtUserID		Int,
	CtUserName		Varchar(128),
);

Create Clustered Index Index_Contact_F_Basic_0002_UserID On Contact_F_Basic_0002 ( UserID Asc );
Create Index Index_Contact_F_Basic_0002_ID On Contact_F_Basic_0002 ( ID Asc );
Create Index Index_Contact_F_Basic_0002_CtUserName On Contact_F_Basic_0002 ( CtUserName Asc );

Create Table Contact_F_Basic_0003 (
	ID				Int,
	UserID			Int,
	CtUserID		Int,
	CtUserName		Varchar(128),
);

Create Clustered Index Index_Contact_F_Basic_0003_UserID On Contact_F_Basic_0003 ( UserID Asc );
Create Index Index_Contact_F_Basic_0003_ID On Contact_F_Basic_0003 ( ID Asc );
Create Index Index_Contact_F_Basic_0003_CtUserName On Contact_F_Basic_0003 ( CtUserName Asc );

If OBJECT_ID('Contact_F_Detail_0000') Is Not Null Begin
	Drop Table Contact_F_Detail_0000;
	Drop Table Contact_F_Detail_0001;
	Drop Table Contact_F_Detail_0002;
	Drop Table Contact_F_Detail_0003;
End;


-- 通信录分表:Detail
Create Table Contact_F_Detail_0000 (
	ID				Int,
	UserID			Int,
	CtFirstName		NVarchar(128),
	CtLastName		NVarchar(128)
);

Create Clustered Index Index_Contact_F_Detail_0000_UserID On Contact_F_Detail_0000 ( UserID Asc);
Create Index Index_Contact_F_Detail_0000_ID On Contact_F_Detail_0000 ( ID Asc);

Go

Create Table Contact_F_Detail_0001 (
	ID				Int,
	UserID			Int,
	CtFirstName		NVarchar(128),
	CtLastName		NVarchar(128)
);

Create Clustered Index Index_Contact_F_Detail_0001_UserID On Contact_F_Detail_0001 ( UserID Asc);
Create Index Index_Contact_F_Detail_0001_ID On Contact_F_Detail_0001 ( ID Asc);

Go

Create Table Contact_F_Detail_0002 (
	ID				Int,
	UserID			Int,
	CtFirstName		NVarchar(128),
	CtLastName		NVarchar(128)
);

Create Clustered Index Index_Contact_F_Detail_0002_UserID On Contact_F_Detail_0002 ( UserID Asc);
Create Index Index_Contact_F_Detail_0002_ID On Contact_F_Detail_0002 ( ID Asc);

Go

Create Table Contact_F_Detail_0003 (
	ID				Int,
	UserID			Int,
	CtFirstName		NVarchar(128),
	CtLastName		NVarchar(128)
);

Create Clustered Index Index_Contact_F_Detail_0003_UserID On Contact_F_Detail_0003 ( UserID Asc);
Create Index Index_Contact_F_Detail_0003_ID On Contact_F_Detail_0003 ( ID Asc);

Go

-- 创建镜像表
If OBJECT_ID('Contact_I_Relation') Is Not Null Begin
	Drop Table Contact_I_Relation;
End;

Create Table Contact_I_Relation (
	ID				Int,
	UserID			Int,
	[Basic.CtUserID]	Int
);

Create Clustered Index Index_Contact_I_Relation_CtUserID On Contact_I_Relation ( [Basic.CtUserID] Asc );
Create Index Index_Contact_I_Relation_ID On Contact_I_Relation ( ID Asc );

Go


/*****************  写入元数据 ******************/

Use UnionTableMeta;
Begin Tran myTran;

Execute DeleteTable 'Contact';

-- 数据库连接
Declare @ConnectionId As Int;
Select @ConnectionId = ID From Connection;

-- 创建表
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKey, PrimaryKeyType, RouteKey, RouteKeyType, DefaultGroup)
	Values('Contact', 'ID', 'Int32', 'UserID', 'Int32', 'Basic');
Set @TableID = @@IDENTITY;

-- 创建字段组
Declare @GroupId_Basic As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Basic', @TableID, 4, 'Mod', '');
Set @GroupId_Basic = @@IDENTITY;

Declare @GroupId_Detail As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Detail', @TableID, 4, 'Mod', '');
Set @GroupId_Detail = @@IDENTITY;

-- 创建字段
Declare @FieldId_CtUserId As Int;
Declare @FieldId_CtUserName As Int;
Declare @FieldId_CtFirstName As Int;
Declare @FieldId_CtLastName As Int;

Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'CtUserName', 'Text');
Set @FieldId_CtUserName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Basic, 'CtUserId', 'Int32');
Set @FieldId_CtUserId = @@IDENTITY;

Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Detail, 'CtFirstName', 'UnicodeText');
Set @FieldId_CtFirstName = @@IDENTITY;
Insert Into Field(FieldGroupID, Name, DataType) Values (@GroupId_Detail, 'CtLastName', 'UnicodeText');
Set @FieldId_CtLastName = @@IDENTITY;

-- 创建分表
Insert Into PartialTable (FieldGroupID, [Index], [ConnectionID]) Values
	(@GroupId_Basic, 0, @ConnectionId),
	(@GroupId_Basic, 1, @ConnectionId),
	(@GroupId_Basic, 2, @ConnectionId),
	(@GroupId_Basic, 3, @ConnectionId);

Insert Into PartialTable (FieldGroupID, [Index], [ConnectionID]) Values
	(@GroupId_Detail, 0, @ConnectionId),
	(@GroupId_Detail, 1, @ConnectionId),
	(@GroupId_Detail, 2, @ConnectionId),
	(@GroupId_Detail, 3, @ConnectionId);
	
-- 创建镜像表
Declare @ImageTableID_Relation As Int;
Insert Into ImageTable ([Name], TableID, ConnectionID,  [Desc]) Values ('Relation', @TableID, @ConnectionID, '');
Set @ImageTableID_Relation = @@IDENTITY;

Declare @ImageTableFieldID_UserID As Int;
Declare @ImageTableFieldID_CtUserID As Int;
Insert Into ImageTableField (ImageTableID, FieldID) Values (@ImageTableID_Relation, @FieldId_CtUserId);
Set @ImageTableFieldID_CtUserId = @@IDENTITY;

-- 创建索引表
Declare @IndexID_Relation As Int;
Insert Into [Index] (ImageTableID, [Desc]) Values (@ImageTableID_Relation, '');
Set @IndexID_Relation = @@IDENTITY;
Insert Into IndexField (IndexID, ImageTableFieldID) Values (@IndexID_Relation, @ImageTableFieldID_CtUserId);

Commit Tran myTran;
