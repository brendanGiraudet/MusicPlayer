using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels
{
    public interface IYoutubePlayerViewModel
    {
        Task<List<string>> SearchAsync(string wordToSearch);
    }
}