CREATE PROCEDURE [dbo].[sp_Sale_Insert]
	@Id INT = 0 OUTPUT
	,@CashierId NVARCHAR(128)
	,@SaleDate DATETIME2
	,@SubTotal MONEY
	,@Tax MONEY
	,@Total MONEY
AS

/*
	EXEC [dbo].[sp_Sale_Insert]
*/
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Sale]
	( 
	 [CashierId], [SaleDate], [SubTotal], [Tax], [Total]
	)
	VALUES
	( 
	 @CashierId, @SaleDate, @SubTotal, @Tax, @Total
	);


	SET @Id = SCOPE_IDENTITY();


END