using AdsbDisplay.ADSB.Feeders;
using AdsbDisplay.ADSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static AdsbDisplay.ADSB.Models.SkyResponse;

namespace AdsbDisplay.ADSB
{
    public class ADSB : IADSB
    {
        private readonly CustomLogger<Main> _logger;
        private readonly IConfiguration _configuration;
        private readonly bool _useLocalFeeder;
        private readonly ILocalFeeder _localFeeder;
        private readonly IAPIFeeder _aPIFeeder;
        private readonly int _radius;
        private string _lastPlaneTail;
        private FlightRadarSearch _lastFlightRadarSearch;
        private readonly HttpClient _client;
        public ADSB(CustomLogger<Main> logger, IConfiguration configuration, ILocalFeeder localFeeder, IAPIFeeder aPIFeeder, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _localFeeder = localFeeder;
            _aPIFeeder = aPIFeeder;
            _radius = int.Parse(_configuration["Flags:Radius"]);
            _client = httpClientFactory.CreateClient();
            if (_configuration["Flags:FeederSource"].ToLower()=="local")
            {
                _useLocalFeeder = true;
            }
            else
            {
                _useLocalFeeder = false;
            }
        }
        
        public async Task<DisplayInformation> GetPlane()
        {
            SkyResponse sky = new SkyResponse();
            if (_useLocalFeeder)
            {
                sky = await _localFeeder.GetSky();
            }
            else
            {
                sky = await _aPIFeeder.GetSky();
            }
           
           
            if (sky==null)
                return null;

            //Some APIs use the AC prop, some use aircrafts. Custom converter populates both props
            sky.EnsureAcAircraft();

            Ac aircraft = FilterAircrafts(sky.aircraft);
            if (aircraft == null)
                return null;
            if (aircraft.flight == null)
                return null;

            string flightNum = aircraft.flight.Replace(" ", string.Empty);
            var flight = GetFlightInfo(flightNum);
            DisplayInformation displayInformation = ExtractVitalFlightInfo(flight, flightNum);
            if (displayInformation == null)
                return null;

            displayInformation.Altitude = aircraft.alt_geom;
            displayInformation.GroundSpeed = aircraft.gs;
            displayInformation.FlightNumber = aircraft.flight;
            return displayInformation;
        }

        public async Task<DisplayInformation> GetRandomPlane()
        {
            SkyResponse sky = new SkyResponse();
            if (_useLocalFeeder)
            {
                sky = await _localFeeder.GetSky();
            }
            else
            {
                sky = await _aPIFeeder.GetSky();
            }


            if (sky == null)
                return null;

            //Some APIs use the AC prop, some use aircrafts. Custom converter populates both props
            sky.EnsureAcAircraft();

            Ac aircraft = GetRandomAircraft(sky);
            if (aircraft == null)
                return null;
            if (aircraft.flight == null)
                return null;

            string flightNum = aircraft.flight.Replace(" ", string.Empty);
            var flight = GetFlightInfo(flightNum);
            DisplayInformation displayInformation = ExtractVitalFlightInfo(flight, flightNum);
            if (displayInformation == null)
                return null;

            displayInformation.Altitude = aircraft.alt_geom;
            displayInformation.GroundSpeed = aircraft.gs;
            displayInformation.FlightNumber = aircraft.flight;
            return displayInformation;
        }

        private Ac GetRandomAircraft(SkyResponse sky)
        {
            Random random = new Random();
            if (sky.aircraft == null || sky.aircraft.Count == 0)
            {
                throw new InvalidOperationException("No aircraft available.");
            }
            int index = random.Next(sky.aircraft.Count);
            return sky.aircraft[index];
        }
        //Makes for easier testing. Type in any flight number and it will return the info
        //Helpful for testing when there are no planes in the sky, or to test a specific plane/airline
        public async Task<DisplayInformation> Test(string flightNumber)
        {
            //If subscribed to API you can get any plane in the sky
            //Ac aircraft = await _aPIFeeder.GetPlane(flightNumber);
            //Otherwise you can get any local plane
           SkyResponse sky = await _localFeeder.GetSky();
            if(sky==null)
            {
                return null;
            }
            Ac aircraft = FilterAircraftsByCallSign(sky.aircraft, flightNumber);
            if(aircraft!=null && aircraft.flight!=null)
            {
                string flightNum = aircraft.flight.Replace(" ", string.Empty);
                var flight = GetFlightInfo(flightNum);
                DisplayInformation displayInformation = ExtractVitalFlightInfo(flight,flightNum);
                if (displayInformation == null)
                    return null;

                displayInformation.Altitude = aircraft.alt_geom;
                displayInformation.GroundSpeed = aircraft.gs;
                displayInformation.FlightNumber = aircraft.flight;
                return displayInformation;
            }
            return null;
        }

