using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace HotelManagementMVC.Models
{
    public class SearchRoomsViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        public int? RoomTypeId { get; set; }

        public string? RoomNumber { get; set; }


        public List<SelectListItem> RoomTypes { get; set; } = new();


        public List<RoomResultViewModel> Results { get; set; } = new();
    }

    public class RoomResultViewModel
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = "";
        public string RoomTypeName { get; set; } = "";
        public decimal PricePerNight { get; set; }
    }
}
