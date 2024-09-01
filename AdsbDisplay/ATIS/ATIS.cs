using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ATIS
{
    public class ATIS : IAtis
    {
        public ATIS() 
        {
            
        }

        public async Task RecognizeAudioAsync(string audioFilePath)
        {
            var config = SpeechConfig.FromSubscription("e2f3a50ef0ad4557872bf952286673bb", "eastus");
            using (var audioInput = AudioConfig.FromWavFileInput(audioFilePath))
            using (var recognizer = new SpeechRecognizer(config, audioInput))
            {
                Console.WriteLine("Transcribing audio...");
                var result = await recognizer.RecognizeOnceAsync();
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"Transcription: {result.Text}");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine("No speech could be recognized, or the audio was not clear enough.");
                }
            }
        }
    }
}
