using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using MusicPlayerApplication.Services.LogService;
using MusicPlayerApplication.Services.SongService;
using MusicPlayerApplication.Settings;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.UnitTest
{
    public class SongServiceUnitTest
    {
        IOptions<YoutubeDlSettings> _youtubeDlSettingsOptions;
     
        IOptions<SongSettings> _songSettingsOptions;
        
        ILogService DefaultLogService
        {
            get
            {
                var mock = new Mock<ILogService>();

                mock.Setup(s => s.Log(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true))
                    .Verifiable();

                return mock.Object;
            }
        }
        ISongService CreateSongService() => new SongService(_youtubeDlSettingsOptions, _songSettingsOptions, DefaultLogService);

        public SongServiceUnitTest(IOptions<YoutubeDlSettings> youtubeDlSettingsOptions, IOptions<SongSettings> songSettingsOptions)
        {
            _youtubeDlSettingsOptions = youtubeDlSettingsOptions;
            _songSettingsOptions = songSettingsOptions;
        }

        #region GetSongsAsync
        [Fact]
        public async Task ShouldHaveListOfSongWhenGetSongsAsync()
        {
            // Arrange
            var songService = CreateSongService();

            // Act
            var getSongResponse = await songService.GetSongsAsync();

            // Arrange
            Assert.False(getSongResponse.HasError);
            Assert.True(getSongResponse.Content.Any());
        }
        #endregion

        #region RemoveByNameAsync
        [Fact(Skip ="no file service to mock")]
        public async Task ShouldHaveTrueWhenRemoveSongByName()
        {
            // Arrange  
            var faker = new Faker();
            var songFileName = faker.Random.String2(2);
            var songService = CreateSongService();

            // Act 
            var removeSongByNameResponse = await songService.RemoveByNameAsync(songFileName);

            // Assert 
            Assert.True(removeSongByNameResponse.Content);
        }
        #endregion
    }
}
