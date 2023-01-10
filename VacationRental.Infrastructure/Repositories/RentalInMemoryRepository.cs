using System;
using System.Collections.Generic;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Infrastructure.Repositories
{
    public class RentalInMemoryRepository : IRentalRepository
    {
        readonly IDictionary<int, Rental> rentals = new Dictionary<int, Rental>();
        
        public Rental GetOne(int id)
        {
            if (rentals.TryGetValue(id, out var result))
                return result;

            throw new ApplicationException("Rental not found");
        }
        
        public int Add(Rental rental)
        {
           rental.Id = NextId();
            
           rentals.Add(rental.Id, rental);

           return rental.Id;
        }

        public void Update(Rental rental)
        {
            rentals[rental.Id] = rental;
        }

        int NextId()
        {
            return rentals.Keys.Count + 1;
        }
    }
}