using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementMVC.Models
{
    public class RoomFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string RoomNumber { get; set; } = "";

        [Required]
        public int RoomTypeId { get; set; }

        [Required]
        public int Status { get; set; } // 1=Available, 2=Maintenance

        public List<SelectListItem> RoomTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>();
    }
}
