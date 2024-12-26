using Fluxor;
using Microsoft.AspNetCore.Components;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Stores;
using MusicPlayerApplication.Stores.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.MusicListComponent
{
    public partial class MusicListComponent
    {
        [Inject] public IState<MusicsState> MusicsState { get; set; }
        [Inject] public IDispatcher Dispatcher { get; set; }
        [Parameter] public EventCallback<SongModel> OnClickCallback { get; set; }
        [Parameter] public bool IsDisplay { get; set; }

        private IEnumerable<SongModel> FilteredSongs { get; set; }
        private bool IsCurrentSong(string title) => MusicsState.Value.CurrentSong.Title == title;

        private async Task OnClickSongLine(SongModel song)
        {
            Dispatcher.Dispatch(new SetCurrentSongAction(song));
            
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
                FilteredSongs = MusicsState.Value.Songs;
                return;
            }

            var songs = MusicsState.Value.Songs.Where(s => (s.Title != null && s.Title.ToLowerInvariant().Contains(filter.ToLowerInvariant())) || (s.Artist != null && s.Artist.ToLowerInvariant().Contains(filter.ToLowerInvariant())));
            FilteredSongs = songs.Any() ? songs : Array.Empty<SongModel>();
            await Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            FilteredSongs = MusicsState.Value.Songs;
        }

        private string GetColorTextStyle(string title)
        {
            var color = IsCurrentSong(title) ? Radzen.Colors.Primary : Radzen.Colors.Secondary;

            var style = $"color : {color}";

            return style;
        }

        private string GetBackgroundColorStyle(string title)
        {
            var color = IsCurrentSong(title) ? Radzen.Colors.Warning : Radzen.Colors.Primary;

            var style = $"background-color : {color}";

            return style;
        }
    }
}
