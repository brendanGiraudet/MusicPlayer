using Microsoft.AspNetCore.Components;
using MusicPlayerApplication.ViewModels;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.YoutubeDownloader
{
    public partial class YoutubeDownloaderComponent
    {
        [Inject] public IYoutubeDlViewModel ViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViewModel.PropertyChanged += async (sender, e) =>
            {
                await InvokeAsync(() =>
                {
                    StateHasChanged();
                });
            };
            await base.OnInitializedAsync();
        }
    }
}
