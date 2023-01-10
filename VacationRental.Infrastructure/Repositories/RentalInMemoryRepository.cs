using System.Collections.Generic;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Infrastructure.Repositories
{
    public class RentalInMemoryRepository : IRentalRepository
    {
        readonly IDictionary<int, Rental> rentals = new Dictionary<int, Rental>();

        public int Add(Rental rental)
        {
           rental.Id = NextId();
            
           rentals.Add(rental.Id, rental);

           return rental.Id;
        }

        public Rental GetOne(int id)
        {
            rentals.TryGetValue(id, out var result);

            return result;
        }

        int NextId()
        {
            return rentals.Keys.Count + 1;
        }
    }
}