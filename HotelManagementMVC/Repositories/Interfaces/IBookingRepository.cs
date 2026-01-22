using BusinessObjects.Entities;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBookingRepository
    {
        void Add(Booking booking);
        Booking? GetById(int id);
        List<Booking> GetByCustomer(string userId);
        void UpdateStatus(int bookingId, BookingStatus status);
        void Save();
    }
}
