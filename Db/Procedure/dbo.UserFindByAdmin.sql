﻿CREATE PROCEDURE [UserFindByAdmin]
@Admin BIT
AS
BEGIN
	SELECT * FROM Users
	WHERE Admin = @Admin
END