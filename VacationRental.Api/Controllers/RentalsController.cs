using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        readonly IDictionary<int, RentalViewModel> rentals;

        public RentalsController(IDictionary<int, RentalViewModel> rentals)
        {
            this.rentals = rentals;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (!rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = rentals.Keys.Count + 1 };

            rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units
            });

            return key;
        }
    }
}
