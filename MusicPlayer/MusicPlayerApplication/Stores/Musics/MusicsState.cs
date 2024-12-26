using System.Collections.Generic;
using Fluxor;
using MusicPlayerApplication.Models;

namespace MusicPlayerApplication.Stores;

[FeatureState]
public class MusicsState
{
    public IEnumerable<SongModel> Songs { get; }
    public SongModel CurrentSong { get; }
    public bool IsLoading { get; }
    public bool IsRandom { get; }
    public bool IsPlaying { get; }

    private MusicsState() : base()
    {
        Songs = [];
        IsLoading = false;
        IsRandom = false;
        IsPlaying = false;
    }

    public MusicsState(
        MusicsState? currentState = null,
        IEnumerable<SongModel>? songs = null,
        bool? isLoading = null,
        SongModel? currentSong = null,
        bool? isRandom = null,
        bool? isPlaying = null)
    {
        Songs = songs ?? currentState?.Songs ?? [];
        IsLoading = isLoading ?? currentState?.IsLoading ?? false;
        CurrentSong = currentSong ?? currentState?.CurrentSong ?? null;
        IsRandom = isRandom ?? currentState?.IsRandom ?? false;
        IsPlaying = isPlaying ?? currentState?.IsPlaying ?? false;
    }
}