using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MusicPlayerApplication.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Player
{
    public partial class PlayerComponent
    {
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Parameter] public bool IsEndList { get; set; }
        [Parameter] public bool IsBeginList { get; set; }
        [Parameter] public bool IsRandom { get; set; }
        [Parameter] public EventCallback OnNextClickCallback { get; set; }
        [Parameter] public EventCallback OnPreviousClickCallback { get; set; }
        [Parameter] public EventCallback OnRandomClickCallback { get; set; }
        [Parameter] public EventCallback OnShowMusicListClickCallback { get; set; }
        [Parameter] public SongModel CurrentSong { get; set; }
        [Parameter] public bool IsDisplay { get; set; }

        private string CssDisplay => !IsDisplay ? "hidden" : string.Empty;

        private readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true };

        public string CurrentTimeAsTime => ConvertToTime(_currentTime);
        private string _currentTime;

        public string DurationAsTime => ConvertToTime(_duration);
        private string _duration;

        public string IconPlayer => _isPlaying ? "pause" : "play_arrow";
        private bool _isPlaying = false;

        public string Id { get; set; } = "audioPlayer";

        public string RandomClass => IsRandom ? string.Empty : "no-random";

        public string DisabledNextButtonClass => HasNextButtonDisabled ? "disabled" : string.Empty;
        private bool HasNextButtonDisabled => IsEndList && !IsRandom;

        public string DisabledPreviousButtonClass => HasPreviousButtonDisabled ? "disabled" : string.Empty;
        private bool HasPreviousButtonDisabled => IsBeginList && !IsRandom;

        private Task OnClickPlayPauseButton() => _isPlaying ? Pause() : Play();

        private async Task OnClickNextButton()
        {
            await OnMusicChange(OnNextClickCallback, !HasNextButtonDisabled);
        }

        private async Task OnClickPreviousButton()
        {
            await OnMusicChange(OnPreviousClickCallback, !HasPreviousButtonDisabled);
        }

        private async Task OnMusicChange(EventCallback callback, bool canChangeMusic)
        {
            if (canChangeMusic)
            {
                if (callback.HasDelegate) await callback.InvokeAsync();

                await ReloadMusic();
            }
        }

        public async Task ReloadMusic()
        {
            await Stop();
            await ChangeSource(CurrentSong.Path);

            StateHasChanged();

            await Play();
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

        private void TimeUpdate(AudioState audioState)
        {
            _currentTime = Math.Round(audioState.CurrentTime).ToString();
            _duration = Math.Round(audioState.Duration).ToString();
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
                // await Play();
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
            try
            {
                var videoData = JsonSerializer.Deserialize<AudioEventData>(ThisEvent, serializationOptions);
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
                Console.WriteLine($"Due to: {ex.Message}");
            }
        }

        private async Task OnRandomClick()
        {
            if (OnRandomClickCallback.HasDelegate) await OnRandomClickCallback.InvokeAsync();
        }

        private async Task OnShowMusicListClick()
        {
            if (OnShowMusicListClickCallback.HasDelegate) await OnShowMusicListClickCallback.InvokeAsync();
        }
    }
}
