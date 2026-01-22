using BusinessObjects;
using BusinessObjects.Entities;
using HotelManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManagementMVC.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVnPayService _vnPayService;
        private readonly IWalletService _walletService;

        public BookingsController(IBookingService bookingService, IRoomService roomService, UserManager<ApplicationUser> userManager, IVnPayService vnPayService, IWalletService walletService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _userManager = userManager;
            _vnPayService = vnPayService;
            _walletService = walletService;
        }

        // GET: /Bookings/Index (My Bookings)
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var bookings = _bookingService.GetMyBookings(userId);
            return View(bookings);
        }

        // GET: /Bookings/Confirm
        [HttpGet]
        public IActionResult Confirm(int roomId, DateTime checkIn, DateTime checkOut)
        {
            if (checkIn >= checkOut)
            {
                TempData["Error"] = "Invalid dates.";
                return RedirectToAction("Search", "Rooms");
            }

            var room = _roomService.GetById(roomId);
            if (room == null) return NotFound();

            int nights = (int)(checkOut - checkIn).TotalDays;
            if (nights < 1) nights = 1;
            
            decimal price = room.RoomType != null ? room.RoomType.PricePerNight : 0;
            
            var model = new BookingConfirmViewModel
            {
                RoomId = roomId,
                RoomNumber = room.RoomNumber,
                RoomTypeName = room.RoomType != null ? room.RoomType.Name : "N/A",
                CheckIn = checkIn,
                CheckOut = checkOut,
                PricePerNight = price,
                TotalNights = nights,
                TotalAmount = nights * price
            };

            return View(model);
        }

        // POST: /Bookings/Create
        [HttpPost]
        public IActionResult Create(BookingConfirmViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            try
            {
                // 1. Create Booking (Pending)
                int bookingId = _bookingService.CreateBooking(userId, model.RoomId, model.CheckIn, model.CheckOut);
                
                // 2. Get Booking Details
                var booking = _bookingService.GetById(bookingId);
                
                if (booking == null) 
                {
                    return RedirectToAction("Index");    
                }

                // --- Hybrid Payment Logic ---
                decimal totalAmount = booking.TotalAmount;
                decimal deducted = _walletService.DeductBalance(userId, totalAmount);
                decimal remaining = totalAmount - deducted;

                // Update booking status if fully paid by wallet
                if (remaining <= 0)
                {
                    _bookingService.ConfirmPayment(bookingId);
                    TempData["SuccessMessage"] = $"Booking successful! Paid full {deducted:N0} via Wallet.";
                    return RedirectToAction("Index");
                }
                
                // If remaining > 0, pay the rest via VNPay
                // 3. Create Payment Request URL
                // Note: We use the 'remaining' amount here, not booking.TotalAmount
                var paymentUrl = _vnPayService.CreatePaymentUrl(booking, HttpContext, remaining);

                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                
                // Re-hydrate model for view
                 var room = _roomService.GetById(model.RoomId);
                 if (room != null)
                 {
                    model.RoomNumber = room.RoomNumber;
                    model.RoomTypeName = room.RoomType != null ? room.RoomType.Name : "N/A";
                 }
                 return View("Confirm", model);
            }
        }
        
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);
            try 
            {
                var booking = _bookingService.GetById(id);
                if (booking == null) throw new Exception("Booking not found.");
                
                // Refund logic if Confirmed (Paid)
                // Assuming if Confirmed means they paid full (either by Wallet or VNPay)
                if (booking.Status == BusinessObjects.Enums.BookingStatus.Confirmed)
                {
                    _walletService.AddBalance(userId, booking.TotalAmount);
                    TempData["SuccessMessage"] = $"Booking cancelled. Refunded {booking.TotalAmount:N0} to your wallet.";
                }
                else
                {
                     TempData["SuccessMessage"] = "Booking cancelled.";
                }

                _bookingService.CancelBooking(id, userId);
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                 // Update booking status here
                 int bookingId = int.Parse(response.OrderId);
                 _bookingService.ConfirmPayment(bookingId);
                 
                 TempData["SuccessMessage"] = $"Payment successful for Booking #{bookingId}";
            }
            else
            {
                TempData["Error"] = $"Payment failed. Code: {response.VnPayResponseCode}";
            }

            // Map DTO to ViewModel (Strict Layered Architecture)
            var viewModel = new PaymentResultViewModel
            {
                Success = response.Success,
                PaymentMethod = response.PaymentMethod,
                OrderDescription = response.OrderDescription,
                OrderId = response.OrderId,
                TransactionId = response.TransactionId,
                Token = response.Token,
                VnPayResponseCode = response.VnPayResponseCode
            };

            return View(viewModel);
        }
    }
}
