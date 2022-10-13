CREATE TABLE [dbo].[Users] (
    [Id]      NVARCHAR (20) NOT NULL,
    [Name]    NVARCHAR (20) NOT NULL,
    [Surname] NVARCHAR (20) NOT NULL,
    [Admin]   BIT           NOT NULL,
    [Major]   INT           NOT NULL,
    [Minor]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

