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

        public static bool EquivalentExcludingMissingMembers<TActual, TExpected>(this TActual actual, TExpected expected, params string[] properties)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());

                return true;
            }
            catch (XunitException)
            {
                return false;
            }
        }
    }
}