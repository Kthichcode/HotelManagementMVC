using System;
using System.ComponentModel.DataAnnotations;
using HotelManagementMVC.ValidationAttributes;

namespace HotelManagementMVC.Models
{
    public class BookingConfirmViewModel
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = "";
        public string RoomTypeName { get; set; } = "";
        
        [Required]
        [DataType(DataType.Date)]
        [DateInFuture(ErrorMessage = "Check-in date cannot be in the past.")]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateGreaterThan("CheckIn", ErrorMessage = "Check-out must be after Check-in.")]
        public DateTime CheckOut { get; set; }
        
        public decimal PricePerNight { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
