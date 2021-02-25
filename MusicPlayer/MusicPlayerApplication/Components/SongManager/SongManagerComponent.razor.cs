﻿using Microsoft.AspNetCore.Components;
using MusicPlayerApplication.ViewModels.SongManagerViewModel;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.SongManager
{
    public partial class SongManagerComponent
    {
        [Inject] public ISongManagerViewModel ViewModel { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadSongsAsync();

            await base.OnInitializedAsync();
        }

        private async void Remove(string fileName, string title)
        {
            await ViewModel.RemoveAsync(fileName, title);
            StateHasChanged();
        }
    }
}
