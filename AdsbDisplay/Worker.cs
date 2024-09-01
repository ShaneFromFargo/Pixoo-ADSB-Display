using AdsbDisplay.Display;
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

            //Copy art work into Bin folder
            CopyArt();

            //Execute first time
            _main.Execute();

            //The core loop of the program
            while (!stoppingToken.IsCancellationRequested)
            {
                //Read values from Configuration, and prepare the program
                int executionLoopDuration = int.Parse(_configuration["Flags:ExecutionLoop"]);
                executionLoopDuration = executionLoopDuration * 1000;
                await Task.Delay(executionLoopDuration, stoppingToken);

                _main.Execute();
            }
        }

        static void CopyArt()
        {
            string sourceDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Art");
            string targetDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Art");

            var sourcePath = Path.GetFullPath(sourceDirectory);
            // Create the target directory if it doesn't exist
            Directory.CreateDirectory(targetDirectory);

            // Copy files and subdirectories recursively
            CopyDirectory(sourceDirectory, targetDirectory);
        }

        static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDirectory);
            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(targetDirectory);

            // Copy files
            foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
            {
                string destinationFilePath = Path.Combine(targetDirectory, file.Name);
                file.CopyTo(destinationFilePath, true);
            }

            // Copy subdirectories recursively
            foreach (DirectoryInfo subDirectory in sourceDirectoryInfo.GetDirectories())
            {
                string subDirectoryPath = Path.Combine(targetDirectory, subDirectory.Name);
                CopyDirectory(subDirectory.FullName, subDirectoryPath);
            }
        }
    }
}