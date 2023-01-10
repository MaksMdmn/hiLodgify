namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public interface IRentalRepository
    {
        Rental GetOne(int id);

        int Add(Rental rental);

        void Update(Rental rental);
    }
}