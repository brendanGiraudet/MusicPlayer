using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services.ModalService;
using MusicPlayerApplication.Services.SongService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels.PlayerViewModel
{
    public class PlayerViewModel : IPlayerViewModel
    {
        private readonly ISongService _songService;
        private readonly IModalService _modalService;

        public PlayerViewModel(ISongService songService,
            IModalService modalService)
        {
            _songService = songService;
            _modalService = modalService;
        }
        public IEnumerable<SongModel> Songs { get; set; } = Enumerable.Empty<SongModel>();
        public int CurrentSongIndex { get; set; } = 0;
        public SongModel CurrentSong { get; set; }
        public bool IsEndList => Songs.Count() == (CurrentSongIndex + 1);
        public bool IsBeginList => 0 == CurrentSongIndex;

        public async Task LoadSongsAsync()
        {
            var getSongsResponse = await _songService.GetSongsAsync();

            if (getSongsResponse.HasError)
            {
                await _modalService.ShowAsync("Erreur lors du chargement des musiques", getSongsResponse.ErrorMessage);
            }
            else
            {
                Songs = getSongsResponse.Content;
                CurrentSong = getSongsResponse.Content.FirstOrDefault();
            }
        }

        public async Task NextSongAsync()
        {
            if (!IsEndList)
            {
                CurrentSongIndex++;
                CurrentSong = Songs.ToArray()[CurrentSongIndex];
            }
            await Task.CompletedTask;
        }

        public async Task PreviousSongAsync()
        {
            if (!IsBeginList)
            {
                CurrentSongIndex--;
                CurrentSong = Songs.ToArray()[CurrentSongIndex];
            }
            await Task.CompletedTask;
        }
    }
}
