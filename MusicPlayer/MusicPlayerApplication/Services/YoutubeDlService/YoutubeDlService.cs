using Microsoft.Extensions.Options;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services
{
    public class YoutubeDlService : IYoutubeDlService
    {
        readonly IOptions<YoutubeDlSettings> _youtubeDlSettings;
        readonly IShellService _shellService;
        private string[] _musicFormats = { "mp3", "wav" };
        public YoutubeDlService(IOptions<YoutubeDlSettings> youtubeDlSettings,
            IShellService shellService)
        {
            _youtubeDlSettings = youtubeDlSettings;
            _shellService = shellService;
        }
        public async Task<ResponseModel<bool>> DownloadMusicAsync(string url)
        {
            var command = $"yt-dlp -o '{_youtubeDlSettings.Value.MusicPath}/%(id)s.%(ext)s' --write-info-json -x --audio-format {_musicFormats.First()} --write-thumbnail '{url}'";

            return await _shellService.RunAsync(command);
        }
    }
}
