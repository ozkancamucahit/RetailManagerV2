CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
/*
    EXEC [dbo].[spInventory_GetAll]
*/
BEGIN
    SET NOCOUNT ON;

    SELECT
    [ProductId], 
    [Quantity], 
    [PurchasePrize], 
    [PurchaseDate]
    FROM
    [dbo].[Inventory]

END





