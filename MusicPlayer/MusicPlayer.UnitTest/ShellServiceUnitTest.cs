using Microsoft.Extensions.Options;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;
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
        public void ShouldHaveRunResponseWithHasErrorFalseWhenRunShellCommand()
        {
            // Arrange
            var fakeCommand = "echo hello";

            // Act
            var runResponse = _shellService.Run(fakeCommand);

            // Arrange
            Assert.False(runResponse.HasError);
        }
        #endregion
    }
}
