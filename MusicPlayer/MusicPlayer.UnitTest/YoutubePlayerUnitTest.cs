using Bogus;
using Google.Apis.YouTube.v3;
using Moq;
using MusicPlayerApplication.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.UnitTest
{
    public class YoutubePlayerUnitTest
    {
        readonly IYoutubePlayerViewModel _youtubePlayerViewModel;

        public YoutubePlayerUnitTest()
        {
            var youtubeServiceMock = new Mock<YouTubeService>();
            youtubeServiceMock
                .Setup(s => s.Search.List(It.IsAny<string>()))
                .Returns(FakerUtils.ListRequestFaker.Generate())
                .Verifiable();

            _youtubePlayerViewModel = new YoutubePlayerViewModel(youtubeServiceMock.Object);
        }

        #region SearchAsync
        [Fact(Skip ="mock reason")]
        public async Task ShouldHaveListOfVideosWhenSearch()
        {
            // Arrange
            var faker = new Faker();
            var fakeWordToSearch = faker.Random.String2(2);

            // Act
            var videos = await _youtubePlayerViewModel.SearchAsync(fakeWordToSearch);

            // Arrange
            Assert.True(videos.Any());
        }
        #endregion
    }
}
