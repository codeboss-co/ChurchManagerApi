using System.Collections.Generic;

namespace ChurchManager.Core.Shared
{
    public record ChurchAttendanceAnnualBreakdownVm
    {
        public int Year { get; set; }
        public IEnumerable<ChurchAttendanceMonthlyTotalsVm> Data { get; set; }
    }

    public record ChurchAttendanceMonthlyTotalsVm
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalAttendance { get; set; }
    }
}
