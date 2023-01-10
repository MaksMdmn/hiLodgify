using System;
using System.Linq;
using AutoMapper;
using VacationRental.Api.Application.Interfaces;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Application.Services
{
    public class BookingService : IBookingService
    {
        readonly IRentalRepository rentals;
        readonly IBookingRepository bookings;
        readonly IMapper mapper;

        public BookingService(IRentalRepository rentals, IBookingRepository bookings, IMapper mapper)
        {
            this.rentals = rentals;
            this.bookings = bookings;
            this.mapper = mapper;
        }

        public BookingViewModel GetOne(int bookingId)
        {
            var booking = bookings.GetOne(bookingId);
            
            return ToViewModel<BookingViewModel>(booking);
        }

        public ResourceIdViewModel MakeBooking(BookingBindingModel model)
        {
            var rental = rentals.GetOne(model.RentalId);

            var unit = FindAvailableUnit(rental, model.Start, model.Nights);

            var booking = BookUnit(rental.Id, unit, model.Start, model.Nights);

            SchedulePreparation(rental, booking.End, unit);

            return ToViewModel(booking.Id);
        }

        Booking BookUnit(int rentalId, int unit, DateTime start, int night)
        {
            var booking = new Booking(rentalId, unit, start, night);
            
            booking.Id = bookings.Add(booking);

            return booking;
        }

        int FindAvailableUnit(Rental rental, DateTime start, int nights)
        {
            var prepared = rental.FindPreparedUnits(start, nights);
            var booked = FindBookedUnits(rental.Id, start, nights);
            
            var availableUnits = prepared.Except(booked).ToArray();

            if (availableUnits.Length == 0)
                throw new ApplicationException("Not available");

            return availableUnits.First();
        }
        
        void SchedulePreparation(Rental rental, DateTime from, int unit)
        {
            rental.SchedulePreparation(from, unit);

            rentals.Update(rental);
        }
        
        int[] FindBookedUnits(int rentalId, DateTime start, int nights)
        {
            return bookings.GetManyByRenalId(rentalId)
                .Where(booking => booking.IsOngoing(start, nights))
                .Select(booking => booking.Unit)
                .ToArray();
        }

        TViewModel ToViewModel<TViewModel>(object source)
        {
            return mapper.Map<TViewModel>(source);
        }
        
        static ResourceIdViewModel ToViewModel(int id)
        {
            return new ResourceIdViewModel { Id = id };
        }
    }
}