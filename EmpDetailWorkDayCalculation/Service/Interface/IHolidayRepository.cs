using EmpDetailWorkDayCalculation.DTO;
using EmpDetailWorkDayCalculation.Models;

namespace EmpDetailWorkDayCalculation.Service.Interface
{
    public interface IHolidayRepository
    {
        Task AddHolidayAsync(PublicHolidays publicHolidays);

        Task<List<PublicHolidays>> GetAllAsync();

        Task<List<DateTime>> GetAllDateOnlyAsync();

        Task<WorkingDaysResultDto> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate);
    }
}

