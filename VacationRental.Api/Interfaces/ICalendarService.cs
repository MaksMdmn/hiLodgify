using System;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Api.Interfaces
{
    public interface ICalendarService
    {
        CalendarViewModel ComposeCalendar(int rentalId, DateTime start, int nights);
    }
}