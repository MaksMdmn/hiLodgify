using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        readonly IRentalRepository rentals;
        readonly IMapper mapper;

        public RentalsController(IRentalRepository rentals, IMapper mapper)
        {
            this.rentals = rentals;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = rentals.GetOne(rentalId);

            return ToViewModel<RentalViewModel>(rental);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var rental = new Rental(model.Units, model.PreparationTimeInDays);

            var id = rentals.Add(rental);

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
