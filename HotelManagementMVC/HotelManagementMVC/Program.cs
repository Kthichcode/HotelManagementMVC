using BusinessObjects;
using DataAccessObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;
using HotelManagementMVC.Data;

namespace HotelManagementMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1) DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // 2) Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // tuỳ chọn: password rules (đỡ khó test)
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
            
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();




            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await DbSeeder.SeedAsync(scope.ServiceProvider);
            }


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            app.Run();
        }
    }
}
