using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using VacationRental.Api;
using Xunit;

namespace VacationRental.Tests.Api.IntegrationTests
{
    [CollectionDefinition("Integration")]
    public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
    {
        readonly TestServer server;

        public HttpClient Client { get; }

        public IntegrationFixture()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            Client = server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            server.Dispose();
        }
    }
}
