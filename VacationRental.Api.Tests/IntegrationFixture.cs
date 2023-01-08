using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Xunit;

namespace VacationRental.Api.Tests
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
