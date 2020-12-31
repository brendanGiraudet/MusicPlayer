using Microsoft.AspNetCore.Components;
using MusicPlayerApplication.Services.ModalService;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Components.Modal
{
    public partial class Modal
    {
        [Inject] public IModalService ModalService { get; set; }
        [Parameter] public bool IsDisplay { get; set; }
        public string DisplayClass => IsDisplay ? "show" : string.Empty;
        public string Title { get; set; }
        public string Message { get; set; }

        protected override void OnInitialized()
        {
            ModalService.ShowInvoked += ShowModal;
        }

        private async Task ShowModal(string title, string message)
        {
            Title = title;
            Message = message;
            Toggle();
            await Task.CompletedTask;
        }

        private void Toggle()
        {
            IsDisplay = !IsDisplay;
            StateHasChanged();
        }
    }
}
