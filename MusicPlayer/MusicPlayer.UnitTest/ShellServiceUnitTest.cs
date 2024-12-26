using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.UnitTest
{
    public class ShellServiceUnitTest
    {
        

        IOptions<ShellSettings> _shellSettingsOptions;

        ILogger<ShellService> DefaultLogService
        {
            get
            {
                var mock = new Mock<ILogger<ShellService>>();

                return mock.Object;
            }
        }

        IShellService CreateShellService() => new ShellService(_shellSettingsOptions, DefaultLogService);

        public ShellServiceUnitTest(IOptions<ShellSettings> shellSettingsOptions)
        {
            _shellSettingsOptions = shellSettingsOptions;
        }

        #region Run
        [Fact]
        public async Task ShouldHaveRunResponseWithHasErrorFalseWhenRunShellCommand()
        {
            // Arrange
            var fakeCommand = "echo hello";
            var shellService = CreateShellService();

            // Act
            var runResponse = await shellService.RunAsync(fakeCommand);

            // Arrange
            Assert.False(runResponse.HasError);
        }
        #endregion
    }
}
