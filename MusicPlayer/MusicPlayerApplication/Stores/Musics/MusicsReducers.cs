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
    public static MusicsState ReduceGetSongsResultAction(MusicsState state, GetSongsResultAction action) => new MusicsState(currentState: state, songs: action.Songs ,isLoading: false, currentSong: action.Songs.FirstOrDefault());
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
}