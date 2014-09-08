DECLARE @reportDate DATETIME = '2014-09-08'
DECLARE @startMonthDate DATETIME = DATEADD(month, -1, @reportDate)
DECLARE @startYearDate DATETIME = DATEADD(year, -1, @reportDate)


SELECT f.Name, fv.UserID, fv.DateViewed INTO #tmp
FROM dbo.lib_FileViews as fv
INNER JOIN dbo.lib_Files as f ON fv.FileID = f.FileId
WHERE fv.FileID IN
(
	-- Get files id from content
	SELECT DISTINCT SUBSTRING(fileLink, PATINDEX ('%FileID=%',fileLink) + 7, 100) fileIds
	FROM
	(
		SELECT t2.a.value(N'@href', N'nvarchar(max)') as fileLink
		FROM   tbl_HomepageContent
				CROSS APPLY (select CAST(Content AS xml)) as t1(c)
				CROSS APPLY t1.c.nodes('/div/div/a') AS t2(a)
		WHERE AreaID = 1194
	) as t3
)


--
SELECT u.UserName, Name as Report, CountMonth as 'Number of times viewed in last month', CountYear 'Number of times viewed in last year' FROM 
(
	SELECT t2.Name, t2.UserId, CountMonth, CountYear FROM
	(
		SELECT Name, UserID, count(*) as CountMonth FROM #tmp
		WHERE DateViewed BETWEEN @startMonthDate and @reportDate
		GROUP BY Name, UserID
	) as t1
	FULL JOIN 
	(
		SELECT Name, UserID, count(*) as CountYear FROM #tmp
		WHERE DateViewed BETWEEN @startYearDate and @reportDate
		GROUP BY Name, UserID
	) as t2 ON t1.Name = t2.Name and t1.UserId = t2.UserId
) as t3 
INNER JOIN aspnet_Users as u ON t3.UserId = u.UserId
ORDER BY CountYear desc, CountMonth desc


DROP TABLE #tmp
