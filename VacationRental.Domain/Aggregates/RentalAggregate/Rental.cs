using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public class Rental : IAggregateRoot, IEntity
    {
        readonly List<Booking> bookings = new List<Booking>();
        readonly List<PreparationTime> preparations = new List<PreparationTime>();
        
        public int Id { get; set; }
        
        public int Units { get; }
        
        public int PreparationTimeInDays { get; }

        public IReadOnlyCollection<Booking> Bookings => bookings;
        public IReadOnlyCollection<PreparationTime> Preparations => preparations;

        public Rental(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public int TryBookUnit(DateTime start, int nights)
        {
            var unit = FindAvailableUnit(start, nights);

            var booking = BookUnit(unit, start, nights);

            PlanPreparationAfter(booking);

            return booking.Id;
        }

        int FindAvailableUnit(DateTime start, int nights)
        {
            var unavailable = BookedUnits(start, nights)
                .Concat(UnpreparedUnits(start, nights))
                .Distinct()
                .ToList();

            for (var i = 1; i <= Units; i++)
            {
                if (!unavailable.Contains(i))
                    return i;
            }
            
            throw new ApplicationException("Not available");
        }

        Booking BookUnit(int unit, DateTime start, int nights)
        {
            var result = new Booking(BookingId(), Id, unit, start, nights);
            
            bookings.Add(result);

            return result;
        }
        
        void PlanPreparationAfter(Booking booking)
        {
            preparations.Add(new PreparationTime(booking.End, booking.Unit, PreparationTimeInDays));
        }

        int BookingId()
        {
            return bookings.Count + 1;
        }

        int[] BookedUnits(DateTime start, int nights)
        {
            return bookings
                .Where(booking => booking.IsOngoing(start, nights))
                .Select(booking => booking.Unit)
                .ToArray();
        }
        
        int[] UnpreparedUnits(DateTime start, int nights)
        {
            return preparations
                .Where(preparation => preparation.IsOngoing(start, nights))
                .Select(preparation => preparation.Unit)
                .ToArray();
        }
    }
}