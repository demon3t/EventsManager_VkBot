CREATE PROCEDURE [dbo].[UserSetParams]
    @Name NVARCHAR(20) = NULL,
    @Surname NVARCHAR(20) = NULL,
    @Admin BIT = NULL,
	@Major INT = NULL,
	@Minor INT = NULL
AS
BEGIN
	IF (@Name IS NOT NULL) BEGIN
	UPDATE Users SET
	Name = @Name END
	
	IF (@Surname IS NOT NULL) BEGIN
	UPDATE Users SET
	Surname = @Surname END
	
	IF (@Admin IS NOT NULL) BEGIN
	UPDATE Users SET
	Admin = @Admin END

	IF (@Major IS NOT NULL) BEGIN
	UPDATE Users SET
	Major = @Major END

	IF (@Minor IS NOT NULL) BEGIN
	UPDATE Users SET
	Minor = @Minor END

END