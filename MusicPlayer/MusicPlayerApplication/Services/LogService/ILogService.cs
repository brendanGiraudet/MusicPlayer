using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.LogService
{
    public interface ILogService
    {
        Task<bool> Log(string level, string message);
    }
}
