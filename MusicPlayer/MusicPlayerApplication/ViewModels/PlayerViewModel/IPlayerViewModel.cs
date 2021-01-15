using MusicPlayerApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels.PlayerViewModel
{
    public interface IPlayerViewModel
    {
        IEnumerable<SongModel> Songs { get; set; }
        int CurrentSongIndex { get; set; }
        SongModel CurrentSong { get; set; }
        bool IsEndList { get; }
        bool IsBeginList { get; }

        Task LoadSongsAsync();
        Task NextSongAsync(bool isRandom);
        Task PreviousSongAsync(bool isRandom);
    }
}
