using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Application.Interfaces;
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
            return service.ComposeCalendar(rentalId, start, nights);
        }
    }
}
