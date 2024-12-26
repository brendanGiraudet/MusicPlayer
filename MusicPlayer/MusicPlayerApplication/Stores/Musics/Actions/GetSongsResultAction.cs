using System.Collections.Generic;
using MusicPlayerApplication.Models;

namespace MusicPlayerApplication.Stores.Actions;

public record GetSongsResultAction(IEnumerable<SongModel> Songs){}