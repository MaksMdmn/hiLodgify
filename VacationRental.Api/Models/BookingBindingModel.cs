using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Models
{
    public class BookingBindingModel
    {
        public int RentalId { get; set; }

        //TODO: better to do in business layer
        public DateTime Start
        {
            get => startIgnoreTime;
            set => startIgnoreTime = value.Date;
        }

        DateTime startIgnoreTime;
        
        [Range(1, int.MaxValue)]
        public int Nights { get; set; }
    }
}
