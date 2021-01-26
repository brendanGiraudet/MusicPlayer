using Microsoft.AspNetCore.Components;
using MusicPlayerApplication.Services.ModalService;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Loader
{
    public partial class Loader
    {
        [Inject] public ILoaderService LoaderService { get; set; }
        [Parameter] public bool IsDisplay { get; set; }
        public string DisplayClass => IsDisplay ? "show" : string.Empty;

        protected override void OnInitialized()
        {
            LoaderService.ToogleLoaderInvoked += ToogleLoader;
        }

        private async Task ToogleLoader()
        {
            IsDisplay = !IsDisplay;
            StateHasChanged();
            await Task.CompletedTask;
        }
    }
}
