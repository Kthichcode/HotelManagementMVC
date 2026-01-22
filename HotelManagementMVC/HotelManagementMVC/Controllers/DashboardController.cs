using Services;
using HotelManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            var dto = _dashboardService.GetDashboardData();
            
            // AutoMapper is ideal here, but manual mapping is fine for now
            var model = new DashboardViewModel
            {
                TotalRooms = dto.TotalRooms,
                AvailableRooms = dto.AvailableRooms,
                MaintenanceRooms = dto.MaintenanceRooms,
                BookingsToday = dto.BookingsToday,
                BookingsThisMonth = dto.BookingsThisMonth,
                RevenueToday = dto.RevenueToday,
                RevenueThisMonth = dto.RevenueThisMonth,
                TopRoomTypes = dto.TopRoomTypes.Select(t => new TopRoomTypeViewModel
                {
                    RoomTypeName = t.RoomTypeName,
                    BookingCount = t.BookingCount
                }).ToList()
            };

            return View(model);
        }
    }
}
