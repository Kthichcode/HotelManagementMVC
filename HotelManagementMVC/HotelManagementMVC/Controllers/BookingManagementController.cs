using BusinessObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace HotelManagementMVC.Controllers
{
    [Authorize(Roles = "Admin,Manager,Staff")]
    public class BookingManagementController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingManagementController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: List all bookings with filters
        public IActionResult Index(DateTime? date, BookingStatus? status)
        {
            var bookings = _bookingService.GetFilteredBookings(date, status);
            
            ViewBag.SelectedDate = date?.ToString("yyyy-MM-dd");
            ViewBag.SelectedStatus = status;

            return View(bookings);
        }

        // GET: Booking Details
        public IActionResult Details(int id)
        {
            var booking = _bookingService.GetById(id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Update Status (Confirm, CheckIn, CheckOut, Cancel)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, BookingStatus newStatus)
        {
            try
            {
                _bookingService.UpdateStatus(id, newStatus);
                TempData["SuccessMessage"] = $"Booking #{id} updated to {newStatus} successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
