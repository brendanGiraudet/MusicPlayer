using Microsoft.Extensions.Logging;

namespace MusicPlayerApplication.Settings;

public class CustomLoggerSettings
{
    public int EventId { get; set; }

    public LogLevel LogLevel { get; set; }
    
    public string Prefix { get; set; }
}
