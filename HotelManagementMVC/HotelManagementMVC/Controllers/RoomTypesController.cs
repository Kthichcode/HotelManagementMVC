using BusinessObjects.Entities;
using HotelManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class RoomTypesController : Controller
    {
        private readonly IRoomTypeService _service;

        public RoomTypesController(IRoomTypeService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var types = _service.GetAll();
            return View(types);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RoomTypeFormViewModel());
        }

        [HttpPost]
        public IActionResult Create(RoomTypeFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = new RoomType
            {
                Name = model.Name,
                Description = model.Description,
                PricePerNight = model.PricePerNight
            };

            _service.Create(entity);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var type = _service.GetById(id);
            if (type == null) return NotFound();

            var vm = new RoomTypeFormViewModel
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description,
                PricePerNight = type.PricePerNight
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(RoomTypeFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var type = _service.GetById(model.Id);
            if (type == null) return NotFound();

            type.Name = model.Name;
            type.Description = model.Description;
            type.PricePerNight = model.PricePerNight;

            _service.Update(type);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
