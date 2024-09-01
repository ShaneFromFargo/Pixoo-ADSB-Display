using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class CustomLogger<T>
{
    //Microsofts default logger doesn't support logging to a local file, so I wrapped their library with addition of adding local file logging
    private readonly ILogger<T> _logger;
    private readonly string _logFilePath;
    private readonly string _errorLogFilePath;
    private readonly IConfiguration _configuration;

    public CustomLogger(ILogger<T> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _logFilePath = configuration["Flags:LogFilePath"]+"Log.txt";
        _errorLogFilePath = configuration["Flags:LogFilePath"] + "ErrorLog.txt";

        if (string.IsNullOrEmpty(_logFilePath))
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt");
            _errorLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog.txt");
        }
        
    }

    public void Log(string message)
    {
        //Default Microsoft Logger
        _logger.LogInformation($"{DateTime.Now}: {message}");

        try
        {
            //Custom logging to local file
            using StreamWriter writer = new StreamWriter(_logFilePath, append: true);
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
  
    }

    public void LogError(string message, string location)
    {
        //Default Microsoft Logger
        _logger.LogInformation($"{DateTime.Now}: {message}");

        try
        {
            //Custom logging to local file
            using StreamWriter writer = new StreamWriter(_errorLogFilePath, append: true);
            writer.WriteLine($"{DateTime.Now}: ERROR! {message} | Location: {location}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }

    }

    public void LogWithTimestamp(string message)
    {
        _logger.LogInformation($"{DateTime.Now}: {message}");
    }
}
