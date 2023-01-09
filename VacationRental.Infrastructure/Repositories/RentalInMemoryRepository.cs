using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Infrastructure.Repositories
{
    public class RentalInMemoryRepository : IRentalRepository
    {
        readonly IDictionary<int, Rental> rentals = new ConcurrentDictionary<int, Rental>();

        public int Add(Rental rental)
        {
            rental.Id = NextId();
            
           rentals.Add(rental.Id, rental);

           return rental.Id;
        }

        public Rental GetOne(int id)
        {
            if (rentals.TryGetValue(id, out var result))
                return result;

            throw new ApplicationException("Rental not found");
        }

        public Rental GetByBookingId(int bookingId)
        {
            var result = rentals.Values.FirstOrDefault(rental => 
                rental.Bookings.Any(booking => booking.Id == bookingId));
            
            if (result == null)
                throw new ApplicationException("Booking not found");

            return result;
        }

        int NextId()
        {
            return rentals.Keys.Count + 1;
        }
    }
}