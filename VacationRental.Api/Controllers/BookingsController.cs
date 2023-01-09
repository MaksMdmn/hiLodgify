using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        readonly IRentalRepository rentals;
        
        public BookingsController(IRentalRepository rentals)
        {
            this.rentals = rentals;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            //TODO: rework
            var rental = rentals.FindByBookingId(bookingId);
            var booking = rental.Bookings.First(b => b.Id == bookingId);

            //TODO: mapping
            return new BookingViewModel
            {
                Id = booking.Id,
                Nights = booking.Nights,
                RentalId = rental.Id,
                Start = booking.Start
            };
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            var rental = rentals.GetOne(model.RentalId);

            var id = rental.TryBookUnit(model.Start, model.Nights);

            return new ResourceIdViewModel { Id = id };
        }

        
    }
}
