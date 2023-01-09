using System;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public class PreparationTime : IValueObject
    {
        public DateTime Start { get; }
        
        public int Unit { get; }

        public int Days { get; }
        
        public DateTime End => Start.AddDays(Days);

        public PreparationTime(DateTime start, int unit, int days)
        {
            Start = start;
            Unit = unit;
            Days = days;
        }
        
        public bool IsOngoing(DateTime date)
        {
            return Start <= date && End > date;
        }
        
        public bool IsOngoing(DateTime from, int nights)
        {
            var until = from.AddDays(nights);

            return Start <= from && End > until
                   || (Start < until && End >= until)
                   || (Start > from && End < until);
        }
    }
}