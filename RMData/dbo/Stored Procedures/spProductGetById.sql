CREATE PROCEDURE [dbo].[spProductGetById]
	@Id int
AS
Begin
set nocount on

	SELECT Id, ProductName, [Description], RetailPrice, QuantityinStock, IsTaxable
	from dbo.Product
	where Id = @Id;
end