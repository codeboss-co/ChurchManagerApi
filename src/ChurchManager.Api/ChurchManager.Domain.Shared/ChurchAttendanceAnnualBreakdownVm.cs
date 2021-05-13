using System.Collections.Generic;

namespace ChurchManager.Domain.Shared
{
    public record ChurchAttendanceAnnualBreakdownVm
    {
        public int Year { get; set; }
        public IEnumerable<ChurchAttendanceMonthlyTotalsVm> Data { get; set; }
    }
}