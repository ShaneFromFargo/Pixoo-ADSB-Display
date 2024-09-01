using AdsbDisplay.Models.Pixoo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.Display.Interfaces
{
    public interface IPixoo
    {
        void SendCustomText(PixooText pt);
        void SetChannel(int channel);
        void SendText(string text);
        void SendTextExpirement(string text);
        void SendCustomTextExpirement(PixooText pt);
        void ClearText();
        void SendPic(string picture);
        void SendBuzzer();
        Task<string> FindPixoo();
        public void SendCustomTextLocked(PixooText pt);
    }
}
