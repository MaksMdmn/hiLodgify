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
            return Start <= DateOnly(date) && End > DateOnly(date);
        }
        
        public bool IsOngoing(DateTime start, int nights)
        {
            var startDate = DateOnly(start);
            var endDate = startDate.AddDays(nights);

            return Start <= startDate && End > startDate
                   || (Start < endDate && End >= endDate)
                   || (Start > startDate && End < endDate);
        }
        
        static DateTime DateOnly(DateTime dateTime)
        {
            return dateTime.Date;
        }
    }
}