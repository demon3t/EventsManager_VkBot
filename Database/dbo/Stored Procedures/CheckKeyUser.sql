﻿CREATE PROCEDURE [CheckKeyUser]
	@Id NVARCHAR(50)
AS
BEGIN
	SELECT Id FROM Users
	WHERE Id = @Id
END