using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookingService
    {
        int CreateBooking(string userId, int roomId, DateTime checkIn, DateTime checkOut);
        List<Booking> GetMyBookings(string userId);
        Booking? GetById(int id);
        void CancelBooking(int bookingId, string userId);
        void ConfirmPayment(int bookingId);
    }
}
