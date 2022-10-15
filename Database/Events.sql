/*
В базе данных должна быть предусмотрена файловая группа MEMORY_OPTIMIZED_DATA,
чтобы можно было создать оптимизированный для памяти объект.

Число сегментов должно быть задано как примерно в два раза 
превышающее максимально ожидаемое количество уникальных значений в 
ключе индекса с округлением до ближайшего четного числа.
*/

CREATE TABLE [dbo].[Events]
(
    [Id]         INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT = 131072),
    [Author]     NVARCHAR (20)  NOT NULL,
    [CreateTime] DATETIME       NOT NULL,
    [Actual]     BIT            NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [Place]      NVARCHAR (50)  NOT NULL,
    [Seats]      INT            NOT NULL,
    [Describe]   NVARCHAR (MAX) NOT NULL,
    [StartTime]  DATETIME       NOT NULL,
    [EndTime]    DATETIME       NOT NULL,
) WITH (MEMORY_OPTIMIZED = ON)