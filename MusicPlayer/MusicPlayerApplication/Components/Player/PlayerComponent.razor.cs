﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MusicPlayerApplication.ViewModels.PlayerViewModel;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Player
{
    public partial class PlayerComponent
    {
        private bool _isPlaying = false;

        [Inject] public IPlayerViewModel ViewModel { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        public string IconPlayerClass => _isPlaying ? "fa-pause" : "fa-play";
        public string IsPlayingAttribute => _isPlaying ? "autoplay" : "muted";

        public void TooglePlay() => _isPlaying = !_isPlaying;

        private async Task OnClickPlayPauseButton()
        {
            if (_isPlaying) await Pause();
            else await Play();
            TooglePlay();
        }
        private async Task OnClickNextButton()
        {
            await Stop();
            await ViewModel.NextSongAsync();
            await Change(ViewModel.CurrentSong.Path);
            StateHasChanged();
        }
        private async Task OnClickPreviousButton()
        {
            await Stop();
            await ViewModel.PreviousSongAsync();
            await Change(ViewModel.CurrentSong.Path);
            StateHasChanged();
        }

        private async Task Play()
        {
            await JSRuntime.InvokeAsync<string>("player.play", "audioPlayer");
        }
        private async Task Pause()
        {
            await JSRuntime.InvokeAsync<string>("player.pause", "audioPlayer");
        }
        private async Task Stop()
        {
            await JSRuntime.InvokeAsync<string>("player.stop", "audioPlayer");
        }
        private async Task Change(string sourceFile)
        {
            await JSRuntime.InvokeAsync<string>("player.change", "audioPlayer", sourceFile);
        }

        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadSongsAsync();
            await base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await JSRuntime.InvokeAsync<string>("player.timeupdate");
        }
    }
}
