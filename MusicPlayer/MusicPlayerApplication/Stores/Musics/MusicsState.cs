using System.Collections.Generic;
using System.Linq;
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
    public int CurrentSongIndex { get; }
    public bool ShowMusicList { get; }

    private MusicsState() : base()
    {
        Songs = [];
        IsLoading = false;
        IsRandom = false;
        IsPlaying = false;
        CurrentSongIndex = 0;
        ShowMusicList = false;
    }

    public MusicsState(
        MusicsState? currentState = null,
        IEnumerable<SongModel>? songs = null,
        bool? isLoading = null,
        SongModel? currentSong = null,
        bool? isRandom = null,
        bool? isPlaying = null,
        int? currentSongIndex = null,
        bool? showMusicList = null)
    {
        Songs = songs ?? currentState?.Songs ?? [];
        IsLoading = isLoading ?? currentState?.IsLoading ?? false;
        CurrentSong = currentSong ?? currentState?.CurrentSong ?? null;
        IsRandom = isRandom ?? currentState?.IsRandom ?? false;
        IsPlaying = isPlaying ?? currentState?.IsPlaying ?? false;
        CurrentSongIndex = currentSongIndex ?? currentState?.CurrentSongIndex ?? 0;
        ShowMusicList = showMusicList ?? currentState?.ShowMusicList ?? false;
    }

    public bool IsFirstSong() => CurrentSongIndex == 0;
    public bool IsLastSong() => Songs.Count() == (CurrentSongIndex + 1);
}