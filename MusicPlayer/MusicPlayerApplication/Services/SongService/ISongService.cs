using MusicPlayerApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.SongService
{
    public interface ISongService
    {
        Task<ResponseModel<IEnumerable<SongModel>>> GetSongsAsync();
    }
}
