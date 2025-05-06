using EmpDetailWorkDayCalculation.DataContext;
using EmpDetailWorkDayCalculation.Models;
using EmpDetailWorkDayCalculation.Service.Interface;
using EmpDetailWorkDayCalculation.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EmpDetailWorkDayCalculation.Service.Repository
{
    public class EmpRepository : IEmpRepository
    {
        
        private readonly WaterLilyDbContext? waterLilyDbContext;

        public EmpRepository(WaterLilyDbContext waterLilyDbContext)
        {
            this.waterLilyDbContext = waterLilyDbContext;
        }
        
        public async Task AddEmployeeAsync(Employees employee)
        {
          
            waterLilyDbContext.Employees.Add(employee);

            await waterLilyDbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var emp = waterLilyDbContext.Employees.FindAsync(id);

            if(emp != null)
            {
                waterLilyDbContext.Employees.Remove(await emp);

                await waterLilyDbContext.SaveChangesAsync();

            }
        }

        public async Task<List<Employees>> GetAllEmployeesAsync()
        {
            return await waterLilyDbContext.Employees.ToListAsync();
        }

        public async Task<Employees> GetEmployeeByIdAsync(int id)
        {
            return await waterLilyDbContext.Employees.FindAsync(id);
        }

        public async Task UpdateEmployeeAsync(Employees employees)
        {
            waterLilyDbContext.Employees.Update(employees);

            await waterLilyDbContext.SaveChangesAsync();
        }
    }
}
