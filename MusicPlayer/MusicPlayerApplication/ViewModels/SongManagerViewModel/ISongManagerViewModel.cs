using MusicPlayerApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels.SongManagerViewModel
{
    public interface ISongManagerViewModel
    {
        IEnumerable<SongModel> Songs { get; set; }

        Task LoadSongsAsync();
        Task RemoveAsync(string name, string title);
    }
}
