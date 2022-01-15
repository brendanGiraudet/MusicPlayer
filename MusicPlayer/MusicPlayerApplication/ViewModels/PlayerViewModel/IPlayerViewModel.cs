using MusicPlayerApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels.PlayerViewModel
{
    public interface IPlayerViewModel
    {
        IEnumerable<SongModel> Songs { get; set; }
        IEnumerable<SongModel> FilteredSongs { get; set; }
        int CurrentSongIndex { get; set; }
        SongModel CurrentSong { get; set; }
        bool IsEndList { get; }
        bool IsBeginList { get; }

        Task ApplyFilter(string filter);
        Task LoadSongsAsync();
        Task NextSongAsync(bool isRandom);
        Task PreviousSongAsync(bool isRandom);
    }
}
