CREATE PROCEDURE [dbo].[spUserLookUp]
	@Id NVARCHAR(128)

AS
/*
	EXEC spUserLookUp ''
*/

BEGIN
	SET NOCOUNT ON;
	
	SELECT
	U.Id
	,U.FirstName
	,U.LastName
	,U.EmailAddress
	,U.CreatedDate
	FROM
		[dbo].[User] AS U (NOLOCK)
	WHERE
	U.Id = @Id


END
