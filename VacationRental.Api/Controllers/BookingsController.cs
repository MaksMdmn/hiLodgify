using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        readonly IRentalRepository rentals;
        readonly IMapper mapper;

        public BookingsController(IRentalRepository rentals, IMapper mapper)
        {
            this.rentals = rentals;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            var booking = rentals.GetByBookingId(bookingId)
                .Bookings.First(b => b.Id == bookingId);

            return ToViewModel<BookingViewModel>(booking);
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");
            
            var rental = rentals.GetOne(model.RentalId);

            var id = rental.TryBookUnit(model.Start, model.Nights);

            return ToViewModel(id);
        }

        static ResourceIdViewModel ToViewModel(int id)
        {
            return new ResourceIdViewModel { Id = id };
        }

        TViewModel ToViewModel<TViewModel>(object source)
        {
            return mapper.Map<TViewModel>(source);
        }
    }
}
