
If OBJECT_ID('Connection') Is Not Null Begin

	Drop Table Connection;
	Drop Table UnionTable;
	Drop Table FieldGroup;
	Drop Table ImageTable;
	Drop Table ImageTableField;
	Drop Table [Index];
	Drop Table IndexField;
	Drop Table PartialTable;
	Drop Table PrimaryKey;
	Drop Table Field;
	--Drop Procedure 
		
End;

-- �������
Create Table Connection (
	[ID]		Int Identity Primary Key,		-- ��������
	[Name]		NVarchar(128) Not Null,			-- ��������
	DataSource	Varchar(128) Not Null,			-- ����Դ���������ƻ�ip��
	DbName		Varchar(128) Not Null,			-- ���ݿ�����
	UserName	Varchar(128) Not Null,			-- �û���
	[Password]	Varchar(128) Not Null,			-- ����
	[Desc]		NVarchar(Max)					-- ��ע
);

Go

-- ��
Create Table UnionTable (
	[ID]				Int Identity Primary Key,	-- ��������
	[Name]				NVarchar(128) Not Null,		-- ����
	[PrimaryKey]		NVarchar(128) Not Null,		-- ��������
	[PrimaryKeyType]	Varchar(32) Not Null,		-- ��������
	[RouteKey]			NVarchar(128) Not Null,		-- ·�ɼ�����
	[RouteKeyType]		Varchar(32) Not Null,		-- ·�ɼ�����
	[DefaultGroup]		NVarchar(128) Not Null,		-- Ĭ�Ϸ���
	[Desc]				NVarchar(Max)				-- ��ע
);

Go

-- ���飨����ֱ�
Create Table FieldGroup (
	[ID]			Int Identity Primary Key,		-- ��������
	[Name]			NVarchar(128) Not Null,			-- ��������
	[TableID]		Int Not Null,					-- �������ID
	[PartialCount]	Int Not Null,					-- �ֱ�����
	[RouteType]		Varchar(32) Not Null,			-- ����·�����ͣ�����ʲô��������Ӧ�ô��ĸ����ж�ȡ��
	[RouteArgs]		Varchar(Max),					-- ����·�ɲ���
	[Desc]			NVarchar(Max)					-- ��ע
);

Go

-- ��
Create Table Field (
	[ID]			Int Identity Primary Key,		-- ��������
	[Name]			NVarchar(128) Not Null,			-- ������
	[FieldGroupID]	Int Not Null,					-- ��������ID
	[DataType]		Varchar(32) Not Null,			-- ������
	[Desc]			NVarchar(Max)					-- ��ע
);

Go

-- �ֱ�����ֱ�
Create Table PartialTable (
	[ID]			Int Identity Primary Key,		-- ��������
	[FieldGroupID]	Int Not Null,					-- ����ID
	[Index]			Int Not Null,					-- �÷ֱ�������ţ�ʵ�ʵı�����ʽΪ��UnionTableName_F_GroupName_{Index:4}��
	ConnectionID	Int Not Null,					-- ���ݿ�����
	[Desc]			NVarchar(Max)					-- ��ע
);

Go

-- �����
Create Table ImageTable (
	[ID]			Int Identity Primary Key,		-- ��������
	[TableID]		Int Not Null,					-- ��ID
	[Name]			Varchar(128)	Not Null,		-- ���ƣ�ʵ�ʵı�����ʽΪ��UnionTableName_I_Name��
	[ConnectionID]	Int Not Null,					-- ���ݿ�����ID
	[Desc]			NVarchar(Max),					-- ��ע
);

Go

-- �����ֶα�
Create Table ImageTableField (
	[ID]			Int Identity Primary Key,		-- ��������
	[ImageTableID]	Int Not Null,					-- �����ID
	[FieldID]		Int Not Null,					-- �ֶ�ID
	[Desc]			NVarchar(Max),					-- ��ע
);

Go

-- ������
Create Table [Index] (
	[ID]			Int Identity Primary Key,		-- ��������
	[ImageTableID]	Int Not Null,					-- �����ID
	[Desc]			NVarchar(Max) Not Null			-- ��ע
);

Go

-- �����ֶα�
Create Table IndexField (
	[ID]			Int Identity Primary Key,		-- ��������
	[IndexID]		Int Not Null,					-- ������ID
	[ImageTableFieldID]		Int Not Null,			-- �ֶ�ID
	[Desc]			NVarchar(Max)					-- ��ע
);

Go

-- ����������
Create Table PrimaryKey (
	[TableID]		Int Primary Key,				-- ���ID
	[CurrentValue]	Int Not Null Default 1,			-- ��ǰֵ
	[Desc]			NVarchar(Max)					-- ��ע
);

Go

If OBJECT_ID('GetPrimaryKeys') Is Not Null Begin
	Drop Procedure GetPrimaryKeys;
End;

Go

-- ��ȡ�����Ĵ洢����
Create Procedure GetPrimaryKeys (
	@tableId Int,
	@count Int
) As Begin

	Declare @table As Table ( Value Int Not Null );

	Begin Tran MyTran;
	
		Declare @currentValue As Int;
		Select @currentValue = CurrentValue From PrimaryKey RowLock Where TableID = @tableId;
		If @currentValue Is Null Begin
			Set @currentValue = 1;
			Insert Into PrimaryKey (TableID, CurrentValue) Values(@tableId, @currentValue);
		End;
		
		Update PrimaryKey Set CurrentValue = (CurrentValue + @count) Where TableID = @tableId;
		Select @currentValue;
	
	Commit Tran MyTran;

End;


/*
Truncate Table Connection;
Truncate Table UnionTable;
Truncate Table FieldGroup;
Truncate Table Field;
Truncate Table PartialTable;
Truncate Table ImageTable;
Truncate Table ImageTableField;
Truncate Table [Index];
Truncate Table IndexField;
Truncate Table PrimaryKey;
*/

/*
Drop Table Connection;
Drop Table UnionTable;
Drop Table FieldGroup;
Drop Table Field;
Drop Table PartialTable;
Drop Table ImageTable;
Drop Table ImageTableField;
Drop Table [Index];
Drop Table IndexField;
Drop Table PrimaryKey;
*/


--Select * From PrimaryKey