using System;
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
    public IEnumerable<SongModel> FilteredSongs { get; }
    public double CurrentTime { get; }
    public double Duration { get; }

    private MusicsState() : base()
    {
        Songs = [];
        IsLoading = false;
        IsRandom = false;
        IsPlaying = false;
        CurrentSongIndex = 0;
        ShowMusicList = false;
        FilteredSongs = [];
        CurrentTime = 0;
        Duration = 1; // Durée minimale par défaut
    }

    public MusicsState(
        MusicsState? currentState = null,
        IEnumerable<SongModel>? songs = null,
        bool? isLoading = null,
        SongModel? currentSong = null,
        bool? isRandom = null,
        bool? isPlaying = null,
        int? currentSongIndex = null,
        bool? showMusicList = null,
        IEnumerable<SongModel>? filteredSongs = null,
        double? currentTime = null,
        double? duration = null)
    {
        Songs = songs ?? currentState?.Songs ?? [];
        IsLoading = isLoading ?? currentState?.IsLoading ?? false;
        CurrentSong = currentSong ?? currentState?.CurrentSong ?? null;
        IsRandom = isRandom ?? currentState?.IsRandom ?? false;
        IsPlaying = isPlaying ?? currentState?.IsPlaying ?? false;
        CurrentSongIndex = currentSongIndex ?? currentState?.CurrentSongIndex ?? 0;
        ShowMusicList = showMusicList ?? currentState?.ShowMusicList ?? false;
        FilteredSongs = filteredSongs ?? currentState?.FilteredSongs ?? [];
        CurrentTime = currentTime ?? currentState?.CurrentTime ?? 0;
        Duration = duration ?? currentState?.Duration ?? 1;
    }

    public bool IsFirstSong() => CurrentSongIndex == 0;
    public bool IsLastSong() => Songs.Count() == (CurrentSongIndex + 1);

    // Récupère la chanson suivante selon les règles
    public SongModel? GetNextSong()
    {
        if (IsRandom && Songs.Any())
        {
            var random = new Random();
            return Songs.ElementAt(random.Next(0, Songs.Count()));
        }
        else if (!IsLastSong())
        {
            return Songs.ElementAt(CurrentSongIndex + 1);
        }

        return null;
    }

    // Récupère la chanson précédente selon les règles
    public SongModel? GetPreviousSong()
    {
        if (!IsFirstSong())
        {
            return Songs.ElementAt(CurrentSongIndex - 1);
        }

        return null;
    }
}
