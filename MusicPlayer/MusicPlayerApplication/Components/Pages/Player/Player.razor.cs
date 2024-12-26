using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MusicPlayerApplication.Components.Player;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services.ModalService;
using MusicPlayerApplication.Services.SongService;
using MusicPlayerApplication.Stores;
using MusicPlayerApplication.Stores.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Pages.Player;

public partial class Player
{
    [Inject] public ISongService _songService { get; set; }
    [Inject] public IModalService _modalService { get; set; }
    [Inject] public ILogger<Player> Logger { get; set; }
    [Inject] public IDispatcher Dispatcher { get; set; }
    [Inject] public IState<MusicsState> MusicsState { get; set; }
    private SongModel _currentSong { get; set; }
    private bool _isEndList => MusicsState.Value.Songs.Count() == (_currentSongIndex + 1);
    private bool _isBeginList => 0 == _currentSongIndex;
    private int _currentSongIndex { get; set; } = 0;
    private bool _isRandom = false;
    private bool _isMusicListDisplay = false;
    private PlayerComponent _playerComponent;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new GetSongsAction());
    }

    private async Task RandomSong()
    {
        var random = new Random();
        _currentSongIndex = random.Next(0, MusicsState.Value.Songs.Count() - 1);
        _currentSong = MusicsState.Value.Songs.ElementAt(_currentSongIndex);
        await Task.CompletedTask;
    }

    public async Task PreviousSongAsync()
    {
        await ChangeSong(!_isBeginList, _currentSongIndex - 1);
    }

    public async Task NextSongAsync()
    {
        await ChangeSong(!_isEndList, _currentSongIndex + 1);
    }

    private async Task ChangeSong(bool canChange, int newSongIndex)
    {
        if (_isRandom)
        {
            await RandomSong();
        }

        else if (canChange)
        {
            _currentSongIndex = newSongIndex;
            _currentSong = MusicsState.Value.Songs.ElementAt(_currentSongIndex);
        }
        await Task.CompletedTask;
    }

    private void MusicListDisplayShuffle()
    {
        _isMusicListDisplay = !_isMusicListDisplay;
        StateHasChanged();
    }

    private async Task OnSelectedSong(SongModel song)
    {
        _currentSongIndex = MusicsState.Value.Songs.ToList().IndexOf(song);

        await _playerComponent.ReloadMusic();
        _isMusicListDisplay = false;
    }
}
