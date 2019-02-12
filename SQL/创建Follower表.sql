Use BhUser;

/*****************  ������ ******************/

If OBJECT_ID('Follower_F_Main_0000') Is Not Null Begin
	Drop Table Follower_F_Main_0000;
	Drop Table Follower_F_Main_0001;
	Drop Table Follower_F_Main_0002;
	Drop Table Follower_F_Main_0003;
End;

-- ��˿��
Create Table Follower_F_Main_0000 (
	ID				Int,
	UserID			Int,
	FrUserID		Int,
	UserName		Varchar(128)
);

Create Clustered Index Index_Follower_F_Main_0000_UserID On Follower_F_Main_0000 ( UserID Asc );
Create Index Index_Follower_F_Main_0000_ID On Follower_F_Main_0000 ( ID Asc );
Create Index Index_Follower_F_Main_0000_UserName On Follower_F_Main_0000 ( UserName Asc );

Go

-- ��˿��
Create Table Follower_F_Main_0001 (
	ID				Int,
	UserID			Int,
	FrUserID		Int,
	UserName		Varchar(128)
);

Create Clustered Index Index_Follower_F_Main_0001_UserID On Follower_F_Main_0001 ( UserID Asc );
Create Index Index_Follower_F_Main_0001_ID On Follower_F_Main_0001 ( ID Asc );
Create Index Index_Follower_F_Main_0001_UserName On Follower_F_Main_0001 ( UserName Asc );

Go


-- ��˿��
Create Table Follower_F_Main_0002 (
	ID				Int,
	UserID			Int,
	FrUserID		Int,
	UserName		Varchar(128)
);

Create Clustered Index Index_Follower_F_Main_0002_UserID On Follower_F_Main_0002 ( UserID Asc );
Create Index Index_Follower_F_Main_0002_ID On Follower_F_Main_0002 ( ID Asc );
Create Index Index_Follower_F_Main_0002_UserName On Follower_F_Main_0002 ( UserName Asc );

Go


-- ��˿��
Create Table Follower_F_Main_0003 (
	ID				Int,
	UserID			Int,
	FrUserID		Int,
	UserName		Varchar(128)
);

Create Clustered Index Index_Follower_F_Main_0003_UserID On Follower_F_Main_0003 ( UserID Asc );
Create Index Index_Follower_F_Main_0003_ID On Follower_F_Main_0003 ( ID Asc );
Create Index Index_Follower_F_Main_0003_UserName On Follower_F_Main_0003 ( UserName Asc );

Go

/*****************  д��Ԫ���� ******************/

Use UnionTableMeta;
Begin Tran myTran;

Exec DeleteTable 'Follower';

-- ���ݿ�����
Declare @ConnectionId As Int;
Select @ConnectionId = ID From Connection Where Name = 'BhUser';
Print 'ConnectionId = ' + Cast(@ConnectionId As Varchar);


-- ������
Declare @TableID As Int;
Insert Into UnionTable(Name, PrimaryKey, PrimaryKeyType, RouteKey, RouteKeyType, DefaultGroup)
	Values('Follower', 'ID', 'Int32', 'UserName', 'Text', 'Main');
Set @TableID = @@IDENTITY;

-- �����ֶ���
Declare @GroupId_FollowerMain As Int;
Insert Into FieldGroup(Name, TableID, PartialCount, RouteType, RouteArgs) Values ('Main', @TableID, 4, 'Mod', '');
Set @GroupId_FollowerMain = @@IDENTITY;

-- �����ֶ�
Insert Into Field(FieldGroupID, Name, DataType) Values
	(@GroupId_FollowerMain, 'FrUserID', 'Int32'),
	(@GroupId_FollowerMain, 'UserID', 'Int32');

-- �����ֱ�
Insert Into PartialTable (FieldGroupID, [Index], [ConnectionID]) Values
	(@GroupId_FollowerMain, 0, @ConnectionId),
	(@GroupId_FollowerMain, 1, @ConnectionId),
	(@GroupId_FollowerMain, 2, @ConnectionId),
	(@GroupId_FollowerMain, 3, @ConnectionId);

Commit Tran myTran;

