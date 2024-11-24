using System;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ModalService
{
    public interface IModalService
    {
        Task ShowAsync(string title, string message);
    }
}
