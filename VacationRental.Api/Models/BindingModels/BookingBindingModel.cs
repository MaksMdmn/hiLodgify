using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Models.BindingModels
{
    public class BookingBindingModel
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => startIgnoreTime;
            set => startIgnoreTime = value.Date;
        }

        DateTime startIgnoreTime;
        
        [Range(1 , int.MaxValue)]
        public int Nights { get; set; }
    }
}
