using AutoMapper;
using VacationRental.Api.Models.ViewModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Mappers
{
    public class ViewModelsProfile : Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<Rental, RentalViewModel>();
            
            CreateMap<Booking, BookingViewModel>();
            CreateMap<Booking, CalendarBookingViewModel>();
            
            CreateMap<PreparationTime, CalendarPreparationTimeViewModel>();
        }
    }
}