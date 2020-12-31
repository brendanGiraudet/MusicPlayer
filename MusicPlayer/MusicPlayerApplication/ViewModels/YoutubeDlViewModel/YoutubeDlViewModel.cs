using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services;
using MusicPlayerApplication.Services.ModalService;
using System;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels
{
    public class YoutubeDlViewModel : BaseViewModel, IYoutubeDlViewModel
    {
        private readonly IYoutubeDlService _youtubeDlService;
        private readonly IModalService _modalService;
        private YoutubeVideoModel _model = new YoutubeVideoModel();

        public YoutubeDlViewModel(
            IYoutubeDlService youtubeDlService,
            IModalService modalService
            )
        {
            _youtubeDlService = youtubeDlService;
            _modalService = modalService;
        }

        public YoutubeVideoModel Model
        {
            get => _model;
            set => SetValue(ref _model, value);
        }

        public async Task DownloadMusicAsync()
        {
            IsBusy = true;
            await _youtubeDlService.DownloadMusicAsync(Model.Url);
            await _modalService.ShowAsync("Téléchargement de la musique provenant de youtube", "Le téléchargement c'est bien effectué");
            IsBusy = false;
        }
    }
}
