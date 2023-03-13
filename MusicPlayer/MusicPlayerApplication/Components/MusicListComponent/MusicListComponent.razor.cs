using Microsoft.AspNetCore.Components;
using MusicPlayerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.MusicListComponent
{
    public partial class MusicListComponent
    {
        [Parameter] public IEnumerable<SongModel> Songs { get; set; }
        [Parameter] public SongModel CurrentSong { get; set; }
        [Parameter] public EventCallback<SongModel> OnClickCallback { get; set; }
        [Parameter] public bool IsDisplay { get; set; }

        private string CssDisplay => IsDisplay ? string.Empty : "hidden";
        private IEnumerable<SongModel> FilteredSongs { get; set; }
        private bool IsCurrentSong(string title) => CurrentSong.Title == title;
        private string PlayedIcon(string title) => IsCurrentSong(title) ? "equalizer" : "play_arrow";
        private string PlayedSongCss(string title) => IsCurrentSong(title) ? "song-played" : string.Empty;

        private async Task OnClickSongLine(SongModel song)
        {
            if (OnClickCallback.HasDelegate) await OnClickCallback.InvokeAsync(song);
        }

        private async Task OnFilterChanged(ChangeEventArgs changeEventArgs)
        {
            var value = changeEventArgs.Value?.ToString();

            if (value != null) await ApplyFilter(value);
        }

        private async Task ApplyFilter(string filter)
        {
            if (filter == null)
            {
                FilteredSongs = Songs;
                return;
            }

            var songs = Songs.Where(s => (s.Title != null && s.Title.ToLowerInvariant().Contains(filter.ToLowerInvariant())) || (s.Artist != null && s.Artist.ToLowerInvariant().Contains(filter.ToLowerInvariant())));
            FilteredSongs = songs.Any() ? songs : Array.Empty<SongModel>();
            await Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            FilteredSongs = Songs;
        }
    }
}
