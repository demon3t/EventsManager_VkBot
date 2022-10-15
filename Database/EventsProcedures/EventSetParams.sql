CREATE PROCEDURE [dbo].[EventSetParams]
    @Id INT,
	@Actual BIT = NULL,
 	@Name NVARCHAR(20) = NULL,
	@Place NVARCHAR(50) = NULL,
	@Seats INT = NULL,
	@Describe NVARCHAR(MAX) = NULL,
	@StartTime DATETIME = NULL,
	@EndTime DATETIME = NULL
AS
BEGIN

	IF (@Actual IS NOT NULL) BEGIN
	UPDATE Events SET Actual = @Actual
	WHERE Id = @Id END

	IF (@Name IS NOT NULL) BEGIN
	UPDATE Events SET Name = @Name
	WHERE Id = @Id END

	IF (@Place IS NOT NULL) BEGIN
	UPDATE Events SET Place = @Place
	WHERE Id = @Id END

	IF (@Seats IS NOT NULL) BEGIN
	UPDATE Events SET Seats = @Seats
	WHERE Id = @Id END

	IF (@Describe IS NOT NULL) BEGIN
	UPDATE Events SET Describe = @Describe
	WHERE Id = @Id END

	IF (@StartTime IS NOT NULL) BEGIN
	UPDATE Events SET StartTime = @StartTime
	WHERE Id = @Id END

	IF (@EndTime IS NOT NULL) BEGIN
	UPDATE Events SET EndTime = @EndTime
	WHERE Id = @Id END
	
END