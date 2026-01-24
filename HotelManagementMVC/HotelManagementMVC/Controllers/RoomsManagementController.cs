using BusinessObjects.Entities;
using BusinessObjects.Enums;
using HotelManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class RoomsManagementController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public RoomsManagementController(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }

        public IActionResult Index()
        {
            var rooms = _roomService.GetAll();
            return View(rooms);
        }

        private void LoadDropdowns(RoomFormViewModel model)
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

            model.StatusOptions = new List<SelectListItem>();
            model.StatusOptions.Add(new SelectListItem { Value = "1", Text = "Available" });
            model.StatusOptions.Add(new SelectListItem { Value = "2", Text = "Maintenance" });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RoomFormViewModel();
            model.Status = 1;
            LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(RoomFormViewModel model)
        {
            LoadDropdowns(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (_roomService.GetAll().Any(r => r.RoomNumber.Equals(model.RoomNumber, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("RoomNumber", "Room Number already exists.");
                    return View(model); // Dropdowns are reloaded at start of method
                }

                var room = new Room();
                room.RoomNumber = model.RoomNumber;
                room.RoomTypeId = model.RoomTypeId;
                room.Status = (RoomStatus)model.Status;

                _roomService.Create(room);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var room = _roomService.GetById(id);
            if (room == null) return NotFound();

            var model = new RoomFormViewModel();
            model.Id = room.Id;
            model.RoomNumber = room.RoomNumber;
            model.RoomTypeId = room.RoomTypeId;
            model.Status = (int)room.Status;

            LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(RoomFormViewModel model)
        {
            LoadDropdowns(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var room = _roomService.GetById(model.Id);
                if (room == null) return NotFound();

                if (_roomService.GetAll().Any(r => r.RoomNumber.Equals(model.RoomNumber, StringComparison.OrdinalIgnoreCase) && r.Id != room.Id))
                {
                    ModelState.AddModelError("RoomNumber", "Room Number already exists.");
                    return View(model);
                }

                room.RoomNumber = model.RoomNumber;
                room.RoomTypeId = model.RoomTypeId;
                room.Status = (RoomStatus)model.Status;

                _roomService.Update(room);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _roomService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
