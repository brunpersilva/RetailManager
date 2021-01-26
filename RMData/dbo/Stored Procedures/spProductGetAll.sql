CREATE PROCEDURE [dbo].[spProductGetAll]
AS
begin
set nocount on;

	SELECT ProductName, [Description], RetailPrice, QuantityinStock, IsTaxable
	from dbo.Product
	order by ProductName;
end
