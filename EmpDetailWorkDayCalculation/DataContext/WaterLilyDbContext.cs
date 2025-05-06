using EmpDetailWorkDayCalculation.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpDetailWorkDayCalculation.DataContext
{
    public class WaterLilyDbContext : DbContext
    {
        public WaterLilyDbContext(DbContextOptions<WaterLilyDbContext> options) : base(options) { }

        public DbSet<Employees> Employees { get; set; }
        public DbSet<PublicHolidays> Holidays { get; set; }
    }
}
