using EmpDetailWorkDayCalculation.Models;
using EmpDetailWorkDayCalculation.ViewModels;

namespace EmpDetailWorkDayCalculation.Service.Interface
{
    
    public interface IEmpRepository
    {

        Task AddEmployeeAsync(Employees employee);

        Task<List<Employees>> GetAllEmployeesAsync();

        Task<Employees> GetEmployeeByIdAsync(int id);

        Task UpdateEmployeeAsync(Employees employees);

        Task DeleteEmployeeAsync(int id);

        

    }

    
}
