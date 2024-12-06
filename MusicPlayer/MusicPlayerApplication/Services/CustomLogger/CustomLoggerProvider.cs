using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicPlayerApplication.Settings;

namespace MusicPlayerApplication.Services.CustomLogger;

[ProviderAlias("CustomLogger")]
public class CustomLoggerProvider : ILoggerProvider
{
    public string Prefix { get; }
    public LogLevel MinimumLogLevel { get; }

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<LogSettings> _logSettingsOption;


    public CustomLoggerProvider(IOptionsMonitor<CustomLoggerSettings> config, IHttpClientFactory httpClientFactory, IOptions<LogSettings> logSettingsOption)
    {
        Prefix = config.CurrentValue.Prefix;
        MinimumLogLevel = config.CurrentValue.LogLevel;
        _httpClientFactory = httpClientFactory;
        _logSettingsOption = logSettingsOption;
    }

    public ILogger CreateLogger(string categoryName) => new CustomLogger(categoryName, this, _httpClientFactory, _logSettingsOption);

    public void Dispose() { }
}