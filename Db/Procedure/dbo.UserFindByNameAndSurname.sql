CREATE PROCEDURE [UserFindByNameAndSurname]
@Param1 NVARCHAR (20),
@Param2 NVARCHAR (20)
AS
BEGIN
	SELECT DISTINCT * FROM Users
	WHERE
	(Name = @Param1 AND Surname = @Param2)
	OR
	(Name = @Param2 AND Surname = @Param1)
END