﻿using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Models.BindingModels
{
    // I considered that partial update is not allowed
    public class UpdateRentalBindingModel
    {
        [Range(1 , int.MaxValue)]
        public int Units { get; set; }

        [Range(0 , int.MaxValue)]
        public int PreparationTimeInDays { get; set; }
    }
}
