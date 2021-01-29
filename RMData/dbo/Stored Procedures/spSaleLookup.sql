CREATE PROCEDURE [dbo].[spSaleLookup]
	@CashierId nvarchar(128),
	@SaleDate datetime2

AS
	begin 
	set nocount on

	select  Id from Sale
	where CashierId = @CashierId and SaleDate = @SaleDate;

end