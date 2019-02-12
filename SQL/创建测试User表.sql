
Create Table User_Basic_0000 (
	ID			Int Primary Key,
	LoginName	Varchar(50) Not Null,
	LoginNameType Int Not Null,
	NickName	NVarchar(20) Not Null
);

Go

Create Table User_Basic_0001 (
	ID			Int Primary Key,
	LoginName	Varchar(50) Not Null,
	LoginNameType Int Not Null,
	NickName	NVarchar(20) Not Null
);

Go

Create Table User_Security_0000 (
	ID			Int Primary Key,
	[Password]	Varchar(20) Not Null,
	[Sid]		Varchar(50) Not Null
);

Create Table User_Security_0001 (
	ID			Int Primary Key,
	[Password]	Varchar(20) Not Null,
	[Sid]		Varchar(50) Not Null
);

Create Table User_Index_LoginName (
	ID			Int Not Null,
	LoginName	Varchar(50) Not Null
);

Create Unique Index Index_User_Index_LoginName On User_Index_LoginName (
	LoginName
);

Go

/*

Truncate Table dbo.User_F_Basic_0000;
Truncate Table dbo.User_F_Basic_0001
Truncate Table dbo.User_I_Image1
Truncate Table dbo.User_F_Security_0000
Truncate Table dbo.User_F_Security_0001
*/

/*
Select * From dbo.User_F_Basic_0000;
Select * From dbo.User_F_Basic_0001
Select * From dbo.User_I_Image1
Select * From dbo.User_F_Security_0000
Select * From dbo.User_F_Security_0001
*/



Declare @idTable As Table( [LineNumber] Int Identity Primary Key, [ID] Int);
Insert Into @idTable ([ID]) Select Top 10 [ID] From User_I_Image1;  -- Where Basic.LoginName > 0;
Select * From @idTable a Join User_I_Image1 b On a.ID = b.ID Where [LineNumber] > 1;





