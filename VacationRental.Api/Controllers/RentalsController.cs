using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        readonly IRentalRepository rentals;

        public RentalsController(IRentalRepository rentals)
        {
            this.rentals = rentals;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = rentals.GetOne(rentalId);
            
            //TODO: error handling ?
            return new RentalViewModel { Id = rental.Id, Units = rental.Units, PreparationTimeInDays = rental.PreparationTimeInDays };
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var rental = new Rental(model.Units, model.PreparationTimeInDays);

            var id = rentals.Add(rental);
            
            //TODO: mapping
            return new ResourceIdViewModel { Id = id };
        }
    }
}
