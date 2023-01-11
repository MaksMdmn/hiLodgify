using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using VacationRental.Api.Application.Interfaces;
using VacationRental.Api.Application.Mappers;
using VacationRental.Api.Application.Services;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Tests.UnitTests.Data;
using Xunit;

namespace VacationRental.Tests.UnitTests.Api.Services.Bookings
{
    public class BookingServiceFixtureBase
    {
        protected readonly Mock<IRentalRepository> Rentals;
        protected readonly Mock<IBookingRepository> Bookings;
        
        protected readonly IMapper Mapper;

        protected IBookingService Service;

        public BookingServiceFixtureBase()
        {
            Rentals = new Mock<IRentalRepository>();
            Bookings = new Mock<IBookingRepository>();
            
            Mapper = new Mapper(new MapperConfiguration(ConfigureMapping));

            Service = new BookingService(Rentals.Object, Bookings.Object, Mapper);
        }
        
        protected void SetupRentalsRepositoryGetOne(Func<int, bool> match, Rental value)
        {
            Rentals
                .Setup(r => r.GetOne(It.Is<int>(input => match(input))))
                .Returns(value);
        }
        
        protected void SetupRentalsRepositoryUpdate()
        {
            Rentals
                .Setup(r => r.Update(It.IsAny<Rental>()));
        }
        
        protected void SetupBookingRepositoryAdd(int value)
        {
            Bookings
                .Setup(r => r.Add(It.IsAny<Booking>()))
                .Returns(value);
        }
        
        protected void SetupBookingsRepositoryGetOne(Func<int, bool> match, Booking value)
        {
            Bookings
                .Setup(r => r.GetOne(It.Is<int>(input => match(input))))
                .Returns(value);
        }
        
        protected void SetupBookingsRepositoryGetManyByRentalId(Func<int, bool> match, params Booking[] value)
        {
            Bookings
                .Setup(r => r.GetManyByRenalId(It.Is<int>(input => match(input))))
                .Returns(value);
        }
        
        protected void AssertQueriedRental(Func<int, bool> match) 
        {
            Rentals.Verify(p => p.GetOne(
                    It.Is<int>(input => match(input))),
                Times.Once);
        }
        
        protected void AssertUpdatedRental(Func<Rental, bool> match) 
        {
            Rentals.Verify(p => p.Update(
                    It.Is<Rental>(input => match(input))),
                Times.Once);
        }
        
        protected void AssertQueriedBooking(Func<int, bool> match)
        {
            AssertQueriedBooking(match, Times.Once);
        }
        
        protected void AssertQueriedBooking(Func<int, bool> match,  Func<Times> times) 
        {
            Bookings.Verify(p => p.GetOne(
                    It.Is<int>(input => match(input))),
                times);
        }
        
        protected void AssertBookingByRentalIdQueried(Func<int, bool> match) 
        {
            Bookings.Verify(p => p.GetManyByRenalId(
                    It.Is<int>(input => match(input))),
                Times.Once);
        }

        protected static void AssertEqual<T>(T actual, T expected)
        {
            Assert.Equal(expected, actual);
        }
        
        protected static void AssertAreEquivalent<TActual, TExpected>(TActual actual, TExpected expected)
        {
            actual.Should().NotBeNull();
            //To save time
            actual.Should().BeEquivalentTo(expected, opt => opt.ExcludingMissingMembers());
        }

        static void ConfigureMapping(IMapperConfigurationExpression mapping)
        {
            mapping.AddProfile<ViewModelsProfile>();
        }
    }
}