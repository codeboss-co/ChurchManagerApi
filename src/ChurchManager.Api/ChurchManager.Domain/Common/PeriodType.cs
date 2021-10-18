using System;

namespace ChurchManager.Domain.Common
{
    /// <summary>
    /// PeriodType Type
    /// </summary>
    [Flags]
    public enum PeriodType
    {
        /// <summary>
        /// Today
        /// </summary>
        Today = 10,

        /// <summary>
        /// Yesterday
        /// </summary>
        Yesterday = 20,

        /// <summary>
        /// ThisWeek
        /// </summary>
        ThisWeek = 30,

        /// <summary>
        /// LastWeek
        /// </summary>
        LastWeek = 40,

        /// <summary>
        /// ThisMonth
        /// </summary>
        ThisMonth = 50,

        /// <summary>
        /// LastMonth
        /// </summary>
        LastMonth = 60,

        ThisYear = 70,

        AllTime = 100,
    }

    /// <summary>
    /// ReportPeriodType Type
    /// </summary>
    [Flags]
    public enum ReportPeriodType
    {
        /// <summary>
        /// Day
        /// </summary>
        Day = 10,

        /// <summary>
        /// Month
        /// </summary>
        Week = 20,

        /// <summary>
        /// Month
        /// </summary>
        Month = 30,

        /// <summary>
        /// ThreeMonths
        /// </summary>
        ThreeMonths = 40,

        /// <summary>
        /// SixMonths
        /// </summary>
        SixMonths = 50,

        /// <summary>
        /// OneYear
        /// </summary>
        OneYear = 60
    }
}
