using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public HomeController(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }

        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            
            // Get featured rooms (first 6 available rooms with their images)
            var allRooms = _roomService.GetAll()
                .Where(r => r.Status == BusinessObjects.Enums.RoomStatus.Available)
                .Take(6)
                .ToList();
            
            // Get all room types for display
            var roomTypes = _roomTypeService.GetAll();
            
            ViewBag.FeaturedRooms = allRooms;
            ViewBag.RoomTypes = roomTypes;
            
            return View();
        }
    }
}
