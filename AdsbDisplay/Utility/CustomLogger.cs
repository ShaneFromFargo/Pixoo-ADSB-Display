using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class CustomLogger<T>
{
    //Microsofts default logger doesn't support logging to a local file, so I wrapped their library with addition of adding local file logging
    private readonly ILogger<T> _logger;
    private readonly string _logFilePath;
    private readonly IConfiguration _configuration;

    public CustomLogger(ILogger<T> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _logFilePath = configuration["Flags:LogFilePath"];

        if (string.IsNullOrEmpty(_logFilePath))
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt");
        }
        else
        {
            _logFilePath = Path.Combine(_logFilePath, "Log.txt");
        }

    }

    public void Log(string message)
    {
        //Default Microsoft Logger
        _logger.LogInformation($"{DateTime.Now}: {message}");

        try
        {
            // Append text to the log file and allow read sharing
            using (FileStream fileStream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to log file: " + ex.Message);
        }
    }

    public void LogWithTimestamp(string message)
    {
        _logger.LogInformation($"{DateTime.Now}: {message}");
    }
}
