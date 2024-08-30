using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.ViewModels.SongManagerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.SongManager
{
    public partial class SongManagerComponent
    {
        [Inject] public ISongManagerViewModel ViewModel { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        private IEnumerable<SongModel> FilteredSongs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadSongsAsync();

            await base.OnInitializedAsync();

            await ApplyFilter();
        }

        private async void Remove(string fileName, string title)
        {
            await ViewModel.RemoveAsync(fileName, title);
            StateHasChanged();
        }

        private async Task DownloadSong(string filePath)
        {
            await JSRuntime.InvokeAsync<string>("songmanager.download", filePath);
        }

        private async Task OnFilterChanged(ChangeEventArgs changeEventArgs)
        {
            var value = changeEventArgs.Value?.ToString();

            await ApplyFilter(value);
        }

        private async Task ApplyFilter(string filter = null)
        {
            if (filter == null)
            {
                FilteredSongs = ViewModel.Songs;
                return;
            }

            IEnumerable<SongModel> songs = Array.Empty<SongModel>();

            foreach (var song in ViewModel.Songs)
            {
                if(song.Title.ToLowerInvariant().Contains(filter.ToLowerInvariant()) 
                    || song.Artist.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                {
                    songs = songs.Append(song);
                }
            }

            FilteredSongs = songs;

            await Task.CompletedTask;
        }
    }
}
