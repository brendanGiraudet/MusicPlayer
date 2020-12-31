using System;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ModalService
{
    public interface IModalService
    {
        event Func<string, string, Task> ShowInvoked;

        Task ShowAsync(string title, string message);
    }
}
