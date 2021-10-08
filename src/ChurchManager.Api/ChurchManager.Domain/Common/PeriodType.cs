using System;

namespace ChurchManager.Domain.Common
{
    /// <summary>
    /// Schedule Type
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
    }
}
