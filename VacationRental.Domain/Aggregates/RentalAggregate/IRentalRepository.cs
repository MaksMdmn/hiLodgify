namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public interface IRentalRepository
    {
        int Add(Rental rental);

        Rental GetOne(int id);

        Rental FindByBookingId(int bookingId);
    }
}