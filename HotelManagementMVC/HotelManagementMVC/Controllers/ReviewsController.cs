using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Linq;

namespace HotelManagementMVC.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(IReviewService reviewService, IBookingService bookingService, UserManager<ApplicationUser> userManager)
        {
            _reviewService = reviewService;
            _bookingService = bookingService;
            _userManager = userManager;
        }

        // GET: Reviews/Create?bookingId=xxx
        [HttpGet]
        public IActionResult Create(int bookingId)
        {
            var userId = _userManager.GetUserId(User);
            
            // Kiểm tra quyền review
            if (!_reviewService.CanReview(bookingId, userId))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền đánh giá đặt phòng này hoặc đặt phòng chưa hoàn thành.";
                return RedirectToAction("Index", "Bookings");
            }

            var booking = _bookingService.GetById(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            ViewBag.Booking = booking;
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int bookingId, int rating, string? comment)
        {
            var userId = _userManager.GetUserId(User);
            
            try
            {
                // Kiểm tra quyền review
                if (!_reviewService.CanReview(bookingId, userId))
                {
                    TempData["ErrorMessage"] = "Bạn không có quyền đánh giá đặt phòng này hoặc đặt phòng chưa hoàn thành.";
                    return RedirectToAction("Index", "Bookings");
                }

                _reviewService.CreateReview(bookingId, rating, comment);
                TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá! Đánh giá của bạn đã được ghi nhận.";
                return RedirectToAction("Index", "Bookings");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Create", new { bookingId });
            }
        }

        // GET: Reviews/Index?roomId=xxx (Xem tất cả reviews của một phòng)
        [AllowAnonymous]
        public IActionResult Index(int? roomId)
        {
            if (roomId.HasValue)
            {
                var reviews = _reviewService.GetByRoomId(roomId.Value);
                ViewBag.RoomId = roomId.Value;
                return View(reviews);
            }
            else
            {
                var reviews = _reviewService.GetAll();
                return View(reviews);
            }
        }
    }
}

