using MusicPlayerApplication.Models;

namespace MusicPlayerApplication.Services
{
    public interface IYoutubeDlService
    {
        ResponseModel DownloadMusic(string url);
    }
}
