
Create Table LargeDataTest (
	ID		Int Identity Primary Key Not Null,
	Title	Varchar(256)
);

Declare @index As Int;
Set @index = 0;
While @index < 1000 * 10000 Begin

	Declare @title As Varchar(256);
	Set @title = 'TITLE_' + '_' + CONVERT(Varchar, @index);
	Insert Into LargeDataTest (Title) Values (@title);

	Set @index = @index + 1;
End;

Select COUNT(*) From LargeDataTest;