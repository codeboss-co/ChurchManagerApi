using System;

namespace ChurchManager.Domain.Features.Groups
{
    #region Enumerations

    /// <summary>
    /// Schedule Type
    /// </summary>
    [Flags]
    public enum ScheduleType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Daily
        /// </summary>
        Daily = 1,

        /// <summary>
        /// Weekly
        /// </summary>
        Weekly = 2,

        /// <summary>
        /// Monthly
        /// </summary>
        Monthly = 3,

        /// <summary>
        /// Custom
        /// </summary>
        Custom = 4,
    }

    #endregion
}
