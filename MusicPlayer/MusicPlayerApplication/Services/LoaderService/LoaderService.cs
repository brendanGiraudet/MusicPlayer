using System;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ModalService
{
    public class LoaderService : ILoaderService
    {
        public event Func<Task> ToogleLoaderInvoked;
        public async Task ShowAsync()
        {
            await ToogleLoaderInvoked.Invoke();
        }
        public async Task HideAsync()
        {
            await ToogleLoaderInvoked.Invoke();
        }
    }
}
