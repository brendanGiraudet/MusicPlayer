using System.Threading.Tasks;
using Radzen;

namespace MusicPlayerApplication.Services.ModalService
{
    public class ModalService(DialogService _dialogService) : IModalService
    {
        public async Task ShowAsync(string title, string message)
        {
            _dialogService.Alert(message, title);
        }
    }
}
