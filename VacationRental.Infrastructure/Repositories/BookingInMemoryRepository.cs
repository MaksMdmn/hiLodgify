using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Aggregates.BookingAggregate;

namespace VacationRental.Infrastructure.Repositories
{
    public class BookingInMemoryRepository : IBookingRepository
    {
        readonly IDictionary<int, Booking> bookings = new Dictionary<int, Booking>();

        public int Add(Booking booking)
        {
            booking.Id = NextId();
            
            bookings.Add(booking.Id, booking);

            return booking.Id;
        }

        public Booking GetOne(int id)
        {
            bookings.TryGetValue(id, out var result);
            
            return result;
        }

        public Booking[] GetManyByRenalId(int rentalId)
        {
            return bookings.Values
                .Where(booking => booking.RentalId == rentalId)
                .ToArray();
        }
        
        int NextId()
        {
            return bookings.Keys.Count + 1;
        }
    }
}