using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.Models.Pixoo
{
    public class Fill
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public bool push_immediately { get; set; }
    }

    public class Text
    {
        public string text { get; set; }
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool push_immediately { get; set; }
    }

    public class Picture
    {
        public string Command { get; set; }
        public int PicNum { get; set; }
        public int PicWidth { get; set; }
        public int PicOffset { get; set; }
        public int PicID { get; set; }
        public int PicSpeed { get; set; }
        public string PicData { get; set; }
    }

    public class PixooBuzzer
    {
        public string Command { get; set; }
        public int ActiveTimeInCycle { get; set; }
        public int OffTimeInCycle { get; set; }
        public int PlayTotalTime { get; set; }
    }

    public class PixooReset
    {
        public string Command { get; set; }
    }

    public class PixooText
    {
        public string Command { get; set; }
        public int TextId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int dir { get; set; }
        public int font { get; set; }
        public int TextWidth { get; set; }
        public int speed { get; set; }
        public string TextString { get; set; }
        public string color { get; set; }
        public int align { get; set; }
    }

    public class PixooChannel
    {
        public string Command { get; set; }
        public string SelectIndex { get; set; }
    }

    public enum Actions
    {
        Fill,
        Draw,
        Text,
        sendGif
    }

    public class DeviceList
    {
        public string DeviceName { get; set; }
        public int DeviceId { get; set; }
        public string DevicePrivateIP { get; set; }
        public string DeviceMac { get; set; }
    }

    public class PixoResponse
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public List<DeviceList> DeviceList { get; set; }
    }

    public class OnlineGif
    {
        public string Command { get; set; }
        public int FileType { get; set; }
        public string FileName { get; set; }
    }

    public class ItemList
    {
        public int TextId { get; set; }
        public int type { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int dir { get; set; }
        public int font { get; set; }
        public int TextWidth { get; set; }
        public int Textheight { get; set; }
        public int speed { get; set; }
        public int align { get; set; }
        public string color { get; set; }
        public string TextString { get; set; }
        public int? update_time { get; set; }
    }

    public class ItemCommand
    {
        public string Command { get; set; }
        public List<ItemList> ItemList { get; set; }
    }

}
