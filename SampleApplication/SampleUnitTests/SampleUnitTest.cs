using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using SampleApplication;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace SampleUnitTests
{
    public class SampleUnitTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly string _weatherUrl = "http://www.google.com/q=";
        public SampleUnitTest()
        {
            // Arrange
            var mock = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseSetting("URL", _weatherUrl);
            
            _server = new TestServer(mock);
            _client = _server.CreateClient();
        }
        //This test check general 404 error handling
        [Fact]
        public async Task CheckHttpErrorTestAsync()
        {
            var response = await _client.GetAsync("/api/some-wrong-endpoint");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Status code page, status code: 404", 
                response.Content.ReadAsStringAsync().Result);
        }

        //This test check an error handling if our 3rd party service return us 404 response
        [Fact]
        public async Task CheckWeatherNotFoundTestAsync()
        {
            var location = "kiev";
            var response = await _client.GetAsync($"/api/weather/{location}");
            var result = response.Content.ReadAsStringAsync().Result;
            Assert.Contains($"Sorry, failed to get weather from {_weatherUrl}", result);
            Assert.Contains($"for {location}", result);
        }
    }
}
