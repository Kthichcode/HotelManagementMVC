using System;

namespace HotelManagementMVC.Models
{
    public class BookingConfirmViewModel
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = "";
        public string RoomTypeName { get; set; } = "";
        
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        
        public decimal PricePerNight { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
