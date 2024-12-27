using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MusicPlayerApplication.Stores;
using MusicPlayerApplication.Stores.Actions;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Player;

public partial class PlayerComponent
{
    [Inject] public IJSRuntime JSRuntime { get; set; }
    [Inject] public IState<MusicsState> MusicsState { get; set; }
    [Inject] public IDispatcher Dispatcher { get; set; }
    [Parameter] public bool IsDisplay { get; set; }

    private IJSObjectReference? module;

    public string IconPlayer => MusicsState.Value.IsPlaying ? "pause" : "play_arrow";

    public string Id { get; set; } = "audioPlayer";

    private bool HasNextButtonDisabled => MusicsState.Value.IsLastSong() && !MusicsState.Value.IsRandom;

    private bool HasPreviousButtonDisabled => MusicsState.Value.IsFirstSong() && !MusicsState.Value.IsRandom;

    private Task OnClickPlayPauseButton() => MusicsState.Value.IsPlaying ? Pause() : Play();

    private async Task OnClickNextButton()
    {
        var nextSong = MusicsState.Value.GetNextSong();
        if (nextSong != null)
        {
            Dispatcher.Dispatch(new SetCurrentSongAction(nextSong));
            Dispatcher.Dispatch(new SetCurrentSongIndexAction(MusicsState.Value.Songs.ToList().IndexOf(nextSong)));
            await ReloadMusic();
        }
    }

    private async Task OnClickPreviousButton()
    {
        var previousSong = MusicsState.Value.GetPreviousSong();
        if (previousSong != null)
        {
            Dispatcher.Dispatch(new SetCurrentSongAction(previousSong));
            Dispatcher.Dispatch(new SetCurrentSongIndexAction(MusicsState.Value.Songs.ToList().IndexOf(previousSong)));
            await ReloadMusic();
        }
    }

    public async Task ReloadMusic()
    {
        await Stop();

        await ChangeSource(MusicsState.Value.CurrentSong.Path);

        await Play();
    }

    private async Task Play()
    {
        if (!MusicsState.Value.IsPlaying)
        {
            Dispatcher.Dispatch(new SetIsPlayingAction(true));
            await module.InvokeAsync<string>("play", Id);
        }
    }

    private async Task Pause()
    {
        if (MusicsState.Value.IsPlaying)
        {
            Dispatcher.Dispatch(new SetIsPlayingAction(false));
            await module.InvokeAsync<string>("pause", Id);
        }
    }

    private async Task Stop()
    {
        if (MusicsState.Value.IsPlaying)
        {
            Dispatcher.Dispatch(new SetIsPlayingAction(false));
            await module.InvokeAsync<string>("stop", Id);
        }
    }

    private async Task ChangeSource(string sourceFile)
    {
        await module.InvokeAsync<string>("change", Id, sourceFile);
    }

    // Gestion des événements JS
    [JSInvokable]
    public void OnTimeUpdate(double currentTime, double duration)
    {
        Dispatcher.Dispatch(new SetCurrentTimeAction(Math.Round(currentTime)));
        Dispatcher.Dispatch(new SetDurationAction(Math.Round(duration)));
    }

    [JSInvokable]
    public async Task OnAudioEnded()
    {
        await OnClickNextButton();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            module = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./Components/Player/PlayerComponent.razor.js");

            var componentRef = DotNetObjectReference.Create(this);

            await module.InvokeVoidAsync("configureAudio", Id, componentRef, nameof(OnTimeUpdate), nameof(OnAudioEnded));
        }
    }

    private async Task OnRandomClick()
    {
        Dispatcher.Dispatch(new SetIsRandomAction(!MusicsState.Value.IsRandom));
    }

    private async Task OnShowMusicListClick()
    {
        Dispatcher.Dispatch(new SetShowMusicListAction(true));
    }

    private void UpdateCurrentTime(double expectedTime)
    {
        Dispatcher.Dispatch(new SetCurrentTimeAction(expectedTime));

        module.InvokeAsync<string>("updateCurrentTime", Id, expectedTime);
    }
}
