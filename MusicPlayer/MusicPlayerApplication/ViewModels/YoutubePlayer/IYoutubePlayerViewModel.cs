using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels
{
    public interface IYoutubePlayerViewModel
    {
        List<string> SearchedVideos { get; set; }

        Task SearchAsync(string wordToSearch);
    }
}