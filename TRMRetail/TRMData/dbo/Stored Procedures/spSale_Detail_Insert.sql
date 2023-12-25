CREATE PROCEDURE [dbo].[spSale_Detail_Insert]
	@SaleId INT
	,@ProductId INT
	,@Quantity INT
	,@PurchasePrice MONEY
	,@Tax MONEY
AS
/*
	EXEC [dbo].[spSale_Detail_Insert]
*/

BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[SaleDetail]
	( 
	 [SaleId], [ProductId], [Quantity], [PurchasePrice], [Tax]
	)
	VALUES
	( 
	 @SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax
	)

END

