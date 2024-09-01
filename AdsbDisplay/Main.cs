using AdsbDisplay.ADSB;
using AdsbDisplay.ADSB.Models;
using AdsbDisplay.ATIS;
using AdsbDisplay.Display.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay
{
    //The main class is the where we bridge the logic from the Pixoo, ADSB, and other sources
    public class Main : IMain
    {
        private readonly CustomLogger<Main> _logger;
        private readonly IADSB _ADSB;
        private readonly IDisplay _display;
        private readonly IPixooTest _test;
        private int _executionCount = 0;
        private bool _aircraft = true;
        private readonly IAudio _audio;
        private readonly IAtis _atis;
        public Main(CustomLogger<Main> logger, IADSB adsb, IDisplay display, IPixooTest test, IAudio audio, IAtis atis)
        {
            _logger = logger;
            _ADSB = adsb;
            _display = display;
            _test = test;
            _audio = audio;
            _atis = atis;
        }
        //Entry point for the application
        public async void Execute()
        {

            //_audio.CaptureStreamingAudio("https://s1-bos.liveatc.net/kads_atis");
            //_audio.CaptureAudioFromStream("https://s1-bos.liveatc.net/kads_atis", "C:\\Hobbies\\kdal.wav");
            //_atis.RecognizeAudioAsync("C:\\Hobbies\\harvard.wav");

            //return;
            //_test.Simulate();
            //return;
            //_logger.Log("In the entry point for the application");
            DisplayInformation display = await _ADSB.GetPlane();


            //Use this to test the display
            //Find any plane in the sky and type in the tail number
            //string testTailNumber = "ENY3497";
            //DisplayInformation display = await _ADSB.Test(testTailNumber);
           // DisplayInformation display = await _ADSB.GetRandomPlane();
            if (display != null)
            {
                _display.Aircraft(display);
                _logger.Log(display.FlightNumber);
                _aircraft = true;
            }
            else
            {
                //Don't hammer the Pixoo with no aircraft
                if(_aircraft)
                {
                    Console.WriteLine("No Aircraft Setting Pixoo Channel");
                    _display.NoAircraft();
                    _aircraft = false;
                    
                }
            }

            _executionCount++;
        }
    }
}
