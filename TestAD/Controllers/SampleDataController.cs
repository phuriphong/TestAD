using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace TestAD.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CallApi([FromBody]restmodel model)
        {
            try
            {
                var client = new RestClient(model.Url);
                var request = new RestRequest(model.Method);
                request.AddParameter("application/x-www-form-urlencoded", model.Data, ParameterType.RequestBody);
                IRestResponse response = await client.ExecuteAsync(request);
                {
                    return Ok(new { response.Content, response = response.ErrorException == null ? "" : response.ErrorException.Message, response.ErrorMessage });
                }

            }
            catch (Exception ex)
            {
                return Ok(new { IsDone = false, ex.Message, ex });
            }


        }

        [HttpPost("AB/[action]")]
        public ActionResult tttt([FromBody]restmodel model)
        {
            try
            {
                var client = new RestClient(model.Url);
                var request = new RestRequest(model.Method);
                var body = model.Data;
                request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                {
                    return Ok(new { response.Content, response = response.ErrorException == null ? "" : response.ErrorException.Message, response.ErrorMessage });
                }

            }
            catch (Exception ex)
            {
                return Ok(new { IsDone = false, ex.Message, ex });
            }


        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PostData([FromBody]restmodel model)
        {
            try
            {
                var dict = new Dictionary<string, string>();
                dict.Add("grant_type", "authorization_code");
                dict.Add("client_id", "039bb643-891d-4abd-846d-a38c2e89adad");
                dict.Add("redirect_uri", "https://test-scgdistpurchasing.nexterdigitals-dev.com/login");
                dict.Add("code", model.Code);
                var client = new HttpClient();
                var req = new HttpRequestMessage(HttpMethod.Post, model.Url) { Content = new FormUrlEncodedContent(dict) };
                var res = await client.SendAsync(req);
                return Ok(res.Content.ReadAsStringAsync().Result);
            }
            catch (WebException we)
            {
                throw new Exception(we.Message);
            }
            catch (Exception ex)
            {
                return Ok(new { IsDone = false, ex.Message });
            }


        }
        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
        public class restmodel
        {
            public string Url { get; set; }
            public Method Method { get; set; }
            public string Output { get; set; }
            public string Data { get; set; }
            public string Code { get; set; }
        }
    }
}
