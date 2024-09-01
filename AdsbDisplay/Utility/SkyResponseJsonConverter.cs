using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AdsbDisplay.ADSB.Models;

public class SkyResponseJsonConverter : JsonConverter<SkyResponse>
    {
           public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }
        public override SkyResponse Read(ref Utf8JsonReader reader,
                                      Type typeToConvert,
                                      JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            SkyResponse value = new SkyResponse();
            while (reader.Read())
            {
                Console.WriteLine($"reader.TokenType:{reader.TokenType}");
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    Console.WriteLine($"End Object!");
                    break;
                }
                int i = 0;
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        {
                            string propertyName = reader.GetString();
                            //Console.WriteLine($"propertyName:{propertyName}");
                            switch (propertyName)
                            {
                                case "alt_baro":
                                    {
                                        reader.Read();
                                        if (reader.TokenType != JsonTokenType.Null)
                                        {
                                            //value.alt_baro = reader.GetString();
                                            //value.ac[i].alt_baro = "hi";
                                        }
                                        break;
                                    }
                                case "ac":
                                    {
                                        reader.Read();
                                        if (reader.TokenType != JsonTokenType.Null)
                                        {
                                            i++;
                                        }
                                        break;
                                    }
                                case "total":
                                    {
                                        reader.Read();
                                        if (reader.TokenType != JsonTokenType.Null)
                                        {
                                            value.total = 8762;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                }
   
            }
            return value;
        }

        public override void Write(Utf8JsonWriter writer,
                                   SkyResponse value,
                                   JsonSerializerOptions options)
        {
            Console.WriteLine($"Writing my due");
            //not implemented
        }
    }

    public class AltBaroConverter : JsonConverter<string>
    {
        public override string Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var currnetValue = jsonDoc.RootElement.GetRawText().ToLower();
                if (currnetValue == "\"ground\"")
                    return "-1";
                return jsonDoc.RootElement.GetRawText();
            }
        }

        public override void Write(
            Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }


