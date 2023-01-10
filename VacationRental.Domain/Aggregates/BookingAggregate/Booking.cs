using System;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Aggregates.BookingAggregate
{
    public class Booking : IEntity
    {
        public int Id { get; set; }

        public int RentalId { get; }
        
        public int Unit { get; }

        public DateTime Start { get; }
        
        public int Nights { get; }
        
        public DateTime End => Start.AddDays(Nights);

        public Booking(int rentalId, int unit, DateTime start, int nights)
        {
            Unit = unit;
            Nights = nights;
            RentalId = rentalId;
            Start = start;
        }

        public bool IsOngoing(DateTime date)
        {
            return Start <= date && End > date;
        }
        
        public bool IsOngoing(DateTime from, int nights)
        {
            var until = from.AddDays(nights);

            return Start <= from && End > from
                   || (Start < until && End >= until)
                   || (Start > from && End < until);
        }
    }
}