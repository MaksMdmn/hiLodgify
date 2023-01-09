using System;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public class Booking : IEntity
    {
        public int Id { get; }
        
        public int Unit { get; }

        public DateTime Start { get; }
        
        public int Nights { get; }
        
        public DateTime End => Start.AddDays(Nights);

        public Booking(int id, int unit, DateTime start, int nights)
        {
            Id = id;
            Unit = unit;
            Nights = nights;
            
            Start = DateOnly(start);
        }

        public bool IsOngoing(DateTime date)
        {
            return Start <= DateOnly(date) && End > DateOnly(date);
        }
        
        public bool IsOngoing(DateTime start, int nights)
        {
            var from = DateOnly(start);
            var until = from.AddDays(nights);

            return Start <= from && End > from
                   || (Start < until && End >= until)
                   || (Start > from && End < until);
        }
        
        static DateTime DateOnly(DateTime dateTime)
        {
            return dateTime.Date;
        }
    }
}