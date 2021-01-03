﻿using MusicPlayerApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels.PlayerViewModel
{
    public interface IPlayerViewModel
    {
        IEnumerable<SongModel> Songs { get; set; }
        int CurrentSongIndex { get; set; }
        SongModel CurrentSong { get; set; }

        Task LoadSongsAsync();
    }
}
