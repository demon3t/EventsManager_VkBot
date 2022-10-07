CREATE PROCEDURE [UserInfo]
	@Id NVARCHAR (20)
AS
BEGIN
	SELECT
	Id, Name, Surname, Admin, Make, Mark
	FROM Users
	WHERE Id = @Id
END