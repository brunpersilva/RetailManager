CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
	
AS
Begin
set nocount on

	SELECT Id, FirstName, LastName, EmailAdress, CreateDate 
	from [dbo].[User]
	where Id = @Id;
End
