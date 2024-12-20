using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MusicPlayerApplication.Components.Player;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services.ModalService;
using MusicPlayerApplication.Services.SongService;
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
    private HashSet<SongModel> _songs { get; set; } = new HashSet<SongModel>();
    private SongModel _currentSong { get; set; }
    private bool _isEndList => _songs.Count() == (_currentSongIndex + 1);
    private bool _isBeginList => 0 == _currentSongIndex;
    private int _currentSongIndex { get; set; } = 0;
    private bool _isRandom = false;
    private bool _isMusicListDisplay = false;
    private PlayerComponent _playerComponent;

    protected override async Task OnInitializedAsync()
    {
        await LoadSongsAsync();
        await base.OnInitializedAsync();
    }

    public async Task LoadSongsAsync()
    {
        var getSongsResponse = await _songService.GetSongsAsync();

        if (getSongsResponse.HasError)
        {
            var message = "Erreur lors du chargement des musiques";
            await _modalService.ShowAsync(message, getSongsResponse.ErrorMessage);
            
            Logger.LogError(message);
        }
        else
        {
            _songs = getSongsResponse.Content;
            _currentSong = getSongsResponse.Content.FirstOrDefault();
        }
    }

    private async Task RandomSong()
    {
        var random = new Random();
        _currentSongIndex = random.Next(0, _songs.Count() - 1);
        _currentSong = _songs.ElementAt(_currentSongIndex);
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
            _currentSong = _songs.ElementAt(_currentSongIndex);
        }
        await Task.CompletedTask;
    }

    private void RandomSongShuffle()
    {
        _isRandom = !_isRandom;
        StateHasChanged();
    }

    private void MusicListDisplayShuffle()
    {
        _isMusicListDisplay = !_isMusicListDisplay;
        StateHasChanged();
    }

    private async Task OnSelectedSong(SongModel song)
    {
        _currentSong = song;

        _currentSongIndex = _songs.ToList().IndexOf(song);

        await _playerComponent.ReloadMusic();
        _isMusicListDisplay = false;
    }
}
