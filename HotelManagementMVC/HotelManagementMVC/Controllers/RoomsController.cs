using HotelManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public RoomsController(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }

        private void LoadRoomTypes(SearchRoomsViewModel model)
        {
            var types = _roomTypeService.GetAll();

            model.RoomTypes = new List<SelectListItem>();

            for (int i = 0; i < types.Count; i++)
            {
                var t = types[i];
                model.RoomTypes.Add(new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                });
            }
        }

        [HttpGet]
        public IActionResult Search()
        {
            var model = new SearchRoomsViewModel();
            model.CheckIn = DateTime.Today;
            model.CheckOut = DateTime.Today.AddDays(1);

            LoadRoomTypes(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SearchRoomsViewModel model)
        {
            LoadRoomTypes(model);

            if (model.CheckIn >= model.CheckOut)
            {
                ModelState.AddModelError("", "Check-out must be after check-in");
                return View(model);
            }

            var rooms = _roomService.GetAvailableRooms(model.CheckIn, model.CheckOut, model.RoomTypeId);

            // filter room number
            if (!string.IsNullOrWhiteSpace(model.RoomNumber))
            {
                string key = model.RoomNumber.Trim();
                var filtered = new List<BusinessObjects.Entities.Room>();

                for (int i = 0; i < rooms.Count; i++)
                {
                    var r = rooms[i];
                    if (r.RoomNumber != null && r.RoomNumber.Contains(key))
                    {
                        filtered.Add(r);
                    }
                }

                rooms = filtered;
            }

            // map result
            model.Results = new List<RoomResultViewModel>();

            for (int i = 0; i < rooms.Count; i++)
            {
                var r = rooms[i];

                var item = new RoomResultViewModel();
                item.RoomId = r.Id;
                item.RoomNumber = r.RoomNumber;
                item.RoomTypeName = r.RoomType != null ? r.RoomType.Name : "";
                item.PricePerNight = r.RoomType != null ? r.RoomType.PricePerNight : 0;

                // ✅ NEW (để hiện ảnh + max occupancy)
                item.ImageUrl = r.ImageUrl;
                item.MaxOccupancy = r.MaxOccupancy;

                model.Results.Add(item);
            }

            return View(model);
        }

        // ✅ NEW: View Details (xem ảnh + mô tả + số người)
        [HttpGet]
        public IActionResult Details(int id)
        {
            var room = _roomService.GetByIdWithImages(id);
            if (room == null) return NotFound();

            return View(room);
        }
    }
}
