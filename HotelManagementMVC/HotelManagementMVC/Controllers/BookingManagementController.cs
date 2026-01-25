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
        public IActionResult Index(DateTime? date, BookingStatus? status, string phoneNumber)
        {
            var bookings = _bookingService.GetFilteredBookings(date, status, phoneNumber);
            
            ViewBag.SelectedDate = date?.ToString("yyyy-MM-dd");
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedPhoneNumber = phoneNumber;
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

        [Authorize(Roles = "Staff")] // Chỉ cho phép nhân viên thực hiện chức năng này
        public IActionResult SearchByPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Staff")] // Chỉ cho phép nhân viên thực hiện chức năng này
        public IActionResult SearchByPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                TempData["Error"] = "Please enter a phone number.";
                return View();
            }

            var bookings = _bookingService.SearchBookingsByPhoneNumber(phoneNumber);
            return View("SearchResults", bookings); // Hiển thị kết quả tìm kiếm
        }
    }
}
