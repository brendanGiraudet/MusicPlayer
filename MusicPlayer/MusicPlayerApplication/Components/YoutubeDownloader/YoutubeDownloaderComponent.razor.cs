using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.YoutubeDownloader
{
    public partial class YoutubeDownloaderComponent
    {
        [Inject] public IYoutubeDlService YoutubeDlService { get; set; }

        public YoutubeVideoModel Model { get; set; } = new YoutubeVideoModel();

        private async Task DownloadClick(MouseEventArgs mouseEventArgs)
        {
            await YoutubeDlService.DownloadMusicAsync(Model.Url);
        }
    }
}
