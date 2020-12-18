using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services;

namespace MusicPlayerApplication.Components.YoutubeDownloader
{
    public partial class YoutubeDownloaderComponent
    {
        [Inject] public IYoutubeDlService YoutubeDlService { get; set; }

        public YoutubeVideoModel Model { get; set; } = new YoutubeVideoModel();

        private void DownloadClick(MouseEventArgs mouseEventArgs)
        {
            var response = YoutubeDlService.DownloadMusic(Model.Url);
        }
    }
}
