using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        readonly IRentalRepository rentals;

        public CalendarService(IRentalRepository rentals)
        {
            this.rentals = rentals;
        }

        public CalendarViewModel Create(int rentalId, DateTime start, int nights)
        {
            var rental = rentals.GetOne(rentalId);

            var dates = new List<CalendarDateViewModel>();
            
            for (var i = 0; i < nights; i++)
            {
                dates.Add(CalendarDate(rental, start.Date.AddDays(i)));
            }

            return new CalendarViewModel 
            {
                RentalId = rentalId,
                Dates = dates
            };
        }

        static CalendarDateViewModel CalendarDate(Rental rental, DateTime date)
        {
            var bookings = rental.Bookings
                .Where(booking => booking.IsOngoing(date))
                .Select(booking => new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit})
                .ToList();
            
            var preparations = rental.Preparations
                .Where(preparation => preparation.IsOngoing(date))
                .Select(preparation => new CalendarPreparationTimeViewModel { Unit = preparation.Unit })
                .ToList();

            return new CalendarDateViewModel
            {
                Date = date,
                Bookings = bookings,
                PreparationTimes = preparations
            };
        }
    }
}