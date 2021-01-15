using Microsoft.Extensions.Options;
using MusicPlayerApplication.Services.SongService;
using MusicPlayerApplication.Settings;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicPlayer.UnitTest
{
    public class SongServiceUnitTest
    {
        readonly ISongService _songService;

        public SongServiceUnitTest(IOptions<YoutubeDlSettings> youtubeDlSettings, IOptions<SongSettings> songSettings)
        {
            _songService = new SongService(youtubeDlSettings, songSettings);
        }

        #region GetSongsAsync
        [Fact]
        public async Task ShouldHaveListOfSongWhenGetSongsAsync()
        {
            // Arrange

            // Act
            var getSongResponse = await _songService.GetSongsAsync();

            // Arrange
            Assert.False(getSongResponse.HasError);
            Assert.True(getSongResponse.Content.Any());
        }
        #endregion
    }
}
