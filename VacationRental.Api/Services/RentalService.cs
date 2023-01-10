using System;
using System.Linq;
using AutoMapper;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Services
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
            var rental = rentals.GetOne(rentalId);

            var date = Today();

            var upcoming = UpcomingBookings(rentalId, date);

            CheckUnitsAvailability(rental, upcoming, model.Units, date);
            CheckPreparationsAvailability(rental, upcoming, model.PreparationTimeInDays);

            UpdateRental(rental, date, model);

            return ToViewModel<RentalViewModel>(rental);
        }

        static void CheckUnitsAvailability(Rental rental, Booking[] upcoming, int updatedUnits, DateTime from)
        {
            if (!upcoming.Any())
                return;
            
            if (updatedUnits >= rental.Units) 
                return;
            
            var days = BookedPeriodInDays(upcoming, from);

            for (var i = 0; i <= days; i++)
            {
                var bookingsPerDay = upcoming.Count(booking => booking.IsOngoing(from.AddDays(i)));

                if (bookingsPerDay > updatedUnits)
                    throw new ApplicationException("Unable to change rental units value as it would affect existing bookings");
            }
        }

        static void CheckPreparationsAvailability(Rental rental, Booking[] upcoming, int preparationDays)
        {
            if (!upcoming.Any())
                return;
            
            if (preparationDays <= rental.PreparationTimeInDays) 
                return;

            var count = upcoming
                .TakeWhile((item, index) => IsOverlapping(upcoming, index, preparationDays))
                .Count();
            
            if (count < upcoming.Length)
                throw new ApplicationException("Unable to change rental preparation days value as it would affect existing bookings");
        }
        
        static int BookedPeriodInDays(Booking[] upcoming, DateTime from)
        {
            //Simplest, but not the best approach
            return 1 + upcoming.Max(booking => booking.End)
                .Subtract(from).Days;
        }
        
        static bool IsOverlapping(Booking[] upcoming, int index, int preparationDays)
        {
            if (index == upcoming.Length - 1)
                return true;
            
            var updatedEnd = upcoming[index].End.AddDays(preparationDays);
            
            return upcoming[index + 1].IsOngoing(updatedEnd);
        }

        void UpdateRental(Rental rental, DateTime date, UpdateRentalBindingModel model)
        {
            rental.SetUnits(model.Units);
            rental.SetPreparationTimeInDays(model.PreparationTimeInDays, date);

            rentals.Update(rental);
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