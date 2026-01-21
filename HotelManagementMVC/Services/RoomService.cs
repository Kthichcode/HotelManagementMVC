using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepo;

        public RoomService(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int? roomTypeId)
        {
            List<Room> result = new List<Room>();

            if (checkIn >= checkOut)
            {
                return result;
            }

            List<Room> rooms = _roomRepo.GetAllWithBookings(); // mình sẽ cho bạn method này ở repo

            for (int i = 0; i < rooms.Count; i++)
            {
                Room r = rooms[i];

                if (r.Status != RoomStatus.Available)
                {
                    continue;
                }

                if (roomTypeId.HasValue && r.RoomTypeId != roomTypeId.Value)
                {
                    continue;
                }

                bool isOverlap = false;

                for (int j = 0; j < r.BookingRooms.Count; j++)
                {
                    Booking booking = r.BookingRooms.ElementAt(j).Booking!;

                    if (booking.Status != BookingStatus.Confirmed && booking.Status != BookingStatus.CheckedIn)
                    {
                        continue;
                    }

                    // overlap condition
                    if (checkIn < booking.CheckOutDate && checkOut > booking.CheckInDate)
                    {
                        isOverlap = true;
                        break;
                    }
                }

                if (!isOverlap)
                {
                    result.Add(r);
                }
            }

            return result;
        }

      
        public List<Room> GetAll()
        {
            return _roomRepo.GetAll();
        }

        public Room? GetById(int id)
        {
            return _roomRepo.GetById(id);
        }

        public void Create(Room room)
        {
            if (room.RoomNumber == null)
            {
                throw new Exception("Room number is required");
            }

            room.RoomNumber = room.RoomNumber.Trim();

            if (_roomRepo.ExistsRoomNumber(room.RoomNumber))
            {
                throw new Exception("Room number already exists");
            }

            _roomRepo.Add(room);
            _roomRepo.Save();
        }

        public void Update(Room room)
        {
            if (room.RoomNumber == null)
            {
                throw new Exception("Room number is required");
            }

            room.RoomNumber = room.RoomNumber.Trim();

            if (_roomRepo.ExistsRoomNumberExceptId(room.RoomNumber, room.Id))
            {
                throw new Exception("Room number already exists");
            }

            _roomRepo.Update(room);
            _roomRepo.Save();
        }

        public void Delete(int id)
        {
            Room? existing = _roomRepo.GetById(id);
            if (existing == null)
            {
                return;
            }

            _roomRepo.Delete(existing);
            _roomRepo.Save();
        }
    }
}
