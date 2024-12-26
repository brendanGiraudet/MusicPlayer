using System.Linq;
using Fluxor;
using MusicPlayerApplication.Stores.Actions;

namespace MusicPlayerApplication.Stores;

public static class MusicsReducers
{
    #region GetSongs
    [ReducerMethod]
    public static MusicsState ReduceGetSongsAction(MusicsState state, GetSongsAction action) => new MusicsState(currentState: state, isLoading: true);

    [ReducerMethod]
    public static MusicsState ReduceGetSongsResultAction(MusicsState state, GetSongsResultAction action) => new MusicsState(currentState: state, songs: action.Songs, isLoading: false, currentSong: action.Songs.FirstOrDefault(), filteredSongs: action.Songs);
    #endregion

    #region SetCurrentSongAction
    [ReducerMethod]
    public static MusicsState ReduceSetCurrentSongAction(MusicsState state, SetCurrentSongAction action) => new MusicsState(currentState: state, currentSong: action.SongModel);
    #endregion

    #region SetIsRandomAction
    [ReducerMethod]
    public static MusicsState ReduceSetIsRandomAction(MusicsState state, SetIsRandomAction action) => new MusicsState(currentState: state, isRandom: action.IsRandom);
    #endregion

    #region SetIsPlayingAction
    [ReducerMethod]
    public static MusicsState ReduceSetIsPlayingAction(MusicsState state, SetIsPlayingAction action) => new MusicsState(currentState: state, isPlaying: action.IsPlaying);
    #endregion

    #region SetCurrentSongIndexAction
    [ReducerMethod]
    public static MusicsState ReduceSetCurrentSongIndexAction(MusicsState state, SetCurrentSongIndexAction action) => new MusicsState(currentState: state, currentSongIndex: action.CurrentSongIndex);
    #endregion

    #region SetShowMusicListAction
    [ReducerMethod]
    public static MusicsState ReduceSetShowMusicListAction(MusicsState state, SetShowMusicListAction action) => new MusicsState(currentState: state, showMusicList: action.ShowMusicList);
    #endregion


    #region RemoveSongResultAction
    [ReducerMethod]
    public static MusicsState ReduceRemoveSongResultAction(MusicsState state, RemoveSongResultAction action)
    {
        var songs = state.Songs;

        if (action.IsSuccess)
            songs = songs.Where(c => c.FileName != action.Filename);

        return new MusicsState(currentState: state, songs: songs, filteredSongs: songs);
    }
    #endregion
    
    
    #region SearchSongAction
    [ReducerMethod]
    public static MusicsState ReduceSearchSongAction(MusicsState state, SearchSongAction action)
    {
        var filteredSongs = state.Songs;

        if(!string.IsNullOrEmpty(action.Filter))
            filteredSongs = filteredSongs.Where(s => 
                (s.Title != null && s.Title.ToLowerInvariant().Contains(action.Filter.ToLowerInvariant())) || 
                (s.Artist != null && s.Artist.ToLowerInvariant().Contains(action.Filter.ToLowerInvariant())));

        return new MusicsState(currentState: state, filteredSongs: filteredSongs);
    }
    #endregion
}