using Microsoft.Extensions.Options;
using MusicPlayerApplication.Dtos;
using MusicPlayerApplication.Settings;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.LogService
{
    public class LogService : ILogService
    {
        readonly HttpClient _httpClient;
        readonly LogSettings _logSettings;
        const string _applicationName = "music_player";
        public LogService(HttpClient httpClient, IOptions<LogSettings> logSettingsOption)
        {
            _httpClient = httpClient;
            _logSettings = logSettingsOption.Value;
        }

        public async Task<bool> Log(string level, string message)
        {
            try
            {
                LogDto logDto = CreateLogDto(level, message);
                var response = await _httpClient.PostAsJsonAsync(_logSettings.Url, logDto);

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
                    Application = _applicationName,
                    Environnement = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    Level = level,
                    Message = message
                }
            };
        }
    }
}
