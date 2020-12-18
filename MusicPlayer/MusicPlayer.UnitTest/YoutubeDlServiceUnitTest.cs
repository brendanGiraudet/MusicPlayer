using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;
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
                .Setup(s => s.Run(It.IsAny<string>()))
                .Returns(new ResponseModel
                {
                    HasError = false
                })
                .Verifiable();

            _youtubeDlService = new YoutubeDlService(youtubeDlSettingsMock.Object, shellServiceMock.Object);
        }

        #region DownloadMusicAsync
        [Fact]
        public void ShouldHaveResponseWithHasErrorFalseWhenDownloadMusic()
        {
            // Arrange
            var faker = new Faker();
            var fakeUrl = faker.Random.String2(2);

            // Act
            var downloadMusicResponse = _youtubeDlService.DownloadMusic(fakeUrl);

            // Arrange
            Assert.False(downloadMusicResponse.HasError);
        }
        #endregion
    }
}
