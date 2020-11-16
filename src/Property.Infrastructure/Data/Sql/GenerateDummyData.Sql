-- =============================================
-- Author:		Blake Byron
-- Create date: 13/11/20202
-- Description:	Creates a large amount (100000 records) of dummy data for testing api. Provides an additional source of data from that created by the application.
-- =============================================
TRUNCATE TABLE dbo.tbl_Property
GO
WITH
	L0   AS(SELECT 1 AS c UNION ALL SELECT 1),
	L1   AS(SELECT 1 AS c FROM L0 AS A CROSS JOIN L0 AS B),
	L2   AS(SELECT 1 AS c FROM L1 AS A CROSS JOIN L1 AS B),
	L3   AS(SELECT 1 AS c FROM L2 AS A CROSS JOIN L2 AS B),
	L4   AS(SELECT 1 AS c FROM L3 AS A CROSS JOIN L3 AS B),
	L5   AS(SELECT 1 AS c FROM L4 AS A CROSS JOIN L4 AS B),
	Tally AS(SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n FROM L5)
INSERT INTO dbo.tbl_Property(PropertyReference, Description)
SELECT TOP (100000)'P'+ CAST(N as nvarchar(10)), 'Description ' + CAST(N as nvarchar(10))
FROM Tally ORDER BY N 