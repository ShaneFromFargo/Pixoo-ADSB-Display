using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay
{
    public class Main : IMain
    {
        private readonly CustomLogger<Main> _logger;
        private readonly IConfiguration _configuration;
        public Main(CustomLogger<Main> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async void Execute()
        {
            _logger.Log("In the main method of the program");
            _logger.Log("This line will be repeated every time in the execution loop");
        }

      }
}
