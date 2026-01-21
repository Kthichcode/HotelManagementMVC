using System.ComponentModel.DataAnnotations;

namespace HotelManagementMVC.Models
{
    public class RoomTypeFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        [Range(0, 999999999)]
        public decimal PricePerNight { get; set; }
    }
}
