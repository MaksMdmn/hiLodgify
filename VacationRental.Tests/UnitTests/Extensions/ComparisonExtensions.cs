using FluentAssertions;
using Xunit.Sdk;

namespace VacationRental.Tests.UnitTests.Extensions
{
    public static class ComparisonExtensions
    {
        public static bool EquivalentTo<TActual, TExpected>(this TActual actual, TExpected expected)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected);

                return true;
            }
            catch (XunitException)
            {
                return false;
            }
        }
    }
}