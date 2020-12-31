using MusicPlayerApplication.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels
{
    public interface IYoutubeDlViewModel
    {
        YoutubeVideoModel Model { get; set; }
        Task DownloadMusicAsync();
        event PropertyChangedEventHandler PropertyChanged;
    }
}
