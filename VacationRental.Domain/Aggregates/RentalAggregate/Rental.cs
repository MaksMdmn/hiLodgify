using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public class Rental : IAggregateRoot, IEntity
    {
        readonly int[] units;
        readonly List<PreparationTime> preparations = new List<PreparationTime>();

        public int Id { get; set; }

        public int Units => units.Length;

        public int PreparationTimeInDays { get; }

        public IReadOnlyCollection<PreparationTime> Preparations => preparations;

        public Rental(int units, int preparationTimeInDays)
        {
            PreparationTimeInDays = preparationTimeInDays;

            this.units = Enumerable.Range(1, units).ToArray();
        }

        public void SchedulePreparation(DateTime start, int unit)
        {
            preparations.Add(new PreparationTime(start, unit, PreparationTimeInDays));
        }

        public int[] FindPreparedUnits(DateTime start, int nights)
        {
            return units.Except(preparations
                .Where(preparation => preparation.IsOngoing(start, nights))
                .Select(preparation => preparation.Unit))
                .ToArray(); 
        }
        
    }
}