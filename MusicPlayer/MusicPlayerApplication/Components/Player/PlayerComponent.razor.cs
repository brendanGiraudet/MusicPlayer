﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.ViewModels.PlayerViewModel;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Player
{
    public partial class PlayerComponent
    {
        [Inject] public IPlayerViewModel ViewModel { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        private bool _isPlaying = false;
        private string _currentTime;
        private string _duration;
        private readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true };
        private bool _isRandom = false;

        private bool HasNextButtonDisabled => ViewModel.IsEndList && !_isRandom;
        private bool HasPreviousButtonDisabled => ViewModel.IsBeginList && !_isRandom;
        public string CurrentTimeAsTime => ConvertToTime(_currentTime);
        public string DurationAsTime => ConvertToTime(_duration);
        public string IconPlayer => _isPlaying ? "pause" : "play_arrow";
        public string Id { get; set; } = "audioPlayer";
        public string RandomClass => _isRandom ? string.Empty : "no-random";
        public string DisabledNextButtonClass => HasNextButtonDisabled ? "disabled" : string.Empty;
        public string DisabledPreviousButtonClass => HasPreviousButtonDisabled ? "disabled" : string.Empty;

        private Task OnClickPlayPauseButton() => _isPlaying ? Pause() : Play();

        private bool IsCurrentSong(string title) => ViewModel.CurrentSong.Title == title;
        private string PlayedIcon(string title) => IsCurrentSong(title) ? "equalizer" : "play_arrow";

        private async Task OnClickNextButton()
        {
            if (!HasNextButtonDisabled)
            {
                await Stop();
                await ViewModel.NextSongAsync(_isRandom);
                await ChangeSource(ViewModel.CurrentSong.Path);
                StateHasChanged();
                await Play();
            }
        }
        private async Task OnClickPreviousButton()
        {
            if (!HasPreviousButtonDisabled)
            {
                await Stop();
                await ViewModel.PreviousSongAsync(_isRandom);
                await ChangeSource(ViewModel.CurrentSong.Path);
                StateHasChanged();
                await Play();
            }
        }

        private async Task Play()
        {
            _isPlaying = true;
            await JSRuntime.InvokeAsync<string>("player.play", "audioPlayer");
        }
        private async Task Pause()
        {
            _isPlaying = false;
            await JSRuntime.InvokeAsync<string>("player.pause", "audioPlayer");
        }
        private async Task Stop()
        {
            _isPlaying = false;
            await JSRuntime.InvokeAsync<string>("player.stop", "audioPlayer");
        }
        private async Task ChangeSource(string sourceFile)
        {
            await JSRuntime.InvokeAsync<string>("player.change", "audioPlayer", sourceFile);
        }

        protected override async Task OnInitializedAsync()
        {
            await ViewModel.LoadSongsAsync();
            await base.OnInitializedAsync();
        }
        private void TimeUpdate(AudioState audioState)
        {
            _currentTime = audioState.CurrentTime.ToString();
            _duration = audioState.Duration.ToString();
        }
        private async Task Ended()
        {
            await OnClickNextButton();
            await Play();
        }

        private string ConvertToTime(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration)) return duration;

            var sec_num = Math.Round(Convert.ToDouble(duration));
            var hours = Math.Floor(sec_num / 3600);
            var minutes = Math.Floor((sec_num - (hours * 3600)) / 60);
            var seconds = sec_num - (hours * 3600) - (minutes * 60);

            string stringHours = hours.ToString();
            string stringMinutes = minutes.ToString();
            string stringSeconds = seconds.ToString();

            if (hours < 10) { stringHours = "0" + stringHours; }
            if (minutes < 10) { stringMinutes = "0" + stringMinutes; }
            if (seconds < 10) { stringSeconds = "0" + stringSeconds; }
            return $"{stringHours}:{stringMinutes}:{stringSeconds}";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await ConfigureEvents();
            }
        }
        private async Task ConfigureEvents()
        {
            await Implement(AudioEvents.TimeUpdate);
            await Implement(AudioEvents.Ended);
        }
        private async Task Implement(AudioEvents eventName)
        {
            await JSRuntime.InvokeVoidAsync("window.CustomEventHandler", Id, eventName.ToString().ToLower(), AudioState.GetPayload());
        }

        private async Task OnChange(ChangeEventArgs args)
        {
            var ThisEvent = args?.Value?.ToString();
            var videoData = new AudioEventData();
            try
            {
                videoData = JsonSerializer.Deserialize<AudioEventData>(ThisEvent, serializationOptions);
                switch (videoData.EventName)
                {
                    case AudioEvents.TimeUpdate:
                        TimeUpdate(videoData.State);
                        break;
                    case AudioEvents.Ended:
                        await Ended();
                        break;
                    default:
                        Console.WriteLine($"{ThisEvent} was not implemented");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to convert the JSON: {ThisEvent}");
            }
        }

        private async Task OnClickPlayIcon(SongModel song)
        {
            if (_isPlaying && ViewModel.CurrentSong.Title == song.Title)
            {
                await Pause();
            }
            else
            {
                ViewModel.CurrentSong = song;
                ViewModel.CurrentSongIndex = ViewModel.Songs.ToList().IndexOf(song);
                await ChangeSource(ViewModel.CurrentSong.Path);
                StateHasChanged();
                await Play();
            }
        }

        private async Task OnFilterChanged(ChangeEventArgs changeEventArgs)
        {
            var value = changeEventArgs.Value?.ToString();

            if(value != null) await ViewModel.ApplyFilter(value);
        }
    }
}
