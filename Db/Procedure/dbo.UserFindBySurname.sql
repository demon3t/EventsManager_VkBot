CREATE PROCEDURE [UserFindBySurname]
@Surname NVARCHAR(20)
AS
BEGIN
	SELECT * FROM Users
	WHERE Surname = @Surname
END