using AdsbDisplay.ADSB;
using AdsbDisplay.ADSB.Models;
using AdsbDisplay.Display.Interfaces;
using AdsbDisplay.Models.Pixoo;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.Display
{
    //The Pixoo is not consitent with how it handles API calls. 
    //This class is used for experimenting and figuring out how the Pixoo works.
    public class PixooTest : IPixooTest
    {
        private readonly IPixoo _pixoo;
        private string _lastFlightNumber;
        private readonly IConfiguration _configuration;
        private readonly IADSB _ADSB;
        private readonly IDisplay _display;
        private int _count = 0;
        public PixooTest(IPixoo pixoo, IConfiguration configuration, IADSB aDSB, IDisplay display)
        {
            _pixoo = pixoo;
            _configuration = configuration;
            _ADSB = aDSB;
            _display = display;
        }

        public void Count()
        {
            SendRandoPic();
            _pixoo.SendText(_count.ToString());
            //if (_count == 0)
            //{
            //    FirstTime();
            //}

            //if(_count/10 % 2 == 0)
            //{
            //    _pixoo.SetChannel(1);
            //}
            //else
            //{
            //    SendRandoPic();
            //    _pixoo.ClearText();
             
            //    SendRandoText();
            //}
  
            _count++;
        }

        //For some reason running in this simulation method causes Pixoo to work much more stable.
        //Not sure why, maybe some async thing I'm not understanding.
        public async void Simulate()
        {
            DisplayInformation display = await _ADSB.GetPlane();
            //string testTailNumber = "N92CN";
            //DisplayInformation display = await _ADSB.Test(testTailNumber);
            //DisplayInformation display = await _ADSB.GetRandomPlane();
            if (display!=null)
            {
                Console.WriteLine(display.FlightNumber);
               
                _display.Aircraft(display);
            }
            else
            {
                _display.NoAircraft();
                //_pixoo.SetChannel(1);
            }
            _count++;
        }

        public void SendProblemText()
        {
            PixooText flightNumber = new PixooText()
            {
                x = 4,
                y = 4,
                dir = 0,
                font = 4,
                TextWidth = 56,
                speed = 10,
                TextString = "AAL2597",
                color = "#C01933",
                align = 1
            };
            _pixoo.SendCustomText(flightNumber);
        }
        public void SendRandoPic()
        {
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;

            int index = _count % 8;
            if (index == 0) index = 8;  // Adjust for zero remainder which should map to the 8th case.
            string imageName = "DeltaL";
            switch (index)
            {
                case 1:
                    imageName = "DeltaL";
                    break;
                case 2:
                    imageName = "SWL";
                    break;
                case 3:
                    imageName = "SWR";
                    break;
                case 4:
                    imageName = "DefaultR";
                    break;
                case 5:
                    imageName = "AA";
                    break;
                case 6:
                    imageName = "AAL";
                    break;
                case 7:
                    imageName = "AAR";
                    break;
                case 8:
                    imageName = "SW";
                    break;
                default:
                    imageName = "DeltaL";
                    break;
            }
            string image = rootDirectory + "Art\\"+imageName+".png";
            _pixoo.SendPic(image);
        }

        private bool CheckValue(int value)
        {
            return (value / 10) % 2 == 0;
        }

        private void SendRandoText()
        {
            string randomString = GenerateRandomString(new Random().Next(3, 11));
            PixooText flightNumber = new PixooText()
            {
                x = 38,
                y = 38,
                dir = 0,
                font = 4,
                TextWidth = 56,
                speed = 10,
                TextString = randomString,
                color = "#C01933",
                align = 1
            };
            _pixoo.SendCustomText(flightNumber);
        }


        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void FirstTime()
        {

            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string image = rootDirectory + "Art\\Delta.png";
            _pixoo.SendPic(image);
        }
    }
}
