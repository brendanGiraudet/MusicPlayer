using MusicPlayerApplication.Services;
using Xunit;

namespace MusicPlayer.FunctionalTest
{
    public class YoutubeDlServiceFunctionalTest
    {
        readonly IYoutubeDlService _youtubeDlService;
        public YoutubeDlServiceFunctionalTest(IYoutubeDlService youtubeDlService)
        {
            _youtubeDlService = youtubeDlService;
        }
        #region DownloadMusicAsync
        [Fact(Skip ="missing youtubedl package")]
        public void ShouldHaveResponseWithHasErrorFalseWhenDownloadMusic()
        {
            // Arrange
            var url = "https://www.youtube.com/watch?v=hjpF8ukSrvk&list=RDCLAK5uy_mfut9V_o1n9nVG_m5yZ3ztCif29AHUffI&index=12";

            // Act
            var downloadMusicResponse = _youtubeDlService.DownloadMusic(url);

            // Arrange
            Assert.False(downloadMusicResponse.HasError);
        }
        #endregion
    }
}
