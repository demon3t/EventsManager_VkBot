CREATE PROCEDURE [EventFindByParams]
	@Id INT = NULL,
    @Author NVARCHAR(20) = NULL,
	@CreateTime DATETIME = NULL,
	@Actual BIT = NULL,
    @Name NVARCHAR(100) = NULL,
    @Place NVARCHAR(50) = NULL,
    @Seats INT = NULL,
	@Describe NVARCHAR(MAX) = NULL,
	@StartTime DATETIME = NULL,
	@EndTime DATETIME = NULL
AS
BEGIN
	SELECT * FROM Events
	WHERE 
	(@Id IS NULL OR Id = @Id ) AND
	(@Author IS NULL OR Author = @Author) AND
	(@CreateTime IS NULL OR CreateTime = @CreateTime) AND
	(@Actual IS NULL OR Actual = @Actual) AND
	(@Name IS NULL OR Name = @Name) AND
	(@Place IS NULL OR Place = @Place) AND
	(@Seats IS NULL OR Seats = @Seats) AND
	(@Describe IS NULL OR Describe = @Describe) AND
	(@StartTime IS NULL OR StartTime = @StartTime) AND
	(@EndTime IS NULL OR EndTime = @EndTime)
END