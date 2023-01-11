using System;
using Moq;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Tests.UnitTests.Data;
using VacationRental.Tests.UnitTests.Extensions;
using Xunit;

namespace VacationRental.Tests.UnitTests.Api.Services.Bookings
{
    public class BookingServiceFixture : BookingServiceFixtureBase
    {
        public BookingServiceFixture()
        {
            Rental = new RentalFaker(Units, PreparationTimeInDays).Generate();
            
            Booking1 = new BookingFaker(Rental.Id, FirstUnit, SomeDate).Generate();
            Booking2 = new BookingFaker(Rental.Id, SecondUnit, SomeDate).Generate();
            
            Bookings = new [] { Booking1, Booking2 };
            
            SetupRentalsRepositoryGetOne(id => id == Rental.Id, Rental);
            
            SetupBookingsRepositoryGetOne(id => id == Booking1.Id, Booking1);
            SetupBookingsRepositoryGetOne(id => id == Booking2.Id, Booking2);
        }
        
        [Fact]
        public void GivenId_WhenGettingOne_ThenReturnBooking()
        {
            var result = Service.GetOne(Booking1.Id);

            AssertAreEquivalent(result, Booking1);
            
            AssertQueriedBooking(id => id == Booking1.Id, Times.Once);
            AssertQueriedBooking(id => id == Booking2.Id, Times.Never);
        }
        
        [Fact]
        public void GivenValidModel_WhenMakingBooking_ThenReturnCreatedBookingId()
        {
            Relate(Rental, Booking1);
            
            SetupBookingsRepositoryGetManyByRentalId(id => id == Rental.Id, Booking1);

            SetupBookingRepositoryAdd(Booking2.Id);

            var model = new BookingBindingModel
            {
                RentalId = Rental.Id, 
                Nights = 3, 
                Start = SomeDate
            };
            
            var result = Service.MakeBooking(model);
            
            AssertEqual(result.Id, Booking2.Id);
            AssertEqual(Rental.Preparations.Count, Bookings.Length);
            
            AssertQueriedRental(id => id == Rental.Id);
            AssertBookingByRentalIdQueried(id => id == Rental.Id);
            AssertUpdatedRental(input => input.EquivalentTo(Rental));
        }
        
        [Fact]
        public void GivenOccupiedRental_WhenMakingBooking_ThenThrowAnError()
        {
            Relate(Rental, Booking1, Booking2);
            
            SetupBookingsRepositoryGetManyByRentalId(id => id == Rental.Id, Bookings);

            SetupBookingRepositoryAdd(SomeBooking.Id);

            var model = new BookingBindingModel
            {
                RentalId = Rental.Id, 
                Nights = 3, 
                Start = SomeDate
            };
            
            var exception = Assert.Throws<ApplicationException>(() =>
                Service.MakeBooking(model));

            AssertNotAvailableException(exception);
            
            AssertQueriedRental(id => id == Rental.Id);
            AssertBookingByRentalIdQueried(id => id == Rental.Id);
        }
        
        static void AssertNotAvailableException(ApplicationException exception)
        {
            Assert.Contains(exception.Message, "Not available");
        }

        static void Relate(Rental rental, params Booking[] bookings)
        {
            foreach (var booking in bookings)
            {
                rental.SchedulePreparation(booking.End, booking.Unit);
            }
        }

        const int Units = 2;
        const int PreparationTimeInDays = 1;
        
        const int FirstUnit = 1;
        const int SecondUnit = 2;
        
        static readonly DateTime SomeDate = DateTime.Now;

        static readonly Booking SomeBooking = new BookingFaker().Generate();
        
        readonly Rental Rental;
        readonly Booking Booking1;
        readonly Booking Booking2;

        readonly Booking[] Bookings;
    }
}