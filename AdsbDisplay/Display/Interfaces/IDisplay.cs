using AdsbDisplay.ADSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.Display.Interfaces
{
    public interface IDisplay
    {
        void Aircraft(DisplayInformation di);
        void NoAircraft();
    }
}
