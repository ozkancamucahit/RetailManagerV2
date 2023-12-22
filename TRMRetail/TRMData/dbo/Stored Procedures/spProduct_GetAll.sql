CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
/*
	EXEC spProduct_GetAll
*/

BEGIN

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
	ORDER BY P.ProductName ASC
END
