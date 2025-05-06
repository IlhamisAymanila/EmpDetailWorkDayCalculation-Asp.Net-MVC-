using EmpDetailWorkDayCalculation.DataContext;
using EmpDetailWorkDayCalculation.DTO;
using EmpDetailWorkDayCalculation.Models;
using EmpDetailWorkDayCalculation.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmpDetailWorkDayCalculation.Service.Repository
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly WaterLilyDbContext? waterLilyDbContext;

        public HolidayRepository(WaterLilyDbContext waterLilyDbContext)
        {
            this.waterLilyDbContext = waterLilyDbContext;
        }

        public async Task AddHolidayAsync(PublicHolidays publicHolidays)
        {
            waterLilyDbContext.Holidays.Add(publicHolidays);

            await waterLilyDbContext.SaveChangesAsync();
        }

        public async Task<WorkingDaysResultDto> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate)
        {
            var publicHolidays = await waterLilyDbContext.Holidays
                .Where(h => h.HolidayDate >= startDate && h.HolidayDate <= endDate)
                .Select(h => h.HolidayDate)
                .ToListAsync();

            List<DayDetail> dayDetails = new List<DayDetail>();
            int workingDays = 0;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                bool isPublicHoliday = publicHolidays.Contains(date);

                if (!isWeekend && !isPublicHoliday)
                {
                    workingDays++;
                }

                dayDetails.Add(new DayDetail
                {
                    Date = date,
                    DayOfWeek = date.DayOfWeek.ToString(),
                    IsWeekend = isWeekend,
                    IsPublicHoliday = isPublicHoliday
                });
            }

            return new WorkingDaysResultDto
            {
                TotalWorkingDays = workingDays,
                DayDetails = dayDetails
            };
        }



        public async Task<List<PublicHolidays>> GetAllAsync()
        {
            return await waterLilyDbContext.Holidays.ToListAsync();
        }

        public async Task<List<DateTime>> GetAllDateOnlyAsync()
        {
            return await waterLilyDbContext.Holidays.Select(h=>h.HolidayDate).ToListAsync();
        }
    }
}
