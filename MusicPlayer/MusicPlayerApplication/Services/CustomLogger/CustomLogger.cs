using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicPlayerApplication.Dtos;
using MusicPlayerApplication.Settings;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.CustomLogger;

public class CustomLogger : ILogger
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly LogSettings _logSettings;
    private readonly string _categoryName;
    private readonly CustomLoggerProvider _provider;

    public CustomLogger(string categoryName, CustomLoggerProvider provider, IHttpClientFactory httpClientFactory, IOptions<LogSettings> logSettingsOption)
    {
        _categoryName = categoryName;
        _provider = provider;
        _logSettings = logSettingsOption.Value;
        _httpClientFactory = httpClientFactory;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _provider.MinimumLogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message))
            return;

        // Ajoute un préfixe personnalisé au message
        var output = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_provider.Prefix} - {logLevel}] {_categoryName}: {message}";

        if (exception != null)
        {
            output += $"\nException: {exception}";
        }

        Console.WriteLine($"----------");
        Console.WriteLine(output);
        Console.WriteLine($"----------");

        Log(logLevel.ToString(), output);
    }

    public async Task<bool> Log(string level, string message)
    {
        try
        {
            LogDto logDto = CreateLogDto(level, message);
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync(_logSettings.Url, logDto);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    private LogDto CreateLogDto(string level, string message)
    {
        return new LogDto
        {
            Fields = new Fields
            {
                Application = _provider.Prefix,
                Environnement = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                Level = level,
                Message = message
            }
        };
    }
}
