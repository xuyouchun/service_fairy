Alter Procedure DeleteTable ( @tableName As NVarchar(128) )
As Begin

	Declare @tableId As Int;
	Select @tableId = ID From UnionTable Where Name = @tableName;
	If @tableId Is Null Begin
		Print 'No Table';
		Return;
	End;

	Begin Tran myTran;
	
		Delete From t From IndexField t
			Join [Index] On t.IndexID = [index].ID
			Join [ImageTable] On [index].ImageTableID = [ImageTable].ID
			Where [ImageTable].TableID = @tableId;
			
		Delete From t From [Index] t
			Join [ImageTable] On ImageTableID = [ImageTable].ID
			Where [ImageTable].TableID = @tableId;
			
		Delete From t From [ImageTableField] t
			Join [ImageTable] On [ImageTable].ID = t.ImageTableID
			Where [ImageTable].TableID = @tableId;
		
		Delete From t From [ImageTable] t
			Where t.TableID = @tableId;
		
		Delete From t From [Field] t
			Join [FieldGroup] On t.FieldGroupID = [FieldGroup].ID
			Where [FieldGroup].TableID = @tableId;
		
		Delete From t From [PartialTable] t
			Join [FieldGroup] On t.FieldGroupID = [FieldGroup].ID
			Where [FieldGroup].TableID = @tableId;
		
		Delete From t From [FieldGroup] t
			Join [UnionTable] On t.TableID = [UnionTable].ID
			Where [UnionTable].ID = @tableId;
		
		Delete From [UnionTable]
			Where ID = @tableId;
		
		Delete From [PrimaryKey] Where TableID = @tableId;
		
	
	Commit Tran myTran;

	Print 'OK!';

End;