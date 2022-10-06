CREATE TABLE [dbo].[Events] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Actual]   BIT           NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [Count]    INT           NOT NULL,
    [Describe] VARCHAR (MAX) NULL,
    [Date]     DATE          NULL,
    [Time]     TIME (7)      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

