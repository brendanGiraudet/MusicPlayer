using System;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ModalService
{
    public class LoaderService : ILoaderService
    {
        public event Func<Task>? ToogleLoaderInvoked;
        public async Task ShowAsync()
        {
            if(ToogleLoaderInvoked is not null)
                await ToogleLoaderInvoked.Invoke();
        }
        public async Task HideAsync()
        {
            if(ToogleLoaderInvoked is not null)
                await ToogleLoaderInvoked.Invoke();
        }
    }
}
