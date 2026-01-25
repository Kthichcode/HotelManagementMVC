using BusinessObjects.Entities;
using BusinessObjects.Enums;
using HotelManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace HotelManagementMVC.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class RoomsManagementController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IWebHostEnvironment _env;

        public RoomsManagementController(IRoomService roomService, IRoomTypeService roomTypeService, IWebHostEnvironment env)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            _env = env;
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

        private List<string> SaveRoomImages(List<IFormFile> images)
        {
            var savedUrls = new List<string>();

            var folder = Path.Combine(_env.WebRootPath, "uploads", "rooms");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            for (int i = 0; i < images.Count; i++)
            {
                var file = images[i];
                if (file == null || file.Length == 0) continue;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var allowed = new string[] { ".jpg", ".jpeg", ".png", ".webp" };

                var ok = false;
                for (int j = 0; j < allowed.Length; j++)
                {
                    if (allowed[j] == ext) { ok = true; break; }
                }
                if (!ok) continue;

                var fileName = Guid.NewGuid().ToString() + ext;
                var fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                savedUrls.Add("/uploads/rooms/" + fileName);
            }

            return savedUrls;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RoomFormViewModel();
            model.Status = 1;
            model.MaxOccupancy = 1;
            LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomFormViewModel model)
        {
            LoadDropdowns(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // ✅ MVC-clean: Controller không tự query list để check trùng
                if (_roomService.IsRoomNumberExists(model.RoomNumber))
                {
                    ModelState.AddModelError("RoomNumber", "Room Number already exists.");
                    return View(model);
                }

                var room = new Room();
                room.RoomNumber = model.RoomNumber;
                room.RoomTypeId = model.RoomTypeId;
                room.Status = (RoomStatus)model.Status;

                room.MaxOccupancy = model.MaxOccupancy;
                room.Description = model.Description ?? "";
                room.ImageUrl = "";

                _roomService.Create(room); // Save to get Id

                // Upload images
                if (model.Images != null && model.Images.Count > 0)
                {
                    var urls = SaveRoomImages(model.Images);

                    if (urls.Count > 0)
                    {
                        room.ImageUrl = urls[0];     // thumbnail
                        _roomService.Update(room);   // update thumbnail

                        _roomService.AddRoomImages(room.Id, urls);
                    }
                }

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

            model.MaxOccupancy = room.MaxOccupancy;
            model.Description = room.Description;
            model.ExistingImageUrls = _roomService.GetRoomImageUrls(id);

            LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoomFormViewModel model)
        {
            LoadDropdowns(model);

            if (!ModelState.IsValid)
            {
                model.ExistingImageUrls = _roomService.GetRoomImageUrls(model.Id);
                return View(model);
            }

            try
            {
                var room = _roomService.GetById(model.Id);
                if (room == null) return NotFound();

                // ✅ MVC-clean: check trùng qua service
                if (_roomService.IsRoomNumberExistsExceptId(model.RoomNumber, model.Id))
                {
                    ModelState.AddModelError("RoomNumber", "Room Number already exists.");
                    model.ExistingImageUrls = _roomService.GetRoomImageUrls(model.Id);
                    return View(model);
                }

                room.RoomNumber = model.RoomNumber;
                room.RoomTypeId = model.RoomTypeId;
                room.Status = (RoomStatus)model.Status;

                room.MaxOccupancy = model.MaxOccupancy;
                room.Description = model.Description ?? "";

                // Replace images if user uploaded new ones
                if (model.Images != null && model.Images.Count > 0)
                {
                    var urls = SaveRoomImages(model.Images);
                    if (urls.Count > 0)
                    {
                        room.ImageUrl = urls[0]; // thumbnail
                        _roomService.ReplaceRoomImages(room.Id, urls);
                    }
                }

                _roomService.Update(room);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.ExistingImageUrls = _roomService.GetRoomImageUrls(model.Id);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _roomService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
