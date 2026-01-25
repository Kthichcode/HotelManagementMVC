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

        // NEW: số người ở tối đa
        [Range(1, 50)]
        public int MaxOccupancy { get; set; }

        // NEW: mô tả
        public string? Description { get; set; }

        // NEW: upload nhiều ảnh
        public List<IFormFile>? Images { get; set; }

        // NEW: hiển thị ảnh cũ khi edit
        public List<string> ExistingImageUrls { get; set; } = new List<string>();

        public List<SelectListItem> RoomTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>();
    }
}
