using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.UnitTest
{
    public class YoutubeDlServiceUnitTest
    {
        readonly IYoutubeDlService _youtubeDlService;

        public YoutubeDlServiceUnitTest()
        {
            var youtubeDlSettingsMock = new Mock<IOptions<YoutubeDlSettings>>();
            youtubeDlSettingsMock
                .Setup(s => s.Value)
                .Returns(FakerUtils.YoutubeDlSettingsFaker.Generate())
                .Verifiable();

            var shellServiceMock = new Mock<IShellService>();
            shellServiceMock
                .Setup(s => s.RunAsync(It.IsAny<string>()))
                .ReturnsAsync(new ResponseModel<bool>
                {
                    HasError = false
                })
                .Verifiable();

            _youtubeDlService = new YoutubeDlService(youtubeDlSettingsMock.Object, shellServiceMock.Object);
        }

        #region DownloadMusicAsync
        [Fact]
        public async Task ShouldHaveResponseWithHasErrorFalseWhenDownloadMusic()
        {
            // Arrange
            var faker = new Faker();
            var fakeUrl = faker.Random.String2(2);

            // Act
            var downloadMusicResponse = await _youtubeDlService.DownloadMusicAsync(fakeUrl);

            // Arrange
            Assert.False(downloadMusicResponse.HasError);
        }
        #endregion
    }
}
