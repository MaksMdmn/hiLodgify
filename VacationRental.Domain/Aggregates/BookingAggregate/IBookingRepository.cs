namespace VacationRental.Domain.Aggregates.BookingAggregate
{
    public interface IBookingRepository
    {
        Booking GetOne(int id);

        Booking[] GetManyByRenalId(int rentalId);
        
        int Add(Booking booking);
    }
}