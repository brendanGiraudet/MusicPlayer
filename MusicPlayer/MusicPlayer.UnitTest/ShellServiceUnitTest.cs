using Microsoft.Extensions.Options;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.UnitTest
{
    public class ShellServiceUnitTest
    {
        readonly IShellService _shellService;

        public ShellServiceUnitTest(IOptions<ShellSettings> shellSettings)
        {
            _shellService = new ShellService(shellSettings);
        }

        #region Run
        [Fact]
        public async Task ShouldHaveRunResponseWithHasErrorFalseWhenRunShellCommand()
        {
            // Arrange
            var fakeCommand = "echo hello";

            // Act
            var runResponse = await _shellService.RunAsync(fakeCommand);

            // Arrange
            Assert.False(runResponse.HasError);
        }
        #endregion
    }
}
