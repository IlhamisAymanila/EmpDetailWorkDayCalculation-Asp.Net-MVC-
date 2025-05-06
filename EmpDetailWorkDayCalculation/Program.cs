using EmpDetailWorkDayCalculation.DataContext;
using EmpDetailWorkDayCalculation.Service.Interface;
using EmpDetailWorkDayCalculation.Service.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<WaterLilyDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("EmpManagmentString")));

builder.Services.AddScoped<IEmpRepository, EmpRepository>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
