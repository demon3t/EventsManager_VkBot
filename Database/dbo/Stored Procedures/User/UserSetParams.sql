CREATE PROCEDURE [dbo].[UserSetParams]
	@Id NVARCHAR(20),
    @Name NVARCHAR(20) = NULL,
    @Surname NVARCHAR(20) = NULL,
    @Admin BIT = NULL,
	@Major INT = NULL,
	@Minor INT = NULL
AS
BEGIN
	IF (@Name IS NOT NULL) BEGIN
	UPDATE Users SET Name = @Name
	WHERE Id = @Id END
	
	IF (@Surname IS NOT NULL) BEGIN
	UPDATE Users SET Surname = @Surname
	WHERE Id = @Id END
	
	IF (@Admin IS NOT NULL) BEGIN
	UPDATE Users SET Admin = @Admin
	WHERE Id = @Id END

	IF (@Major IS NOT NULL) BEGIN
	UPDATE Users SET Major = @Major
	WHERE Id = @Id END

	IF (@Minor IS NOT NULL) BEGIN
	UPDATE Users SET Minor = @Minor
	WHERE Id = @Id END

END