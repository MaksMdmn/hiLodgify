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
            
            CheckBookingOverlap(date, rentalId, model.Units, model.PreparationTimeInDays);

            var rental = UpdateRental(rentalId, date, model);

            return ToViewModel<RentalViewModel>(rental);
        }

        void CheckBookingOverlap(DateTime from, int rentalId, int updatedUnits, int updatedPreparationTimeInDays)
        {
            var upcoming = UpcomingBookings(from, rentalId);

            var simulation = SimulateBookings(upcoming, updatedPreparationTimeInDays);

            var dates = simulation.Select(booking => booking.Start)
                .Concat(simulation.Select(booking => booking.End))
                .Distinct();

            foreach (var date in dates)
            {
                var bookingsPerDay = simulation.Count(booking => booking.IsOngoing(date));

                if (bookingsPerDay > updatedUnits)
                    throw new ApplicationException("Unable to change rental units value as it would affect existing bookings");
            }
        }

        static Booking[] SimulateBookings(Booking[] data, int additionalNights)
        {
            var simulation = data.DeepCopy();
                
            foreach (var simulatedBooking in simulation)
            {
                simulatedBooking.SetNights(simulatedBooking.Nights + additionalNights);
            }

            return simulation;
        }
        
        Rental UpdateRental(int rentalId, DateTime date, UpdateRentalBindingModel model)
        {
            var rental = rentals.GetOne(rentalId);
            
            rental.SetUnits(model.Units);
            rental.SetPreparationTimeInDays(model.PreparationTimeInDays, date);

            rentals.Update(rental);

            return rental;
        }
        
        Booking[] UpcomingBookings(DateTime from, int rentalId)
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