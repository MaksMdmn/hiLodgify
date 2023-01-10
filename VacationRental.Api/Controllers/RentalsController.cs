using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        readonly IRentalService service;

        public RentalsController(IRentalService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            return service.GetOne(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(CreateRentalBindingModel model)
        {
            return service.Create(model);
        }
        
        
        //Response was not specified, so I did is as I thought would be right
        [HttpPost]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, UpdateRentalBindingModel model)
        {
            return service.Update(rentalId, model);
        }
    }
}
