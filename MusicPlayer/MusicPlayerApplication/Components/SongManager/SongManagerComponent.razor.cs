using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Stores;
using MusicPlayerApplication.Stores.Actions;
using MusicPlayerApplication.ViewModels.SongManagerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.SongManager
{
    public partial class SongManagerComponent
    {
        [Inject] public IState<MusicsState> MusicsState { get; set; }
        [Inject] public ISongManagerViewModel ViewModel { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public IDispatcher Dispatcher { get; set; }

        private IEnumerable<SongModel> FilteredSongs { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (MusicsState.Value.Songs.Count() == 0)
                Dispatcher.Dispatch(new GetSongsAction());
        }

        private async void Remove(string fileName, string title)
        {
            Dispatcher.Dispatch(new RemoveSongAction(fileName, title));
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
            Dispatcher.Dispatch(new SearchSongAction(filter));
        }
    }
}
