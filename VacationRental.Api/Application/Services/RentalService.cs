using System;
using System.Linq;
using AutoMapper;
using VacationRental.Api.Application.Extensions;
using VacationRental.Api.Application.Interfaces;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Application.Services
{
    public class RentalService : IRentalService
    {
        readonly IRentalRepository rentals;
        readonly IBookingRepository bookings;
        readonly IMapper mapper;

        public RentalService(IRentalRepository rentals, IBookingRepository bookings, IMapper mapper)
        {
            this.rentals = rentals;
            this.bookings = bookings;
            this.mapper = mapper;
        }

        public RentalViewModel GetOne(int rentalId)
        {
            var rental = rentals.GetOne(rentalId);
            
            return ToViewModel<RentalViewModel>(rental);
        }

        public ResourceIdViewModel Create(CreateRentalBindingModel model)
        {
            var rental = new Rental(model.Units, model.PreparationTimeInDays);

            var id = rentals.Add(rental);

            return ToViewModel(id);
        }

        public RentalViewModel Update(int rentalId, UpdateRentalBindingModel model)
        {
            var date = Today();
            var upcoming = UpcomingBookings(rentalId, date);
            var duration = BookedPeriodInDays(upcoming, date);
            
            var simulation = SimulateBookings(upcoming, model.PreparationTimeInDays);

            for (var day = 0; day <= duration; day++)
            {
                var bookingsPerDay = simulation.Count(
                    booking => booking.IsOngoing(date.AddDays(day)));

                if (bookingsPerDay > model.Units)
                    throw new ApplicationException("Unable to change rental units value as it would affect existing bookings");
            }

            var rental = UpdateRental(rentalId, date, model);

            return ToViewModel<RentalViewModel>(rental);
        }

        static Booking[] SimulateBookings(Booking[] data, int adjustNights)
        {
            var copy = data.DeepCopy();
                
            foreach (var booking in data)
            {
                booking.SetNights(booking.Nights + adjustNights);
            }

            return copy;
        }

        static int BookedPeriodInDays(Booking[] upcoming, DateTime from)
        {
            //Simplest, but not the best approach
            return 1 + upcoming.Max(booking => booking.End)
                .Subtract(from).Days;
        }
        
        Rental UpdateRental(int rentalId, DateTime date, UpdateRentalBindingModel model)
        {
            var rental = rentals.GetOne(rentalId);
            
            rental.SetUnits(model.Units);
            rental.SetPreparationTimeInDays(model.PreparationTimeInDays, date);

            rentals.Update(rental);

            return rental;
        }
        
        Booking[] UpcomingBookings(int rentalId, DateTime from)
        {
            // I skipped part of bookings which are already opened 
            return bookings
                .GetManyByRenalId(rentalId)
                .Where(booking => booking.Start >= from || booking.IsOngoing(from))
                .OrderBy(booking => booking.Start)
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

        static DateTime Today()
        {
            return DateTime.Now.Date;
        }
    }
}