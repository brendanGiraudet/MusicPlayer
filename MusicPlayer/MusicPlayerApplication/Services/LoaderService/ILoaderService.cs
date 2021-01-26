using System;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ModalService
{
    public interface ILoaderService
    {
        event Func<Task> ToogleLoaderInvoked;
        Task HideAsync();
        Task ShowAsync();
    }
}
