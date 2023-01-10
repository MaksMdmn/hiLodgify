using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Api.Application.Interfaces
{
    public interface IBookingService
    {
        BookingViewModel GetOne(int bookingId);

        ResourceIdViewModel MakeBooking(BookingBindingModel model);
    }
}