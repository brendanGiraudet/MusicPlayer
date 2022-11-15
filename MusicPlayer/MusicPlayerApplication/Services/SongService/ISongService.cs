using MusicPlayerApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.SongService
{
    public interface ISongService
    {
        Task<ResponseModel<HashSet<SongModel>>> GetSongsAsync();
        Task<ResponseModel<bool>> RemoveByNameAsync(string songFileName);
    }
}
