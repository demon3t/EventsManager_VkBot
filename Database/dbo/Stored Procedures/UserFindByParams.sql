CREATE PROCEDURE [UserFindByParams]
	@Id NVARCHAR(20) = NULL,
    @Name NVARCHAR(20) = NULL,
	@Surname NVARCHAR(20) = NULL,
    @Admin BIT = NULL,
    @Major INT = NULL,
    @Minor INT = NULL
AS
BEGIN
	SELECT * FROM Users
	WHERE 
	(@Id IS NULL OR Id = @Id ) AND
	(@Name IS NULL OR Name = @Name) AND
	(@Surname IS NULL OR Surname = @Surname) AND
	(@Admin IS NULL OR Admin = @Admin) AND
	(@Major IS NULL OR Major = @Major) AND
	(@Minor IS NULL OR Minor = @Minor)
END