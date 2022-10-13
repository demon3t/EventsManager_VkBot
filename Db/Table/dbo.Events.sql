CREATE TABLE [dbo].[Events] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Author]     NVARCHAR (20)  NOT NULL,
    [CreateTime] DATETIME       NOT NULL,
    [Actual]     BIT            NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [Place]      NVARCHAR (50)  NOT NULL,
    [Seats]      INT            NOT NULL,
    [Describe]   NVARCHAR (MAX) NOT NULL,
    [StartTime]  DATETIME       NOT NULL,
    [EndTime]    DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

