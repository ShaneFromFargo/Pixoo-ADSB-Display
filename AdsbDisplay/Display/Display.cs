using AdsbDisplay.ADSB.Models;
using AdsbDisplay.Display.Design;
using AdsbDisplay.Display.Interfaces;
using AdsbDisplay.Models;
using AdsbDisplay.Models.Pixoo;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdsbDisplay.Display
{
    public class Display : IDisplay
    {
        private readonly IPixoo _pixoo;
        private string _lastFlightNumber;
        private readonly IConfiguration _configuration;
        public Display(IPixoo pixoo, IConfiguration configuration)
        {
            _pixoo = pixoo;
            _configuration = configuration;
        }
        public void Aircraft(DisplayInformation di)
        {
            if (di == null)
                return;
         
            AirlineBranding brand = Branding.GenericBranding();
            switch (di.Airline)
            {
                case "Delta Air Lines":
                    brand = Branding.DeltaBranding();
                    break;
                case "Southwest Airlines":
                    brand = Branding.SouthwestBranding();
                    break;
                case "American Airlines":
                    brand = Branding.AmericanBranding();
                    break;
                case "JSX":
                    brand = Branding.JSXBranding();
                    break;
                case "Envoy Air":
                    brand = Branding.AmericanBranding();
                    break;
            }
           
            if (_lastFlightNumber != null && _lastFlightNumber == di.FlightNumber)
            {
                DisplayAltitude(di.Altitude, brand.Altitude);
                return;
            }


            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string backgroundImage = rootDirectory + "Art\\" + "Load.png";
            _pixoo.SendPic(backgroundImage);

            DisplayFlight(di, brand);
            _lastFlightNumber = di.FlightNumber;
          
        }

        public void NoAircraft()
        {
            //Setting the channel works. But after running for a while it gets weird
           // _pixoo.ClearText();
            //Thread.Sleep(1000);
           //_pixoo.SetChannel(0);
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string backgroundImage = rootDirectory + "Art\\" + "Load.png";
            _pixoo.SendPic(backgroundImage);
        }

        private void DisplayFlight(DisplayInformation info, AirlineBranding brand)
        {

            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            brand.BackgroundImage = rootDirectory + "Art\\" + brand.BackgroundImage;
           

            var bareBonesInfo = ChooseDirection(info, brand.BackgroundImage);

            //Default branding
            if(brand.Name == "None" && info.Destination==null)
            {
                bareBonesInfo.Image = rootDirectory + "Art\\" + Branding.GeneralAviation().BackgroundImage;
            }
            Thread.Sleep(1000);
            _pixoo.SendPic(bareBonesInfo.Image);
            _pixoo.ClearText();
            Thread.Sleep(1000);

            Console.WriteLine("Airline: " + brand.Name);
            Console.WriteLine("-------------Sending Display Info to Pixoo-------------");
            if(info.FlightNumber== null)
            {
                return;
            }
            PixooText flightNumber = new PixooText()
            {
                x = brand.FlightNumber.OffsetX,
                y = brand.FlightNumber.OffsetY,
                dir = 0,
                font = 4,
                TextWidth = 56,
                speed = 10,
                TextString = info.FlightNumber,
                color = brand.FlightNumber.Color,
                align = 1
            };
            Console.WriteLine("Flight Number: " + flightNumber.TextString);
            _pixoo.SendCustomText(flightNumber);
      
            if(bareBonesInfo.LeftText != null)
            {
                PixooText depart = new PixooText()
                {
                    x = brand.Departure.OffsetX,
                    y = brand.Departure.OffsetY,
                    dir = 0,
                    font = 4,
                    TextWidth = 64,
                    speed = 10,
                    TextString = bareBonesInfo.LeftText,
                    color = brand.Departure.Color,
                    align = 1
                };
                Console.WriteLine("Left: " + depart.TextString);
                _pixoo.SendCustomText(depart);
               
            }
         
            if(bareBonesInfo.RightText != null)
            {
                PixooText arrival = new PixooText()
                {
                    x = brand.Arrival.OffsetX,
                    y = brand.Arrival.OffsetY,
                    dir = 0,
                    font = 4,
                    TextWidth = 64,
                    speed = 10,
                    TextString = bareBonesInfo.RightText,
                    color = brand.Arrival.Color,
                    align = 1
                };
                Console.WriteLine("Right: " + arrival.TextString);
                _pixoo.SendCustomText(arrival);
                
            }
         
        }

        //This allows you to configure your home airport to always display as the left text on the screen
        private DirectionalInfo ChooseDirection(DisplayInformation di, string image)
        {
            var airportsConfig = _configuration["Flags:HomeAirports"];
            List<string> airportCodes = null;

            if(airportsConfig!=null && airportsConfig != "")
            {
                airportCodes = airportsConfig.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(a => a.Trim(' ', '"')).ToList();
            }
            string destination = RemoveSpecialCharacters(di.Destination);
            string arrival = RemoveSpecialCharacters(di.Origin);

            DirectionalInfo info = new DirectionalInfo();
            if(airportCodes!= null && airportCodes.Contains(destination))
            {
                int dotIndex = image.LastIndexOf('.');
                if (dotIndex != -1)
                {
                    image = image.Substring(0, dotIndex) + "L" + image.Substring(dotIndex);
                    info.Image = image;
                }
                info.LeftText = destination;
                info.RightText = arrival;
            }
            else
            {
                int dotIndex = image.LastIndexOf('.');
                if (dotIndex != -1)
                {
                    image = image.Substring(0, dotIndex) + "R" + image.Substring(dotIndex);
                    info.Image = image;
                }
                info.LeftText = arrival;
                info.RightText = destination;
            }
            return info;
        }

        //
        private void DisplayAltitude(int alt, Altitude brandedAltitude)
        {
                PixooText altitude = new PixooText()
                {
                    x = brandedAltitude.OffsetX,
                    y = brandedAltitude.OffsetY,
                    dir = 0,
                    font = brandedAltitude.Font,
                    TextWidth = 64,
                    speed = 5,
                    TextString = alt.ToString("N0"),
                    color = brandedAltitude.Color,
                    align = 1,
                    TextId = 19
                };
            Console.WriteLine(altitude.TextString);
            _pixoo.SendCustomTextLocked(altitude);
            
        }
        private static string RemoveSpecialCharacters(string str)
        {
            if (str == null)
                return null;
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }

    public class DirectionalInfo
    {
        public string LeftText { get; set; }
        public  string RightText { get; set; }
        public  string Image { get; set; }
    }
}
