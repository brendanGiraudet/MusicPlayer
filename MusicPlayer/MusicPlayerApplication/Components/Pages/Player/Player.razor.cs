using Fluxor;
using Microsoft.AspNetCore.Components;
using MusicPlayerApplication.Components.Player;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Stores;
using MusicPlayerApplication.Stores.Actions;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Pages.Player;

public partial class Player
{
    [Inject] public IDispatcher Dispatcher { get; set; }
    [Inject] public IState<MusicsState> MusicsState { get; set; }

    private bool _isMusicListDisplay = false;
    private PlayerComponent _playerComponent;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new GetSongsAction());
    }

    private void MusicListDisplayShuffle()
    {
        _isMusicListDisplay = !_isMusicListDisplay;
        StateHasChanged();
    }

    private async Task OnSelectedSong(SongModel song)
    {
        Dispatcher.Dispatch(new SetCurrentSongIndexAction(MusicsState.Value.Songs.ToList().IndexOf(song)));

        await _playerComponent.ReloadMusic();
        _isMusicListDisplay = false;
    }
}
