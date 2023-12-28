CREATE PROCEDURE [dbo].[spSale_SaleReport]

AS
BEGIN 
	
    SET NOCOUNT ON;

    SELECT
    S.SaleDate
    ,S.SubTotal
    ,S.Tax
    ,S.Total
    ,U.FirstName
    ,U.LastName
    ,U.EmailAddress
    FROM
    [dbo].[Sale] AS S (NOLOCK)
    INNER JOIN [dbo].[User] AS U ON S.CashierId = U.Id;

END
