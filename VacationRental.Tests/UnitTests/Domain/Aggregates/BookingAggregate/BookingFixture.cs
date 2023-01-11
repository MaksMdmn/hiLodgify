using System;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Tests.UnitTests.Data;
using Xunit;

namespace VacationRental.Tests.UnitTests.Domain.Aggregates.BookingAggregate
{
    public class BookingFixture
    {
        [Fact]
        public void GivenPositiveValue_WhenSettingNights_ThenChangeNights()
        {
            var expected = AllNights + 666;
            
            Booking.SetNights(expected);

            var expectedEnd = Booking.Start.AddDays(Booking.Nights);

            Assert.Equal(expected, Booking.Nights);
            Assert.Equal(expectedEnd, Booking.End);
        }
        
        [Theory]
        [InlineData(-666)]
        [InlineData(-1)]
        [InlineData(0)]
        public void GivenNegativeOrZeroValue_WhenSettingNights_ThrowAnError(int expected)
        {
            var exception = Assert.Throws<ApplicationException>(() =>
                Booking.SetNights(expected));

            AssertUnitsException(exception);
        }
        
        [Theory]
        [MemberData(nameof(WithinRangeDates))]
        public void GivenWithinRangeDate_WhenCheckingIfOngoing_ReturnsTrue(DateTime date)
        {
            var actual = Booking.IsOngoing(date);
            
            Assert.True(actual);
        }
        
        [Theory]
        [MemberData(nameof(OutOfRangeDates))]
        public void GivenOutOfRangeDate_WhenCheckingIfOngoing_ReturnsFalse(DateTime date)
        {
            var actual = Booking.IsOngoing(date);
            
            Assert.False(actual);
        }
        
        [Theory]
        [MemberData(nameof(OverlappedDateRange))]
        public void GivenOverlappedDateRange_WhenCheckingIfOngoing_ReturnsTrue(DateTime date, int nights)
        {
            var actual = Booking.IsOngoing(date, nights);
            
            Assert.True(actual);
        }
        
        [Theory]
        [MemberData(nameof(NotOverlappedDateRange))]
        public void GivenNotOverlappedDateRange_WhenCheckingIfOngoing_ReturnsFalse(DateTime date, int nights)
        {
            var actual = Booking.IsOngoing(date, nights);
            
            Assert.False(actual);
        }
        
        static void AssertUnitsException(ApplicationException exception)
        {
            Assert.Contains(exception.Message, "Units cannot be less than 1");
        }

        static DateTime SomeDaysBefore(DateTime date)
        {
            return date.AddDays(-1 * SomeNights);
        }
        
        static DateTime DayBefore(DateTime date)
        {
            return date.AddDays(-1);
        }
        
        static DateTime DayAfter(DateTime date)
        {
            return date.AddDays(1);
        }
        
        static DateTime SomeDaysAfter(DateTime date)
        {
            return date.AddDays(SomeNights);
        }

        readonly Booking Booking = new BookingFaker(Start, AllNights).Generate();

        const int OneNight = 1;
        const int SomeNights = AllNights / 2;
        const int AllNights = 8;
        const int LotsOfNights = AllNights * 2;
        
        static readonly DateTime Start = DateTime.Today.Date;
        static readonly DateTime End = Start.AddDays(AllNights);
        
        public static object[][] WithinRangeDates =
        {
            new object[] { Start },
            new object[] { DayAfter(Start) },
            new object[] { SomeDaysAfter(Start) },
            new object[] { DayBefore(End) }
        };
        
        public static object[][] OutOfRangeDates =
        {
            new object[] { SomeDaysBefore(Start) },
            new object[] { DayBefore(Start) },
            new object[] { End },
            new object[] { DayAfter(End) },
            new object[] { SomeDaysAfter(End) }
        };
        
        public static object[][] OverlappedDateRange =
        {
            new object[] { Start, OneNight },
            new object[] { Start, SomeNights },
            new object[] { Start, AllNights },
            new object[] { Start, LotsOfNights },
            
            new object[] { SomeDaysBefore(Start), LotsOfNights },
            
            new object[] { SomeDaysAfter(Start), OneNight },
            new object[] { SomeDaysAfter(Start), SomeNights },
            new object[] { SomeDaysAfter(Start), AllNights }
        };
        
        public static object[][] NotOverlappedDateRange =
        {
            new object[] { SomeDaysBefore(Start), SomeNights },
            
            new object[] { End, LotsOfNights},
            
            new object[] { DayAfter(End), LotsOfNights },
        };
    }
}