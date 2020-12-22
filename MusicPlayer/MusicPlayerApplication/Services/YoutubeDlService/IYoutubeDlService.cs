using MusicPlayerApplication.Models;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services
{
    public interface IYoutubeDlService
    {
        Task<ResponseModel> DownloadMusicAsync(string url);
    }
}
