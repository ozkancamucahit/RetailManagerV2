CREATE PROCEDURE [dbo].[spInventoryInsert]
	@ProductId INT,
	@Quantity INT,
	@PurchasePrice MONEY,
	@PurchaseDate DATETIME2
AS


BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Inventory]
	( 
	 [ProductId], [Quantity], [PurchasePrize], [PurchaseDate]
	)
	VALUES
	( 
	 @ProductId, @Quantity, @PurchasePrice, @PurchaseDate
	);

END



