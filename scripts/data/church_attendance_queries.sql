SELECT
       date_part('year', c."AttendanceDate")::INT AS "Year",
       date_part('month', c."AttendanceDate")::INT AS "Month",
       COALESCE(SUM(c."AttendanceCount"), 0)::INT AS "TotalAttendance",
       COALESCE(SUM(c."MalesCount"), 0)::INT AS "TotalMales",
       COALESCE(SUM(c."FemalesCount"), 0)::INT AS "TotalFemales",
       COALESCE(SUM(c."ChildrenCount"), 0)::INT AS "TotalChildren"
     , (COALESCE(SUM(c."ChildrenCount"), 0) / nullif(lag(c."ChildrenCount"), 0))
FROM "Churches"."ChurchAttendance" AS c
WHERE (c."AttendanceDate" >= '2020-09-1') AND (c."AttendanceDate" <= '2020-12-31')
GROUP BY date_part('year', c."AttendanceDate")::INT, date_part('month', c."AttendanceDate")::INT
ORDER BY date_part('year', c."AttendanceDate")::INT DESC, date_part('month', c."AttendanceDate")::INT DESC


SELECT
       date_part('year', c."AttendanceDate")::INT AS "Year",
       date_part('month', c."AttendanceDate")::INT AS "Month",
       COALESCE(SUM(c."AttendanceCount"), 0)::INT AS "TotalAttendance",
       c."ChurchId" AS "ChurchId",
	LAG(COALESCE(SUM(c."AttendanceCount"), 0),1) OVER (
		PARTITION BY "ChurchId"
		ORDER BY date_part('year', c."AttendanceDate")::INT DESC
	) previous_year_sales
FROM "Churches"."ChurchAttendance" AS c
    GROUP BY date_part('year', c."AttendanceDate")::INT
;


-- https://www.postgresqltutorial.com/postgresql-lag-function/
WITH cte AS (
	SELECT
        date_part('year', c."AttendanceDate") AS "Year",
		COALESCE(SUM(c."AttendanceCount"), 0) AS "TotalAttendance"
    FROM "Churches"."ChurchAttendance" AS c
	GROUP BY "Year"
	ORDER BY "Year"
)
SELECT
	"Year",
	"TotalAttendance",
	LAG("TotalAttendance",1) OVER ( ORDER BY "Year"	) previous_year_sales
FROM
	cte;

-- VARIANCE --

WITH cte AS (
	SELECT
        date_part('year', c."AttendanceDate") AS "Year",
		COALESCE(SUM(c."AttendanceCount"), 0) AS "TotalAttendance"
    FROM "Churches"."ChurchAttendance" AS c
	GROUP BY "Year"
	ORDER BY "Year"
), cte2 AS (
	SELECT
        "Year",
        "TotalAttendance",
        LAG("TotalAttendance",1) OVER ( ORDER BY "Year"	) previous_year_sales
	FROM
		cte
)
SELECT
    "Year",
    "TotalAttendance",
	previous_year_sales,
	(previous_year_sales - "TotalAttendance") variance
FROM
	cte2;