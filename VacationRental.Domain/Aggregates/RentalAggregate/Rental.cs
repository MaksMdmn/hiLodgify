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
            var occupied = CountOccupiedUnits(start, nights);
            
            if (occupied >= Units)
                throw new ApplicationException("Not available");

            return AvailableUnitNumber(occupied);
        }

        Booking BookUnit(int unit, DateTime start, int nights)
        {
            var result = new Booking(BookingId(), unit, start, nights);
            
            bookings.Add(result);

            return result;
        }
        
        void PlanPreparationAfter(Booking booking)
        {
            preparations.Add(new PreparationTime(booking.End, booking.Unit, PreparationTimeInDays));
        }

        int CountOccupiedUnits(DateTime start, int nights)
        {
            return bookings.Count(booking => booking.IsOngoing(start, nights)) 
                   + preparations.Count(preparation => preparation.IsOngoing(start, nights));
        }

        static int AvailableUnitNumber(int occupied)
        {
            return occupied + 1;
        }

        int BookingId()
        {
            return bookings.Count + 1;
        }
    }
}