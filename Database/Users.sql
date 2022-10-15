/*
В базе данных должна быть предусмотрена файловая группа MEMORY_OPTIMIZED_DATA,
чтобы можно было создать оптимизированный для памяти объект.

Число сегментов должно быть задано как примерно в два раза 
превышающее максимально ожидаемое количество уникальных значений в 
ключе индекса с округлением до ближайшего четного числа.
*/

CREATE TABLE [dbo].[Users]
(
    [Id]      NVARCHAR (20) NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 131072),
    [Name]    NVARCHAR (20) NOT NULL,
    [Surname] NVARCHAR (20) NOT NULL,
    [Admin]   BIT           NOT NULL,
    [Major]   INT           NOT NULL,
    [Minor]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)

) WITH (MEMORY_OPTIMIZED = ON)