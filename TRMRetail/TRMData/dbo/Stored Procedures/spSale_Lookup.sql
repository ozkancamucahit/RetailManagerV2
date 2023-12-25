CREATE PROCEDURE [dbo].[spSale_Lookup]
	@CashierId NVARCHAR(128)
	,@SaleDate DATETIME2
AS
/*
	EXEC [dbo].[spSale_Lookup]
*/
BEGIN
	SET NOCOUNT ON;

	SELECT
	S.Id
	FROM
	[dbo].[Sale] AS S (NOLOCK)
	WHERE
	S.CashierId = @CashierId;

END


