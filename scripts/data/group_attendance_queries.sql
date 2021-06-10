SELECT
    EXTRACT (YEAR FROM "AttendanceDate") as year,
    EXTRACT (MONTH FROM "AttendanceDate") as month,
    --EXTRACT (WEEK FROM "AttendanceDate") as week,
    G."Name" as group,
    COUNT ("AttendanceCount") as attendance,
    COUNT ("FirstTimerCount") as firsttimers,
    COUNT ("NewConvertCount") as newconverts,
    COUNT ("ReceivedHolySpiritCount") as holyspirit
FROM
    "GroupAttendance"
JOIN "Group" G on G."Id" = "GroupAttendance"."GroupId"
WHERE "AttendanceDate" BETWEEN '2020-10-29' AND '2021-03-16'
GROUP BY
    ROLLUP (
        EXTRACT (YEAR FROM "AttendanceDate"),
        EXTRACT (MONTH FROM "AttendanceDate"),
        --EXTRACT (WEEK FROM "AttendanceDate"),
        G."Name"
    )
ORDER BY 1 DESC, 2 ASC, 3 ASC;


-- DATE BY WEEK and YEAR
SELECT TO_DATE('20211', 'yyyyww');
-- END OF WEEK
SELECT TO_DATE('20211', 'yyyyww') + 7; -- add 7 days

SELECT * FROM "GroupAttendance" WHERE "GroupId" = 16;


-- Day of the week
SELECT EXTRACT(DOW FROM DATE '2016-01-01') -- The day of the week (0 - 6; Sunday is 0)

