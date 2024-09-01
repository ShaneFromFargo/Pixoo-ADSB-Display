using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ATIS
{
    public interface IAtis
    {
        Task RecognizeAudioAsync(string audioFilePath);
    }
}
