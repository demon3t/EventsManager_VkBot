CREATE PROCEDURE [InsertUser]
    @Id NVARCHAR(20),
    @Name NVARCHAR(20),
    @Surname NVARCHAR(20),
    @Admin BIT,
    @Make BIT,
	@MakeState FLOAT,
    @Mark BIT
 
AS
    INSERT INTO [Users]
    (
    Id,
    Name,
    Surname,
    Admin,
    Make,
	MakeState,
    Mark
    )
 
    VALUES
    (
    @Id,
    @Name,
    @Surname,
    @Admin,
    @Make,
	@MakeState,
    @Mark
    )
SELECT SCOPE_IDENTITY()