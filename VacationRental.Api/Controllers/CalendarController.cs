using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        readonly ICalendarService calendar;

        public CalendarController(ICalendarService calendar)
        {
            this.calendar = calendar;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
                throw new ApplicationException("Nights must be positive");

            return calendar.Create(rentalId, start, nights);
        }
    }
}
