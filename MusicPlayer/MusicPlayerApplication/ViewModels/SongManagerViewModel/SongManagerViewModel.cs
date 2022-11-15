using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services.ModalService;
using MusicPlayerApplication.Services.SongService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels.SongManagerViewModel
{
    public class SongManagerViewModel : ISongManagerViewModel
    {
        private readonly ISongService _songService;
        private readonly IModalService _modalService;
        public SongManagerViewModel(
            IModalService modalService,
            ISongService songService)
        {
            _modalService = modalService;
            _songService = songService;
        }

        public HashSet<SongModel> Songs { get; set; } = new();

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
            }
        }

        public async Task RemoveAsync(string name, string title)
        {
            await _songService.RemoveByNameAsync(name);
            await _modalService.ShowAsync("Confirmation", $"La musique {title}({name}) à bien été supprimée");
            var removedSong = Songs.FirstOrDefault(s => s.FileName == name);

            if (removedSong != null)
            {
                Songs.RemoveWhere(s => s.FileName == name);
            }
        }
    }
}
