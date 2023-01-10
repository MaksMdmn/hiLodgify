using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        readonly IRentalRepository rentals;
        readonly IBookingRepository bookings;
        readonly IMapper mapper;

        public CalendarService(IRentalRepository rentals, IBookingRepository bookings, IMapper mapper)
        {
            this.rentals = rentals;
            this.bookings = bookings;
            this.mapper = mapper;
        }

        public CalendarViewModel ComposeCalendar(int rentalId, DateTime start, int nights)
        {
            var rental = rentals.GetOne(rentalId);

            var dates = new List<CalendarDateViewModel>();
            
            for (var i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                
                dates.Add(CalendarDate(rental, date));
            }

            return new CalendarViewModel 
            {
                RentalId = rentalId,
                Dates = dates
            };
        }

        CalendarDateViewModel CalendarDate(Rental rental, DateTime date)
        {
            return new CalendarDateViewModel
            {
                Date = date,
                
                Bookings = bookings.GetManyByRenalId(rental.Id)
                    .Where(booking => booking.IsOngoing(date))
                    .Select(ToViewModel<CalendarBookingViewModel>)
                    .ToList(),
                
                PreparationTimes = rental.Preparations
                    .Where(preparation => preparation.IsOngoing(date))
                    .Select(ToViewModel<CalendarPreparationTimeViewModel>)
                    .ToList()
            };
        }
        
        TViewModel ToViewModel<TViewModel>(object source)
        {
            return mapper.Map<TViewModel>(source);
        }
    }
}