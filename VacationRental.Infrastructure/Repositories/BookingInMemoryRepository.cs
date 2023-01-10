using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Aggregates.BookingAggregate;

namespace VacationRental.Infrastructure.Repositories
{
    public class BookingInMemoryRepository : IBookingRepository
    {
        readonly IDictionary<int, Booking> bookings = new Dictionary<int, Booking>();

        public Booking GetOne(int id)
        {
            if (bookings.TryGetValue(id, out var result))
                return result;
            
            throw new ApplicationException("Rental not found");
        }

        public Booking[] GetManyByRenalId(int rentalId)
        {
            return bookings.Values
                .Where(booking => booking.RentalId == rentalId)
                .ToArray();
        }
        
        public int Add(Booking booking)
        {
            booking.Id = NextId();
            
            bookings.Add(booking.Id, booking);

            return booking.Id;
        }

        int NextId()
        {
            return bookings.Keys.Count + 1;
        }
    }
}