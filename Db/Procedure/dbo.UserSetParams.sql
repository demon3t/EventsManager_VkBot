CREATE PROCEDURE [UserSetParams]
	@Id NVARCHAR(20),
    @Name NVARCHAR(20),
    @Surname NVARCHAR(20),
    @Admin BIT,
	@Major INT,
	@Minor INT
AS
BEGIN
    UPDATE Users SET
	Name = @Name ,
    Surname = @Surname,
    Admin = @Admin,
	Major = @Major,
	Minor = @Minor
    WHERE Id = @Id
END