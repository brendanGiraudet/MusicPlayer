using MusicPlayerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels.PlayerViewModel
{
    public class PlayerViewModel : IPlayerViewModel
    {
        public IEnumerable<SongModel> Songs { get; set; } = Enumerable.Empty<SongModel>();
        public int CurrentSongIndex { get; set; }
        public SongModel CurrentSong { get; set; }

        public async Task LoadSongsAsync()
        {
            var song = new SongModel
            {
                Artist = "Ray Charles",
                Path = "./Musics/test.mp4",
                Title = "Hit the road Jack",
                ImagePath = "./Musics/test.jpg"
            };
            Songs = Songs.Append(song);
            CurrentSong = song;
            await Task.CompletedTask;
        }
    }
}
