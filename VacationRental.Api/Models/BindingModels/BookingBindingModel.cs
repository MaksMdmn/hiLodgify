using System;

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
        
        public int Nights { get; set; }
    }
}
