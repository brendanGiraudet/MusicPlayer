using System;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ModalService
{
    public class ModalService : IModalService
    {
        public event Func<string, string, Task> ShowInvoked;
        public async Task ShowAsync(string title, string message)
        {
            await ShowInvoked.Invoke(title, message);
        }
    }
}
