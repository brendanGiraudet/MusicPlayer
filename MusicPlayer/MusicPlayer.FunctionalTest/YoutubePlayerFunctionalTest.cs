using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MusicPlayerApplication.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.FunctionalTest
{
    public class YoutubePlayerFunctionalTest
    {
        readonly IYoutubePlayerViewModel _youtubePlayer;
        public YoutubePlayerFunctionalTest()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyB49jzTKoKpANQTekgi4LUnfwlBXYtqYzg",
                ApplicationName = "MusicPlayer"
            });
            _youtubePlayer = new YoutubePlayerViewModel(youtubeService);
        }
        #region SearchAsync
        [Fact]
        public async Task ShouldHaveListOfVideosWhenSearch()
        {
            // Arrange
            var wordToSearch = "i believe i can fly";

            // Act
            var videos = await _youtubePlayer.SearchAsync(wordToSearch);

            // Arrange
            Assert.True(videos.Any());
        }
        #endregion
    }
}
