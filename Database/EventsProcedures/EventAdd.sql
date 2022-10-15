CREATE PROCEDURE [EventAdd]
	@Author NVARCHAR(20),
	@CreateTime DATETIME,
	@Actual BIT = 1,
    @Name NVARCHAR(100) = '',
    @Place NVARCHAR(50) = '',
    @Seats INT = 0,
	@Describe NVARCHAR(MAX) = '',
	@StartTime DATETIME = '0001-01-01',
	@EndTime DATETIME = '0001-01-01'
AS
    INSERT INTO [Events]
    (
	Author,
	CreateTime,
	Actual,
	Name,
	Place,
	Seats,
	Describe,
	StartTime,
	EndTime
	)
	VALUES
	(
	@Author,
	@CreateTime,
	@Actual,
	@Name,
	@Place,
	@Seats,
	@Describe,
	@StartTime,
	@EndTime
	)
SELECT SCOPE_IDENTITY()