using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int? roomTypeId = null);

        List<Room> GetAll();
        Room? GetById(int id);
        void Create(Room room);
        void Update(Room room);
        void Delete(int id);
    }
}
