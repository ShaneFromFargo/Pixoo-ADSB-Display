using AdsbDisplay.ADSB.Feeders;
using AdsbDisplay.ADSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdsbDisplay.ADSB
{
    public class LocalFeeder : ILocalFeeder
    {
        private readonly CustomLogger<Main> _logger;
        private readonly IConfiguration _configuration;
        private string _feederIp;
        private bool _ready;
        private bool _seeking = false;
        private readonly HttpClient _client;

        public LocalFeeder(CustomLogger<Main> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _feederIp = _configuration["Flags:LocalFeederIp"];
            _client = httpClientFactory.CreateClient();
        }

        public async Task<SkyResponse> GetSky()
        {
            //Checking if feeder is ready
            //if (!_ready && (_seeking == false))
            //{
            //    await FindLocalADSBFeeder();
            //    _logger.Log("Program has started looking for Feeder ");
            //    return null;
            //}
            //if (!_ready && (_seeking == true))
            //{
            //    _logger.Log("Program is still looking for Feeder ");
            //    return null;
            //}

            //Feeder is ready, getting all planes from local feeder
            SkyResponse response = GetSkyFromFeeder();
            return response;
        }

        //Get all planes from local feeder
        public SkyResponse GetSkyFromFeeder()
        {
            if (!_feederIp.StartsWith("http://") && !_feederIp.StartsWith("https://"))
            {
                // If not, concatenate "http://" with the _feederIp
                _feederIp = "http://" + _feederIp;
            }
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_feederIp+"/tar1090/data/aircraft.json"),
            };
            try
            {
                var response = _client.Send(request);
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().Result;
                SkyResponse sky = JsonSerializer.Deserialize<SkyResponse>(body);
                return sky;
            }
            catch (Exception ex)
            {
                _logger.LogError("Local Feeder Error: " + ex.Message, "LocalFeeder.cs | Line 70");
                _logger.Log("Error getting planes from local feeder: " + ex.Message);
                return null;
            }
        }



        //private async Task<bool> FindLocalADSBFeeder()
        //{
          
        //    _seeking = true;
        //    //Using custom HttpClient to set timeout
        //    HttpClient client = new HttpClient();
        //    string url = $"http://{_feederIp}/tar1090"; // Currently only supports tar1090. If you have another local feeder protocol, you will need to change this.
        //    client.Timeout = TimeSpan.FromSeconds(1);
        //    try
        //    {
        //        HttpResponseMessage response = client.GetAsync(url).Result;
        //        if (response.IsSuccessStatusCode) // If status code is 200-299
        //        {
        //            _logger.Log("Local Feeder Found. Connected to " + url);
        //            _feederIp = _feederIp;
        //            _seeking = false;
        //            _ready = true;
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        _logger.Log("Local Feeder not found at: " + _feederIp + ". Scanning network...");
        //        // Specific IP not found or not listening on this port
        //    }
        //    HttpClient client2 = new HttpClient();
        //    client2.Timeout = TimeSpan.FromSeconds(.1);
        //    for (int i = 1; i <= 255; i++)
        //    {

        //        string subnet = "192.168.68.";
        //        string ipAddress = subnet + i.ToString();
        //        string urlScan = $"http://{ipAddress}/tar1090"; // Currently only supports tar1090. If you have another local feeder protocol, you will need to change this.
        //        _logger.Log(ipAddress);
        //        try
        //        {
        //            HttpResponseMessage response = await client2.GetAsync(urlScan);
        //            if (response.IsSuccessStatusCode) // If status code is 200-299
        //            {
        //                _logger.Log("Device found: " + urlScan);
        //                _logger.Log("Local Feeder Found. Consider updating your feeder IP address in the app config for faster startup. IP: " + ipAddress);
        //                _feederIp = ipAddress;
        //                _seeking = false;
        //                _ready = true;
        //                return true;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            // Device not found or not listening on this port
        //        }
        //    }

        //    return false;
        //}
    }
}