        private FlightRadarSearch GetFlightInfo(string tailNumber)
        {
            //If it's the last plane we searched for, return the last search(to avoid hitting the API too much)
            if(_lastPlaneTail != null && _lastFlightRadarSearch != null)
            {
                if(_lastPlaneTail == tailNumber)
                {
                    return _lastFlightRadarSearch;
                }
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://flight-radar1.p.rapidapi.com/flights/search?query=" + tailNumber + "&limit=25"),
                Headers =
    {
        { "X-RapidAPI-Key", _configuration["Flags:RapidAPIKey"] },
        { "X-RapidAPI-Host", "flight-radar1.p.rapidapi.com" },
    },
            };
            //using (var response = await client.SendAsync(request))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var body = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(body);
            //    FlightRadarSearch plane = JsonSerializer.Deserialize<FlightRadarSearch>(body);
            //    return plane;
            //}
            try
            {
                var response = _client.Send(request);
                var body = response.Content.ReadAsStringAsync().Result;
                FlightRadarSearch plane = JsonSerializer.Deserialize<FlightRadarSearch>(body);
                _lastPlaneTail = tailNumber;
                _lastFlightRadarSearch = plane;
                return plane;
            }
            catch(Exception e)
            {
                _logger.LogError("Error getting flight info. Tail Number: "+tailNumber+" Error: " + e.Message, "ADSB.cs Line 138");
                return null;
            }
        

        }

     
        private static DisplayInformation ExtractVitalFlightInfo(FlightRadarSearch annoyingJson, string callsign)
        {
            if (annoyingJson == null || annoyingJson.results == null || annoyingJson.results.Count == 0)
                return null;

            DisplayInformation info = new DisplayInformation();
            foreach (var lostKey in annoyingJson.results)
            {
                if (lostKey.type == "operator")
                {
                    info.Airline = lostKey.name;
                    if (lostKey.detail != null && lostKey.detail.logo != null)
                    {
                        info.AirlineLogo = lostKey.detail.logo;
                    }
                }
                if (lostKey.type == "live")
                {
                    if (lostKey.detail != null)
                    {
                        if (lostKey.detail != null)
                        {
                            //Sometimes flight radar returns multiple flights for the same plane
                            //Example flight SWA42 would also show results for planes in the air with call sign SWA4278
                            if (lostKey.detail.callsign != null && lostKey.detail.callsign.ToLower()==callsign.ToLower())
                            {
                                info.Destination = lostKey.detail.schd_to;
                                info.Origin = lostKey.detail.schd_from;
                                info.FlightDescription = lostKey.detail.route;
                                info.AircraftType = lostKey.detail.ac_type;
                            }
                            
                        }

                    }
                }

            }
            return info;
        }

        private Ac FilterAircrafts(List<Ac> aircrafts)
        {
            if (aircrafts == null || aircrafts.Count==0)
                return null;
            //Look for the most important plane and display that(or display none)
            //Find Emergency aircraft
            foreach (var plane in aircrafts)
            {
                if (plane.emergency != null && plane.emergency != "none")
                    return plane;
            }

            foreach (var plane in aircrafts)
            {
                int altitude = 0;
                int.TryParse(plane.alt_baro, out altitude);
                if (altitude > 1)
                {
                    //Some APIs auto filter for this. But local feeders don't. Filtering here to be safe
                    if (WithinRadius(plane.lat, plane.lon, _radius))
                    {
                        return plane;
                    }
                }
            }
            return null;
        }

        private Ac FilterAircraftsByCallSign(List<Ac> aircrafts, string callSign)
        {
            foreach (var plane in aircrafts)
            {
                if(plane!=null && plane.flight!=null)
                {
                    if(plane.flight.Contains("SWA"))
                    {
                        Console.WriteLine("Plane: " + plane.flight);
                    }
                    string planeFormatted = plane.flight.Trim().ToLower();
                    if (planeFormatted == callSign.ToLower())
                    {
                        return plane;
                    }
                }
            
            }
            return null;
        }

        public bool WithinRadius(double latitude, double longitude, int radius)
        {
            //Hardcoded position of my house
            double homeLat = double.Parse(_configuration["Flags:Latitude"]);
            double homeY = double.Parse(_configuration["Flags:Longitude"]);

            double rlat1 = Math.PI * latitude / 180;
            double rlat2 = Math.PI * homeLat / 180;
            double theta = longitude - homeY;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            if (dist <= radius)
                return true;
            else
                return false;
        }
    }
}
