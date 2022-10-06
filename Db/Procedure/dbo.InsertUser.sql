CREATE PROCEDURE [InsertUser]
    @Id VARCHAR(20),
    @Name VARCHAR(20),
    @Surname VARCHAR(20),
    @Admin BIT,
    @Make BIT,
    @Mark BIT
 
AS
    INSERT INTO [Users]
    (
    Id,
    Name,
    Surname,
    Admin,
    Make,
    Mark
    )
 
    VALUES
    (
    @Id,
    @Name,
    @Surname,
    @Admin,
    @Make,
    @Mark
    )
SELECT SCOPE_IDENTITY()