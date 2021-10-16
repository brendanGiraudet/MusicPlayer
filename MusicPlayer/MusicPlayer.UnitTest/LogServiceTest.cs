using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MusicPlayerApplication.Services.LogService;
using MusicPlayerApplication.Settings;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.UnitTest
{
    public class LogServiceTest
    {
        HttpClient _httpClient;

        IOptions<LogSettings> _logSettingsOptions;

        ILogService CreateLogService() => new LogService(_httpClient, _logSettingsOptions);
        
        public LogServiceTest(IOptions<LogSettings> logSettingsOptions)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            _httpClient = HttpClientFactory.Create(httpClientHandler);
            _logSettingsOptions = logSettingsOptions;
        }

        #region Log
        [Fact]
        public async Task ShouldHaveTrueWhenLog()
        {
            // Arrange
            var type = "Informations";
            var content = "musicplayertest";
            var logService = CreateLogService();

            // Act
            var logResponse = await logService.Log(type, content);

            // Assert
            Assert.True(logResponse);
        }
        #endregion
    }
}
