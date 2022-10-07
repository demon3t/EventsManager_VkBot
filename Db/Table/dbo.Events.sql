CREATE TABLE [dbo].[Events] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Actual]   BIT            NOT NULL,
    [Name]     NVARCHAR (100) NOT NULL,
    [Place]    NVARCHAR (50)  NULL,
    [Count]    INT            NOT NULL,
    [Describe] NVARCHAR (MAX) NULL,
    [Date]     DATETIME       NULL,
    [Time]     DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

