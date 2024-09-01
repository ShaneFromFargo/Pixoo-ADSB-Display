using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ATIS
{
    public class Audio : IAudio
    {
        public Audio() 
        {
            
        }

        public void CaptureStreamingAudio(string url)
        {
            using (var mf = new MediaFoundationReader(url))
            using (var wo = new WaveOutEvent())
            {
                wo.Init(mf);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void CaptureAudioFromStream(string url, string outputPath)
        {
            using (var mfReader = new MediaFoundationReader(url))
            using (var waveWriter = new WaveFileWriter(outputPath, mfReader.WaveFormat))
            {
                // Create a buffer to hold the samples
                byte[] buffer = new byte[1024];
                int bytesRead;
                int totalBytesRead = 0;
                int targetBytes = mfReader.WaveFormat.AverageBytesPerSecond * 60; // 60 seconds of audio

                // Read from the stream for 1 minute or until the stream ends
                while ((bytesRead = mfReader.Read(buffer, 0, buffer.Length)) > 0 && totalBytesRead < targetBytes)
                {
                    waveWriter.Write(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;
                }
            }
        }
    }
}
