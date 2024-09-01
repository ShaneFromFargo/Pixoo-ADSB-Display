using AdsbDisplay.ADSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static AdsbDisplay.ADSB.Models.SkyResponse;

namespace AdsbDisplay.ADSB.Feeders
{
    public class APIFeeder : IAPIFeeder
    {
        private readonly IConfiguration _configuration;
        private readonly string _lat;
        private readonly string _lon;
        private readonly string _radius;
        private readonly HttpClient _client;
        public APIFeeder(IConfiguration configuration, IHttpClientFactory httpClientFactory) 
        { 
            _configuration = configuration;
            _lat = _configuration["Flags:Latitude"];
            _lon = _configuration["Flags:Longitude"];
            _radius = _configuration["Flags:Radius"];
            _client = httpClientFactory.CreateClient();
        }

        public async Task<SkyResponse> GetSky()
        {
            SkyResponse sky = await GetAdsbExchangeFeeder();
            return sky;
        }

        public async Task<Ac> GetPlane(string callSign)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://adsbexchange-com1.p.rapidapi.com/v2/callsign/{callSign}/"),
                Headers =
    {
        { "X-RapidAPI-Key", _configuration["Flags:RapidAPIKey"] },
        { "X-RapidAPI-Host", "adsbexchange-com1.p.rapidapi.com" },
    },
            };
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                SkyResponse sky = JsonSerializer.Deserialize<SkyResponse>(body);
                Ac plane = sky.ac.FirstOrDefault();
                return plane;
            }
        }

        //AdsbOne feeder is completely free, no API key required
        //https://adsb.one/
        //Rate limit of 1 request per 2 seconds
        //Unable to use this provider due to SSL Issue see link below
        //https://github.com/ADSB-One/api/issues/2
        private SkyResponse GetAdsbOneFeeder()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                //RequestUri = new Uri($"https://api.adsb.one/v2/point/{_lat}/{_lon}/{_radius}"),
                RequestUri = new Uri("https://api.adsb.one/v2/point/32.792671/-96.802589/5")
            };
            
            var response = _client.Send(request);
            response.EnsureSuccessStatusCode();
            var body = response.Content.ReadAsStringAsync().Result;
            SkyResponse sky = JsonSerializer.Deserialize<SkyResponse>(body);
            return sky;
        }

        private async Task<SkyResponse> GetAdsbExchangeFeeder()
        {

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://adsbexchange-com1.p.rapidapi.com/v2/lat/{_lat}/lon/{_lon}/dist/{_radius}/"),
                Headers =
    {
        { "X-RapidAPI-Key", _configuration["Flags:RapidAPIKey"] },
        { "X-RapidAPI-Host", "adsbexchange-com1.p.rapidapi.com" },
    },
            };
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                SkyResponse sky = JsonSerializer.Deserialize<SkyResponse>(body);
                return sky;
            }
        }


    }
}
