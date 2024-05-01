using Microsoft.Extensions.Configuration;

namespace AdsbDisplay
{
    public class Worker : BackgroundService
    {
        private readonly CustomLogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMain _main;

        public Worker(CustomLogger<Worker> logger, IConfiguration configuration, IMain main)
        {
            _logger = logger;
            _configuration = configuration;
            _main = main;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Log("Program Has Started");
            //The core loop of the program
            while (!stoppingToken.IsCancellationRequested)
            {
                //Read values from Configuration, and prepare the program
                int executionLoopDuration = int.Parse(_configuration["Flags:ExecutionLoop"]);
                executionLoopDuration = executionLoopDuration * 1000;

                _main.Execute();
                await Task.Delay(executionLoopDuration, stoppingToken);
            }
        }
    }
}