using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Api.Interfaces
{
    public interface IRentalService
    {
        RentalViewModel GetOne(int rentalId);

        ResourceIdViewModel Create(CreateRentalBindingModel model);
        
        RentalViewModel Update(int rentalId, UpdateRentalBindingModel model);
    }
}