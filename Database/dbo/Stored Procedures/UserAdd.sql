CREATE PROCEDURE [UserAdd]
    @Id NVARCHAR(20),
    @Name NVARCHAR(20) = '',
    @Surname NVARCHAR(20) = '',
    @Admin BIT = 0,
	@Major INT = 0,
	@Minor INT = 0
 
AS
    INSERT INTO [Users]
    (
    Id,
    Name,
    Surname,
    Admin,
	Major,
	Minor
    )
 
    VALUES
    (
    @Id,
    @Name,
    @Surname,
    @Admin,
	@Major,
	@Minor
 
    )
SELECT SCOPE_IDENTITY()