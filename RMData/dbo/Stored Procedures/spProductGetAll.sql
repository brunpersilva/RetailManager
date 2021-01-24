CREATE PROCEDURE [dbo].[spProductGetAll]
AS
begin
set nocount on;

	SELECT ProductName, [Description], RetailPrice, QuantityinStock
	from dbo.Product
	order by ProductName;
end
