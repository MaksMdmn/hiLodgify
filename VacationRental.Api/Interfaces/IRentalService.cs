using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Api.Interfaces
{
    public interface IRentalService
    {
        RentalViewModel GetOne(int rentalId);

        ResourceIdViewModel CreateRental(RentalBindingModel model);
    }
}