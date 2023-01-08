using System;

namespace VacationRental.Api.Models
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
