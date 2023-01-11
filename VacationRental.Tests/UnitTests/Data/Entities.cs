using System;
using AutoBogus;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Tests.UnitTests.Data
{
    sealed class RentalFaker : AutoFaker<Rental>
    {
        public RentalFaker()
        {
            RuleFor(t => t.Id, s => s.Random.Int(1));
            RuleFor(t => t.Units, s => s.Random.Int(1, 100));
            RuleFor(t => t.PreparationTimeInDays, s => s.Random.Int(0, 100));
            RuleFor(t => t.Preparations, s => new [] { new PreparationTimeFaker().Generate() });
        }

        public RentalFaker(int units, int preparationTimes)
        {
            RuleFor(t => t.Id, s => s.Random.Int(1));
            RuleFor(t => t.Units, s => units);
            RuleFor(t => t.PreparationTimeInDays, preparationTimes);
            RuleFor(t => t.Preparations, s => Array.Empty<PreparationTime>());
        }
    }
    
    sealed class BookingFaker : AutoFaker<Booking>
    {
        public BookingFaker()
        {
            RuleFor(t => t.Id, s => s.Random.Int(1));
            RuleFor(t => t.RentalId, s => s.Random.Int(1));
            RuleFor(t => t.Unit, s => s.Random.Int(1, 100));
            RuleFor(t => t.Nights, s => s.Random.Int(1, 100));
            RuleFor(t => t.Start, s => s.Date.Soon());
        }
        
        public BookingFaker(DateTime date)
        {
            RuleFor(t => t.Id, s => s.Random.Int(1));
            RuleFor(t => t.RentalId, s => s.Random.Int(1));
            RuleFor(t => t.Unit, s => s.Random.Int(1, 100));
            RuleFor(t => t.Nights, s => s.Random.Int(1, 100));
            RuleFor(t => t.Start, date);
        }
        
        public BookingFaker(DateTime date, int nights)
        {
            RuleFor(t => t.Id, s => s.Random.Int(1));
            RuleFor(t => t.RentalId, s => s.Random.Int(1));
            RuleFor(t => t.Unit, s => s.Random.Int(1, 100));
            RuleFor(t => t.Nights, nights);
            RuleFor(t => t.Start, date);
        }
        
        public BookingFaker(int rentalId, int unit, DateTime date)
        {
            RuleFor(t => t.Id, s => s.Random.Int(1));
            RuleFor(t => t.RentalId, s => rentalId);
            RuleFor(t => t.Unit, s => unit);
            RuleFor(t => t.Nights, s => s.Random.Int(1, 100));
            RuleFor(t => t.Start, s => date);
        }
    }

    sealed class PreparationTimeFaker : AutoFaker<PreparationTime>
    {
        public PreparationTimeFaker()
        {
            RuleFor(t => t.Unit, s => s.Random.Int(1, 100));
            RuleFor(t => t.Days, s => s.Random.Int(0, 100));
            RuleFor(t => t.Start, s => s.Date.Soon());
        }
    }
}