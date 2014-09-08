
-- Get files ids from content
SELECT DISTINCT SUBSTRING(fileLink, PATINDEX ('%FileID=%',fileLink) + 7, 100) fileIds
FROM
(
	SELECT t2.a.value(N'@href', N'nvarchar(max)') as fileLink
	FROM   tbl_HomepageContent
			CROSS APPLY (select CAST(Content AS xml)) as t1(c)
			CROSS APPLY t1.c.nodes('/div/div/a') AS t2(a)
	WHERE AreaID = 1194
) as t3
