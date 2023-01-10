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
        readonly ICalendarService service;

        public CalendarController(ICalendarService service)
        {
            this.service = service;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
                throw new ApplicationException("Nights must be positive");

            return service.ComposeCalendar(rentalId, start, nights);
        }
    }
}
