CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id INT
AS

/*
	EXEC [dbo].[spProduct_GetById] 5
*/


BEGIN
	SET NOCOUNT ON;

	SELECT
	P.Id
	,P.ProductName
	,P.Description
	,P.RetailPrice
	,P.QuantityInStock
	,P.CreateDate
	,P.LastModified
	,P.IsTaxable
	FROM
		Product AS P (NOLOCK)
	WHERE
	P.Id = @Id;


END


