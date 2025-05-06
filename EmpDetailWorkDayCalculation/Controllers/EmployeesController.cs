using EmpDetailWorkDayCalculation.Models;
using EmpDetailWorkDayCalculation.Service.Interface;
using EmpDetailWorkDayCalculation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static EmpDetailWorkDayCalculation.Service.Interface.IEmpRepository;

namespace EmpDetailWorkDayCalculation.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmpRepository empRepository;
        private readonly IMemoryCache memoryCache;

        private readonly string cacheKey = "EmpCacheKey";

        public EmployeesController(IEmpRepository empRepository, IMemoryCache memoryCache)
        {
            this.empRepository = empRepository;
            this.memoryCache = memoryCache;
        }

        public IActionResult EmployeesHome()
        {
            return View("EmployeeManagment");
        }

        public async Task<IActionResult> EmployeeManagmnet(int id)
        {
            if (id > 0 ) 
            {
                var empModel = new EmployeeViewModel();
                var empRepoResult = await empRepository.GetEmployeeByIdAsync(id);

                empModel.Id = empRepoResult.Id;
                empModel.Name = empRepoResult.Name;
                empModel.Email = empRepoResult.Email;
                empModel.JobPosition = empRepoResult.JobPosition;

                return PartialView("EmployeesCreateOrUpdate", empModel);
            }
            else
            {
                var empModel = new EmployeeViewModel();
                empModel.Id = 0;
                return PartialView("EmployeesCreateOrUpdate", empModel);
            }
            
        }

        public async Task<IActionResult> CreateEmployee(string name, string email, string jobD)
        {
            try
            {
                var empModel = new Employees();

                empModel.Name = name;
                empModel.Email = email;
                empModel.JobPosition = jobD;

                await empRepository.AddEmployeeAsync(empModel);

                memoryCache.Remove(cacheKey);

                return Json(new { isOk = true, message = "Employee Create Successfull" });
            }
            catch (Exception ex)
            {

                return Json(new { isOk = false, message = ex });
            }


        }

        public async Task<IActionResult> GetAllEmployee()
        {
            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<Employees> empGet))
            {
                Console.WriteLine("Employees Not found in cache, fetching from DB");

                empGet = await empRepository.GetAllEmployeesAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5700))
                    .SetPriority(CacheItemPriority.Normal);

                memoryCache.Set(cacheKey, empGet, cacheEntryOptions);
            }
            else
            {
                Console.WriteLine("Employees found in cache");
            }

            return PartialView("EmployeeTable", empGet);
        }





        public async Task<IActionResult> UpdateEmpDetail(string name, string email, string jobD,int id)
        {
            try
            {
                var empModel = new Employees();
                

                empModel.Id = id;
                empModel.Name = name;
                empModel.Email = email;
                empModel.JobPosition = jobD;

                await empRepository.UpdateEmployeeAsync(empModel);
                memoryCache.Remove(cacheKey);
                return Json(new { isOk = true, message = "Employee Update Successfull" });
            }
            catch (Exception ex)
            {

                return Json(new { isOk = false, message = ex });
            }
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
           
                await empRepository.DeleteEmployeeAsync(id);
            memoryCache.Remove(cacheKey);
            return Json(new { isOk = true, message = "Employee Deleted Successfull" });
            
            

        }
    }
}
