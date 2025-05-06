using EmpDetailWorkDayCalculation.DTO;
using EmpDetailWorkDayCalculation.Models;
using EmpDetailWorkDayCalculation.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;

namespace EmpDetailWorkDayCalculation.Controllers
{
    public class HollidayController : Controller
    {
        private readonly IHolidayRepository holidayRepository;
        private readonly IMemoryCache memoryCache;



        private readonly string cacheKey = "holidayCacheKey";

        public HollidayController(IHolidayRepository holidayRepository, IMemoryCache memoryCache)
        {
            this.holidayRepository = holidayRepository;
            this.memoryCache = memoryCache;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HolidayPage()
        {
            return View("HolidayHomePage");
        }

        public IActionResult CreateDate()
        {

            return PartialView("_AddHoliday");

        }


        public IActionResult CalculateDate()
        {
            return PartialView("_CalculateWorkingDay");
        }

        public async Task<IActionResult> InsertDate(DateTime newDate, string holidayDescription)
        {
            var holiday = new PublicHolidays();

            holiday.HolidayDate = newDate;
            holiday.Description = holidayDescription;

            await holidayRepository.AddHolidayAsync(holiday);
            memoryCache.Remove(cacheKey);
            return Json(new { isOk = true, message = "Public Holiday Create Successfull" });
        }

        public async Task<IActionResult> GetAllDate()
        {
            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<PublicHolidays> holidays))
            {
                Console.WriteLine("Holiday found in cache");
            }
            else
            {
                Console.WriteLine("Holiday Not found in cahche");

                holidays = await holidayRepository.GetAllAsync();

                var cachEntryOption = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5700))
                    .SetPriority(CacheItemPriority.Normal);

                memoryCache.Set(cacheKey, holidays, cachEntryOption);
            }
            return PartialView("_HolidayList", holidays);
        }

        public async Task<IActionResult> CalculateHoliday(DateTime startDate, DateTime endDate)
        {

            if (startDate > endDate)
            {
                return Json(new { isOk = false, message = "Date range is no valid" });
            }
            else if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return Json(new { isOk = false, message = "Start date cant be weekend days" });
            }

            var workdayResult = new WorkingDaysResultDto();

            workdayResult = await holidayRepository.CalculateWorkingDaysAsync(startDate, endDate);

            

            return PartialView("_WorkingDayResult", workdayResult);

           

        }

       
    }
}
