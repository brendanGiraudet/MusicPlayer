using MusicPlayerApplication.Models;

namespace MusicPlayerApplication.Services.ShellService
{
    public interface IShellService
    {
        ResponseModel Run(string cmd);
    }
}
