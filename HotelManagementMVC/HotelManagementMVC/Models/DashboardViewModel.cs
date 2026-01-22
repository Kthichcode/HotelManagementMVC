using System.Collections.Generic;

namespace HotelManagementMVC.Models
{
    public class DashboardViewModel
    {
        // Room Stats
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int MaintenanceRooms { get; set; }

        // Booking Stats
        public int BookingsToday { get; set; }
        public int BookingsThisMonth { get; set; }

        // Revenue Stats
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisMonth { get; set; }

        // Top Room Types
        public List<TopRoomTypeViewModel> TopRoomTypes { get; set; } = new List<TopRoomTypeViewModel>();
    }

    public class TopRoomTypeViewModel
    {
        public string RoomTypeName { get; set; } = "";
        public int BookingCount { get; set; }
    }
}
