
SELECT t2.a.value(N'@href', N'nvarchar(max)')
FROM   tbl_HomepageContent
		CROSS APPLY (select CAST(Content AS xml)) as t1(c)
		CROSS APPLY t1.c.nodes('/div/div/a') AS t2(a)
WHERE AreaID = 1194
