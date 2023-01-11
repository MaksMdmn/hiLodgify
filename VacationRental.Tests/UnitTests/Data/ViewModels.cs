using System;
using System.Collections.Generic;
using Bogus;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Tests.UnitTests.Data
{
    public static class ViewModels
    {
        public static T Generate<T>() where T : class
        {
            return (T)Generator[typeof(T)]();
        }

        static readonly Dictionary<Type, Func<object>> Generator = new Dictionary<Type, Func<object>>
        {
            {typeof(RentalViewModel), () => RentalViewModel.Generate() },
            {typeof(BookingViewModel), () => BookingViewModel.Generate() },
            {typeof(ResourceIdViewModel), () => ResourceIdViewModel.Generate() },
        };

        static readonly Faker<RentalViewModel> RentalViewModel = new Faker<RentalViewModel>()
            .RuleFor(t => t.Id, s => s.Random.Int(1))
            .RuleFor(t => t.Units, s => s.Random.Int(1))
            .RuleFor(t => t.PreparationTimeInDays, s => s.Random.Int(0));
        
        static readonly Faker<BookingViewModel> BookingViewModel = new Faker<BookingViewModel>()
            .RuleFor(t => t.Id, s => s.Random.Int(1))
            .RuleFor(t => t.RentalId, s => s.Random.Int(1))
            .RuleFor(t => t.Nights, s => s.Random.Int(1))
            .RuleFor(t => t.Start, s => s.Date.Soon());
        
        static readonly Faker<ResourceIdViewModel> ResourceIdViewModel = new Faker<ResourceIdViewModel>()
            .RuleFor(t => t.Id, s => s.Random.Int(1));
    }
}