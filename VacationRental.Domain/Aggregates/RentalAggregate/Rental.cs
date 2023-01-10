using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public class Rental : IAggregateRoot, IEntity
    {
        readonly List<PreparationTime> preparations = new List<PreparationTime>();

        public int Id { get; set; }

        public int Units { get; private set; }

        public int PreparationTimeInDays { get; private set; }

        public IReadOnlyCollection<PreparationTime> Preparations => preparations;

        public Rental(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public void SchedulePreparation(DateTime start, int unit)
        {
            preparations.Add(new PreparationTime(start, unit, PreparationTimeInDays));
        }

        public int[] FindPreparedUnits(DateTime start, int nights)
        {
            return Enumerable.Range(1, Units).Except(preparations
                .Where(preparation => preparation.IsOngoing(start, nights))
                .Select(preparation => preparation.Unit))
                .ToArray(); 
        }

        public void SetUnits(int units)
        {
            if (units < 1)
                throw new ApplicationException("Units cannot be less than 1");
            
            Units = units;
        }

        public void SetPreparationTimeInDays(int preparationTimeInDays, DateTime from)
        {
            if (preparationTimeInDays < 0)
                throw new ApplicationException("Preparation time cannot be less than 0");
            
            PreparationTimeInDays = preparationTimeInDays;

            var update = preparations.Where(preparation => preparation.Start >= from);

            foreach (var preparation in update)
            {
                preparation.SetDays(PreparationTimeInDays);
            }
        }
    }
}