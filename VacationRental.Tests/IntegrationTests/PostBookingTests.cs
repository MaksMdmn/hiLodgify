using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models.BindingModels;
using VacationRental.Api.Models.ViewModels;
using Xunit;

namespace VacationRental.Tests.IntegrationTests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        readonly HttpClient client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new CreateRentalBindingModel
            {
                Units = 4
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await client.PostAsJsonAsync("/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 3,
                 Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var postRentalRequest = new CreateRentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await client.PostAsJsonAsync("/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await client.PostAsJsonAsync("/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (await client.PostAsJsonAsync("/api/v1/bookings", postBooking2Request)) { }
            });
        }
    }
}
