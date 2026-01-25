using System.ComponentModel.DataAnnotations;

namespace HotelManagementMVC.Models
{
    public class ProfileViewModel
    {
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";

        [Required]
        public string FullName { get; set; } = "";

        public string? PhoneNumber { get; set; }
    }
}
