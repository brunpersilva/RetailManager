CREATE PROCEDURE [dbo].[spSale_SaleReport]
As

begin
set nocount on;

select [S].[SaleDate], [S].[SubTotal], [S].[Tax],
[S].[Total], u.FirstName, u.LastName, u.EmailAdress 

from dbo. Sale S
inner join dbo.[User] U on S.CashierId = U.Id

end