using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ATIS
{
    public interface IAudio
    {
        void CaptureStreamingAudio(string url);
        void CaptureAudioFromStream(string url, string outputPath);
    }
}
