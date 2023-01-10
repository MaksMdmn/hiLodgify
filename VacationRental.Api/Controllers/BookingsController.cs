using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Application.Interfaces;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        readonly IBookingService service;

        public BookingsController(IBookingService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            return service.GetOne(bookingId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");

            return service.MakeBooking(model);
        }
    }
}
