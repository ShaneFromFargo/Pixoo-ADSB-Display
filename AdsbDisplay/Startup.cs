using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay
{
    public class Startup
    {
        public static async Task<Config> Start()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            Config config = new Config();
            string feederSource = configuration["Flags:FeederSource"];
            if(feederSource != null && feederSource.ToLower()=="local")
            {
                string feederIp = configuration["Flags:LocalFeederIp"];
                string ip = await FindLocalADSBFeeder(feederIp);
                config.LocalFeederIP = ip;
            }
            return config;
           
        }

        private static async Task<string> FindLocalADSBFeeder(string ip)
        {


            HttpClient client = new HttpClient();
            string url = $"http://{ip}/tar1090"; // Currently only supports tar1090. If you have another local feeder protocol, you will need to change this.
            client.Timeout = TimeSpan.FromSeconds(1);
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode) // If status code is 200-299
                {
                    return ip;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Local Feeder not found at: " + ip + ". Scanning network...");
                // Specific IP not found or not listening on this port
            }
            HttpClient client2 = new HttpClient();
            client2.Timeout = TimeSpan.FromSeconds(.1);
            for (int i = 1; i <= 255; i++)
            {

                string subnet = "192.168.68.";
                string ipAddress = subnet + i.ToString();
                string urlScan = $"http://{ipAddress}/tar1090"; // Currently only supports tar1090. If you have another local feeder protocol, you will need to change this.
                Console.WriteLine(ipAddress);
                try
                {
                    HttpResponseMessage response = await client2.GetAsync(urlScan);
                    if (response.IsSuccessStatusCode) // If status code is 200-299
                    {
                        Console.WriteLine("Device found: " + urlScan);
                        Console.WriteLine("Local Feeder Found. Consider updating your feeder IP address in the app config for faster startup. IP: " + ipAddress);
                        return ipAddress;
                    }
                }
                catch (Exception e)
                {
                    // Device not found or not listening on this port
                }
            }

            return null;
        }
    }
    public class Config
    {
        public string LocalFeederIP { get; set; }
        public string PixooIp { get; set; }
    }
}
