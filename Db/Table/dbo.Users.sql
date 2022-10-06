CREATE TABLE [dbo].[Users] (
    [Id]      VARCHAR (20) NOT NULL,
    [Name]    VARCHAR (20) NULL,
    [Surname] VARCHAR (20) NULL,
    [Admin]   BIT          NOT NULL,
    [Make]    BIT          NOT NULL,
    [Mark]    BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

