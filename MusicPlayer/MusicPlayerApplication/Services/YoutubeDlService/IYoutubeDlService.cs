using MusicPlayerApplication.Models;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services
{
    public interface IYoutubeDlService
    {
        Task<ResponseModel<bool>> DownloadMusicAsync(string url);
    }
}
