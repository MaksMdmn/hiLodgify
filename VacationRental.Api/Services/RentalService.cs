using System;
using AutoMapper;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        readonly IRentalRepository rentals;
        readonly IMapper mapper;

        public RentalService(IRentalRepository rentals, IMapper mapper)
        {
            this.rentals = rentals;
            this.mapper = mapper;
        }

        public RentalViewModel GetOne(int rentalId)
        {
            var rental = rentals.GetOne(rentalId);
            
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return ToViewModel<RentalViewModel>(rental);
        }

        public ResourceIdViewModel CreateRental(RentalBindingModel model)
        {
            var rental = new Rental(model.Units, model.PreparationTimeInDays);

            var id = rentals.Add(rental);

            return ToViewModel(id);
        }

        TViewModel ToViewModel<TViewModel>(object source)
        {
            return mapper.Map<TViewModel>(source);
        }
        
        static ResourceIdViewModel ToViewModel(int id)
        {
            return new ResourceIdViewModel { Id = id };
        }
    }
}