using MusicPlayerApplication.Models;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ShellService
{
    public interface IShellService
    {
        Task<ResponseModel<bool>> RunAsync(string cmd);
    }
}
