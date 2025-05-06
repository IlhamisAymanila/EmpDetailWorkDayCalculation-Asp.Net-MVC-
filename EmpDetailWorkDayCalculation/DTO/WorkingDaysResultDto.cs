namespace EmpDetailWorkDayCalculation.DTO
{
    public class WorkingDaysResultDto
    {
        public int TotalWorkingDays { get; set; }
        public List<DayDetail> DayDetails { get; set; }
    }

    public class DayDetail
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsPublicHoliday { get; set; }
    }
}
