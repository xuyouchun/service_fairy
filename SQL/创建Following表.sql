Use BhUser;

/*****************  ������ ******************/

If OBJECT_ID('Following_F_Main_0000') Is Not Null Begin
	Drop Table Following_F_Main_0000;
	Drop Table Following_F_Main_0001;
	Drop Table Following_F_Main_0002;
	Drop Table Following_F_Main_0003;
End;

-- ��ע��(Main)
Create Table Following_F_Main_0000 (
	ID				Int,
	UserID			Int,
	FgUserID		Int,
	FgUserName		Varchar(128),
);

Create Clustered Index Index_Following_F_Main_0000_UserID On Following_F_Main_0000 ( UserID Asc );
Create Index Index_Following_F_Main_0000_ID On Following_F_Main_0000 ( ID Asc );
Create Index Index_Following_F_Main_0000_FgUserName On Following_F_Main_0000 ( FgUserName Asc );

Go

Create Table Following_F_Main_0001 (
	ID				Int,
	UserID			Int,
	FgUserID		Int,
	FgUserName		Varchar(128),
);

Create Clustered Index Index_Following_F_Main_0001_UserID On Following_F_Main_0001 ( UserID Asc );
Create Index Index_Following_F_Main_0001_ID On Following_F_Main_0001 ( ID Asc );
Create Index Index_Following_F_Main_0001_FgUserName On Following_F_Main_0001 ( FgUserName Asc );

Go

Create Table Following_F_Main_0002 (
	ID				Int,
	UserID			Int,
	FgUserID		Int,
	FgUserName		Varchar(128),
);

Create Clustered Index Index_Following_F_Main_0002_UserID On Following_F_Main_0002 ( UserID Asc );
Create Index Index_Following_F_Main_0002_ID On Following_F_Main_0002 ( ID Asc );
Create Index Index_Following_F_Main_0002_FgUserName On Following_F_Main_0002 ( FgUserName Asc );

Go

Create Table Following_F_Main_0003 (
	ID				Int,
	UserID			Int,
	FgUserID		Int,
	FgUserName		Varchar(128),
);

Create Clustered Index Index_Following_F_Main_0003_UserID On Following_F_Main_0003 ( UserID Asc );
Create Index Index_Following_F_Main_0003_ID On Following_F_Main_0003 ( ID Asc );
Create Index Index_Following_F_Main_0003_FgUserName On Following_F_Main_0003 ( FgUserName Asc );

Go

If OBJECT_ID('Following_F_Detail_0000') Is Not Null Begin
	Drop Table Following_F_Detail_0000;
	Drop Table Following_F_Detail_0001;
	Drop Table Following_F_Detail_0002;
	Drop Table Following_F_Detail_0003;
End;

-- ��ע��(Detail)

Create Table Following_F_Detail_0000 (
	ID				Int,
	UserID			Int,
	FgFirstName		NVarchar(128),
	FgLastName		NVarchar(128)
);

Create Clustered Index Index_Following_F_Detail_0000_UserID On Following_F_Detail_0000 ( UserID Asc );
Create Index Index_Following_F_Detail_0000_ID On Following_F_Detail_0000 ( ID Asc );

Go

-- ��ע��(Detail)

Create Table Following_F_Detail_0001 (
	ID				Int,
	UserID			Int,
	FgFirstName		NVarchar(128),
	FgLastName		NVarchar(128)
);

Create Clustered Index Index_Following_F_Detail_0001_UserID On Following_F_Detail_0001 ( UserID Asc );
Create Index Index_Following_F_Detail_0001_ID On Following_F_Detail_0001 ( ID Asc );

Go

-- ��ע��(Detail)

Create Table Following_F_Detail_0002 (
	ID				Int,
	UserID			Int,
	FgFirstName		NVarchar(128),
	FgLastName		NVarchar(128)
);

Create Clustered Index Index_Following_F_Detail_0002_UserID On Following_F_Detail_0002 ( UserID Asc );
Create Index Index_Following_F_Detail_0002_ID On Following_F_Detail_0002 ( ID Asc );

Go

-- ��ע��(Detail)

Create Table Following_F_Detail_0003 (
	ID				Int,
	UserID			Int,
	FgFirstName		NVarchar(128),
	FgLastName		NVarchar(128)
);

Create Clustered Index Index_Following_F_Detail_0003_UserID On Following_F_Detail_0003 ( UserID Asc );
Create Index Index_Following_F_Detail_0003_ID On Following_F_Detail_0003 ( ID Asc );

Go

/*****************  д��Ԫ���� ******************/

Use UnionTableMeta;
Begin Tran myTran;

Exec DeleteTable 'Following';

-- ���ݿ�����
Declare @ConnectionId As Int;
Select @ConnectionId = ID From Connection Where Name = 'BhUser';
Print 'ConnectionId = ' + Cast(@ConnectionId As Varchar);

-- ������
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKey, PrimaryKeyType, RouteKey, RouteKeyType, DefaultGroup)
	Values('Following', 'ID', 'Int32', 'UserID', 'Int32', 'Main');
Set @TableID = @@IDENTITY;

-- �����ֶ���
Declare @GroupId_Following_Main As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Main', @TableID, 4, 'Mod', '');
Set @GroupId_Following_Main = @@IDENTITY;

Declare @GroupId_Following_Detail As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Detail', @TableID, 4, 'Mod', '');
Set @GroupId_Following_Detail = @@IDENTITY;

-- �����ֶ�
Insert Into Field(FieldGroupID, Name, DataType) Values
	(@GroupId_Following_Main,	'FgUserID',		'Int32'),
	(@GroupId_Following_Main,	'FgUserName',	'Text'),
	(@GroupId_Following_Detail, 'FgFirstName',	'UniqueText'),
	(@GroupId_Following_Detail, 'FgLastName',	'UniqueText');

-- �����ֱ�
Insert Into PartialTable (FieldGroupID, [Index], [ConnectionID]) Values
	(@GroupId_Following_Main, 0, @ConnectionId),
	(@GroupId_Following_Main, 1, @ConnectionId),
	(@GroupId_Following_Main, 2, @ConnectionId),
	(@GroupId_Following_Main, 3, @ConnectionId);

Insert Into PartialTable (FieldGroupID, [Index], [ConnectionID]) Values
	(@GroupId_Following_Detail, 0, @ConnectionId),
	(@GroupId_Following_Detail, 1, @ConnectionId),
	(@GroupId_Following_Detail, 2, @ConnectionId),
	(@GroupId_Following_Detail, 3, @ConnectionId);

Commit Tran myTran;

