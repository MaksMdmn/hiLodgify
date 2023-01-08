using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        readonly IDictionary<int, RentalViewModel> rentals;
        readonly IDictionary<int, BookingViewModel> bookings;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            this.rentals = rentals;
            this.bookings = bookings;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            if (!bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return bookings[bookingId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");
            if (!rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookings.Values)
                {
                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }
                if (count >= rentals[model.RentalId].Units)
                    throw new ApplicationException("Not available");
            }


            var key = new ResourceIdViewModel { Id = bookings.Keys.Count + 1 };

            bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            return key;
        }
    }
}
