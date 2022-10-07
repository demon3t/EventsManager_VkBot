CREATE TABLE [dbo].[Users] (
    [Id]        NVARCHAR (20) NOT NULL,
    [Name]      NVARCHAR (20) NULL,
    [Surname]   NVARCHAR (20) NULL,
    [Admin]     BIT           NOT NULL,
    [Make]      BIT           NOT NULL,
    [MakeState] INT           NOT NULL,
    [Mark]      BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

