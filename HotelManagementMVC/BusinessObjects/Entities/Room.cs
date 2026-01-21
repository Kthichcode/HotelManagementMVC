using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class Room
    {
        public int Id { get; set; }

        [Required] public string RoomNumber { get; set; } = "";

        public RoomStatus Status { get; set; } = RoomStatus.Available;

        public int RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; }

        public ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
    }
}
