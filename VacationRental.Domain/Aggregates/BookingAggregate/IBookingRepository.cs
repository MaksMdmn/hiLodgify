namespace VacationRental.Domain.Aggregates.BookingAggregate
{
    public interface IBookingRepository
    {
        int Add(Booking booking);

        Booking GetOne(int id);

        Booking[] GetManyByRenalId(int rentalId);
    }
}