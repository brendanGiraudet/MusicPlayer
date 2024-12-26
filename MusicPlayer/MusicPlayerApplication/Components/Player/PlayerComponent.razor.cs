using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Stores;
using MusicPlayerApplication.Stores.Actions;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Player;

public partial class PlayerComponent
{
    [Inject] public IJSRuntime JSRuntime { get; set; }
    [Inject] public IState<MusicsState> MusicsState { get; set; }
    [Inject] public IDispatcher Dispatcher { get; set; }
    [Parameter] public bool IsEndList { get; set; }
    [Parameter] public bool IsBeginList { get; set; }
    [Parameter] public EventCallback OnNextClickCallback { get; set; }
    [Parameter] public EventCallback OnPreviousClickCallback { get; set; }
    [Parameter] public EventCallback OnShowMusicListClickCallback { get; set; }
    [Parameter] public bool IsDisplay { get; set; }

    private IJSObjectReference? module;

    private string CssDisplay => !IsDisplay ? "hidden" : string.Empty;

    private readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true };

    public string CurrentTimeAsTime => ConvertToTime(_currentTime);
    private double _currentTime = 0;

    public string DurationAsTime => ConvertToTime(Duration);
    public decimal DurationAsDecimal => Convert.ToDecimal(Duration);
    public double Duration { get; set; } = 1;

    public string IconPlayer => MusicsState.Value.IsPlaying ? "pause" : "play_arrow";

    public string Id { get; set; } = "audioPlayer";

    public string RandomClass => MusicsState.Value.IsRandom ? string.Empty : "no-random";

    public string DisabledNextButtonClass => HasNextButtonDisabled ? "disabled" : string.Empty;
    private bool HasNextButtonDisabled => IsEndList && !MusicsState.Value.IsRandom;

    public string DisabledPreviousButtonClass => HasPreviousButtonDisabled ? "disabled" : string.Empty;
    private bool HasPreviousButtonDisabled => IsBeginList && !MusicsState.Value.IsRandom;

    private Task OnClickPlayPauseButton() => MusicsState.Value.IsPlaying ? Pause() : Play();

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
        await ChangeSource(MusicsState.Value.CurrentSong.Path);

        StateHasChanged();

        await Play();
    }

    private async Task Play()
    {
        Dispatcher.Dispatch(new SetIsPlayingAction(true));
        await module.InvokeAsync<string>("play", Id);
    }

    private async Task Pause()
    {
        Dispatcher.Dispatch(new SetIsPlayingAction(false));
        await module.InvokeAsync<string>("pause", Id);
    }

    private async Task Stop()
    {
        Dispatcher.Dispatch(new SetIsPlayingAction(false));
        await module.InvokeAsync<string>("stop", Id);
    }

    private async Task ChangeSource(string sourceFile)
    {
        await module.InvokeAsync<string>("change", Id, sourceFile);
    }

    private void TimeUpdate(AudioState audioState)
    {
        _currentTime = Math.Round(audioState.CurrentTime);
        Duration = audioState.Duration > 0 ? Math.Round(audioState.Duration) : 1;
    }

    private async Task Ended()
    {
        await OnClickNextButton();
        await Play();
    }

    private string ConvertToTime(double secondsNumber)
    {
        var sec_num = Math.Round(secondsNumber);
        var hours = Math.Floor(sec_num / 3600);
        var minutes = Math.Floor((sec_num - (hours * 3600)) / 60);
        var seconds = sec_num - (hours * 3600) - (minutes * 60);

        string stringHours = hours.ToString().PadLeft(2, '0');
        string stringMinutes = minutes.ToString().PadLeft(2, '0');
        string stringSeconds = seconds.ToString().PadLeft(2, '0');

        return $"{stringHours}:{stringMinutes}:{stringSeconds}";
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            module = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./Components/Player/PlayerComponent.razor.js");

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
        await module.InvokeVoidAsync("CustomEventHandler", Id, eventName.ToString().ToLower(), AudioState.GetPayload());
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
        Dispatcher.Dispatch(new SetIsRandomAction(!MusicsState.Value.IsRandom));
    }

    private async Task OnShowMusicListClick()
    {
        if (OnShowMusicListClickCallback.HasDelegate) await OnShowMusicListClickCallback.InvokeAsync();
    }

    private void UpdateCurrentTime(double expectedTime)
    {
        _currentTime = expectedTime;
        module.InvokeAsync<string>("updateCurrentTime", Id, expectedTime);
    }
}
