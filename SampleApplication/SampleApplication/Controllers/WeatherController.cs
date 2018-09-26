using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public WeatherController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "I am alive!";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)//London,uk
        {
            var apiUrl = _configuration.GetValue<string>("URL");
            var token = _configuration.GetValue<string>("Token");
            var weatherUrl = $"{apiUrl}{id}&appid={token}";
            using (HttpClient client = new HttpClient())
            {
                var request = client.GetAsync(weatherUrl).Result;
                if (request.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return $"Sorry, failed to get weather from {weatherUrl} for {id}";
                }
                var content = request.Content.ReadAsStringAsync().Result;
                dynamic result = JsonConvert.DeserializeObject(content);
                double celsius = (double)result.main.temp - 273.15;
                string message = $"Temperature in {id} is {celsius} Celsius";
                return message;
            }           
        }
    }
}
