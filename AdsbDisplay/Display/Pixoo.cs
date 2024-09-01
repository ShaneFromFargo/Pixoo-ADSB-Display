using AdsbDisplay.Display.Interfaces;
using AdsbDisplay.Models.Pixoo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AdsbDisplay.Display
{
    public class Pixoo : IPixoo
    {
        private string _ipAddress;
        //Pixoo needs to send an image count, so must restart pixoo before running this app
        private int _imageCount;
        //Text will overwrite if this isn't incremented. We don't want this, instead we use the clear text method when clearing text.
        private int _textCount = 1;
        private int _picId = 1;

        private bool _locked; //Locks the Pixoo from sending messages. To prevent threading of sending multiple messages at once
        private bool _found = false; //Don't send commands to Pixoo until we have found it
        private readonly CustomLogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;


        //Official Pixo Documentation
        //http://doc.divoom-gz.com/web/#/12?page_id=336
        //https://docin.divoom-gz.com/web/#/5/60
        public Pixoo(CustomLogger<Worker> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
            _configuration = configuration;
            _client.Timeout = TimeSpan.FromSeconds(4);
            //Reboot();
            ResetPicId();
            ClearText();
           
        }
        public void SendCustomText(PixooText pt)
        {
            //Textid is a rotating list of 20 codes. Stopping at 17 , because 18 and 19 are reserved for animations
            if (_textCount > 16)
                _textCount = 1;
            Console.WriteLine("Text Count: " + _textCount);
            pt.Command = "Draw/SendHttpText";
            pt.TextId = _textCount;
            var pixooJson = JsonConvert.SerializeObject(pt);
            SendRequest(pixooJson);
            _textCount++;
        }

        public void SendCustomTextExpirement(PixooText pt)
        {
            //Textid is a rotating list of 20 codes. Stopping at 17 , because 18 and 19 are reserved for animations
            if (_textCount > 16)
                _textCount = 1;
            Console.WriteLine("Text Count: " + _textCount);
            pt.Command = "Draw/SendHttpText";
            var pixooJson = JsonConvert.SerializeObject(pt);
            SendRequest(pixooJson);
            _textCount++;
        }

        public void SendCustomTextLocked(PixooText pt)
        {
            //Not tracking the text count, so we can update the specific text by id
            pt.Command = "Draw/SendHttpText";
            var pixooJson = JsonConvert.SerializeObject(pt);
            SendRequest(pixooJson);
        }
        public void SetChannel(int channel)
        {
            PixooChannel pc = new PixooChannel()
            {
                Command = "Channel/SetIndex",
                SelectIndex = channel.ToString()
            };
            var pixooJson = JsonConvert.SerializeObject(pc);
            SendRequest(pixooJson);
        }
        public void SendText(string text)
        {
            if (_textCount > 16)
                _textCount = 1;
            PixooText pt = new PixooText()
            {
                Command = "Draw/SendHttpText",
                TextId = _textCount,
                x = 20,
                y = 20,
                dir = 0,
                font = 4,
                TextWidth = 56,
                speed = 10,
                TextString = text,
                color = "#F9B612",
                align = 1
            };
            _textCount++;
            var pixooJson = JsonConvert.SerializeObject(pt);
            SendRequest(pixooJson);
        }

        //Having issues with the Pixoo not rendering text.
        //Using this for expirementing and testing
        public void SendTextExpirement(string text)
        {
            PixooText pt = new PixooText()
            {
                Command = "Draw/SendHttpText",
                x = 20,
                y = 20,
                dir = 0,
                font = 4,
                TextWidth = 56,
                speed = 10,
                TextString = text,
                color = "#F9B612",
                align = 1
            };
            var pixooJson = JsonConvert.SerializeObject(pt);
            SendRequest(pixooJson);
        }

        public void ClearText()
        {
            PixooText pt = new PixooText()
            {
                Command = "Draw/ClearHttpText",
            };
            var pixooJson = JsonConvert.SerializeObject(pt);
            SendRequest(pixooJson);
            _textCount = 1;
        }

        public void SendBuzzer()
        {
            PixooBuzzer pb = new PixooBuzzer()
            {
                Command = "Device/PlayBuzzer",
                ActiveTimeInCycle = 500,
                OffTimeInCycle = 500,
                PlayTotalTime = 2000,
            };
            var pixooJson = JsonConvert.SerializeObject(pb);
            SendRequest(pixooJson);
        }

        public void SendPic(string picture)
        {
            _picId++;
            if(_picId > 20)
                ResetPicId();
            Bitmap pix64 = ResizeImageTo64x64(picture);
            string encoded = GetBase64EncodedRGBData(pix64);
            Picture pic = new Picture()
            {
                Command = "Draw/SendHttpGif",
                PicNum = 1,
                PicWidth = 64,
                PicOffset = 0,
                PicID = _picId,
                PicSpeed = 1000,
                PicData = encoded
            };
            var pixooJson = JsonConvert.SerializeObject(pic);
            Console.WriteLine("Pic Count: " + _picId);
            SendRequest(pixooJson);
        }

        private void ResetPicId()
        {
            PixooReset pr = new PixooReset()
            {
                Command = "Draw/ResetHttpGifId",
            };
            var pixooJson = JsonConvert.SerializeObject(pr);
            SendRequest(pixooJson);
            _picId = 1;
        }

        private Bitmap ResizeImageTo64x64(string imagePath)
        {
            using (Bitmap originalImage = new Bitmap(imagePath))
            {
                int newWidth = 64;
                int newHeight = 64;
                return (Bitmap)originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            }
        }

        private string GetBase64EncodedRGBData(Bitmap image)
        {

            int width = image.Width;
            int height = image.Height;
            byte[] rgbData = new byte[width * height * 3];

            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    System.Drawing.Color pixel = image.GetPixel(x, y);
                    rgbData[index++] = pixel.R;
                    rgbData[index++] = pixel.G;
                    rgbData[index++] = pixel.B;
                }
            }

            return Convert.ToBase64String(rgbData);

        }

        public async Task<string> FindPixoo()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://app.divoom-gz.com/Device/ReturnSameLANDevice");
            request.Headers.Add("accept", "application/json");
            try
            {
                HttpResponseMessage response = client.Send(request);
                response.EnsureSuccessStatusCode();
                string body = await response.Content.ReadAsStringAsync();
                var devices = JsonSerializer.Deserialize<PixoResponse>(body);
                if (devices != null)
                    if (devices.DeviceList != null && devices.DeviceList.Count > 0)
                    {
                        _ipAddress = devices.DeviceList[0].DevicePrivateIP;
                        return _ipAddress;
                    }

            }
            catch (Exception e)
            {
                _logger.Log("Did not find Pixoo on Local Network");
                Console.WriteLine(e.Message);
            }
            _logger.Log("Did not find Pixoo on Local Network");
            return "error";

        }

        private void SendRequest(string json)
        {
            if(_locked)
            {
                _logger.Log("Pixoo is locked, cannot send request");
                return;
            }
            _locked = true;
            //On first request, find the Pixoo
            if (_ipAddress == null)
            {
                FindPixoo();
            }
          
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"http://{ _ipAddress }:80/post");
            request.Headers.Add("accept", "*/*");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Connection", "keep-alive");

            request.Content = content;
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                HttpResponseMessage response = _client.Send(request);
                Console.WriteLine("Sent Request to Pixoo: "+response.StatusCode);
            }
            catch (Exception e)
            {

                _logger.LogError("Unable to send request to Pixoo: " + e.Message, "Pixoo.cs | Line 227");
                _logger.LogError("JSON: " + json, "Pixoo.cs | Line 228");
                _logger.Log($"{e.Message}");
            }
            _locked = false;
        }

        private void Reboot()
        {
            PixooReset pr = new PixooReset()
            {
                Command = "Device/SysReboot",
            };
            var pixooJson = JsonConvert.SerializeObject(pr);
            SendRequest(pixooJson);
        }
    }
}
