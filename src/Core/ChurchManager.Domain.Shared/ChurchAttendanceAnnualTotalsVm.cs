namespace ChurchManager.Domain.Shared
{
    public record ChurchAttendanceMonthlyTotalsVm
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public long TotalAttendance { get; set; }
        public long? TotalNewConverts { get; set; }
        public long? TotalFirstTimers { get; set; }
    }
}
