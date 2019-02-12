
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

-- 表的连接
Create Table Connection (
	[ID]		Int Identity Primary Key,		-- 递增主键
	[Name]		NVarchar(128) Not Null,			-- 连接名称
	DataSource	Varchar(128) Not Null,			-- 数据源（主机名称或ip）
	DbName		Varchar(128) Not Null,			-- 数据库名称
	UserName	Varchar(128) Not Null,			-- 用户名
	[Password]	Varchar(128) Not Null,			-- 密码
	[Desc]		NVarchar(Max)					-- 备注
);

Go

-- 表
Create Table UnionTable (
	[ID]				Int Identity Primary Key,	-- 递增主键
	[Name]				NVarchar(128) Not Null,		-- 表名
	[PrimaryKey]		NVarchar(128) Not Null,		-- 主键名称
	[PrimaryKeyType]	Varchar(32) Not Null,		-- 主键类型
	[RouteKey]			NVarchar(128) Not Null,		-- 路由键名称
	[RouteKeyType]		Varchar(32) Not Null,		-- 路由键类型
	[DefaultGroup]		NVarchar(128) Not Null,		-- 默认分组
	[Desc]				NVarchar(Max)				-- 备注
);

Go

-- 列组（纵向分表）
Create Table FieldGroup (
	[ID]			Int Identity Primary Key,		-- 递增主键
	[Name]			NVarchar(128) Not Null,			-- 列组名称
	[TableID]		Int Not Null,					-- 所属表的ID
	[PartialCount]	Int Not Null,					-- 分表数量
	[RouteType]		Varchar(32) Not Null,			-- 数据路由类型（决定什么样的数据应该从哪个表中读取）
	[RouteArgs]		Varchar(Max),					-- 数据路由参数
	[Desc]			NVarchar(Max)					-- 备注
);

Go

-- 列
Create Table Field (
	[ID]			Int Identity Primary Key,		-- 递增主键
	[Name]			NVarchar(128) Not Null,			-- 列名称
	[FieldGroupID]	Int Not Null,					-- 所属列组ID
	[DataType]		Varchar(32) Not Null,			-- 列类型
	[Desc]			NVarchar(Max)					-- 备注
);

Go

-- 分表（横向分表）
Create Table PartialTable (
	[ID]			Int Identity Primary Key,		-- 递增主键
	[FieldGroupID]	Int Not Null,					-- 列组ID
	[Index]			Int Not Null,					-- 该分表的索引号（实际的表名格式为：UnionTableName_F_GroupName_{Index:4}）
	ConnectionID	Int Not Null,					-- 数据库连接
	[Desc]			NVarchar(Max)					-- 备注
);

Go

-- 镜像表
Create Table ImageTable (
	[ID]			Int Identity Primary Key,		-- 递增主键
	[TableID]		Int Not Null,					-- 表ID
	[Name]			Varchar(128)	Not Null,		-- 名称（实际的表名格式为：UnionTableName_I_Name）
	[ConnectionID]	Int Not Null,					-- 数据库连接ID
	[Desc]			NVarchar(Max),					-- 备注
);

Go

-- 镜像字段表
Create Table ImageTableField (
	[ID]			Int Identity Primary Key,		-- 递增主键
	[ImageTableID]	Int Not Null,					-- 镜像表ID
	[FieldID]		Int Not Null,					-- 字段ID
	[Desc]			NVarchar(Max),					-- 备注
);

Go

-- 索引表
Create Table [Index] (
	[ID]			Int Identity Primary Key,		-- 递增主键
	[ImageTableID]	Int Not Null,					-- 镜像表ID
	[Desc]			NVarchar(Max) Not Null			-- 备注
);

Go

-- 索引字段表
Create Table IndexField (
	[ID]			Int Identity Primary Key,		-- 递增主键
	[IndexID]		Int Not Null,					-- 索引表ID
	[ImageTableFieldID]		Int Not Null,			-- 字段ID
	[Desc]			NVarchar(Max)					-- 备注
);

Go

-- 递增主键表
Create Table PrimaryKey (
	[TableID]		Int Primary Key,				-- 表的ID
	[CurrentValue]	Int Not Null Default 1,			-- 当前值
	[Desc]			NVarchar(Max)					-- 备注
);

Go

If OBJECT_ID('GetPrimaryKeys') Is Not Null Begin
	Drop Procedure GetPrimaryKeys;
End;

Go

-- 获取主键的存储过程
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