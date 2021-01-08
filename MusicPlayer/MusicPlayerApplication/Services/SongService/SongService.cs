using Microsoft.Extensions.Options;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.SongService
{
    public class SongService : ISongService
    {
        private YoutubeDlSettings _youtubeDlSettings;
        private SongSettings _songSettings;
        public SongService(IOptions<YoutubeDlSettings> youtubeDlSettings,
            IOptions<SongSettings> songSettings)
        {
            _youtubeDlSettings = youtubeDlSettings.Value;
            _songSettings = songSettings.Value;
        }

        Task<ResponseModel<IEnumerable<SongModel>>> ISongService.GetSongsAsync()
        {
            var response = new ResponseModel<IEnumerable<SongModel>>
            {
                Content = Enumerable.Empty<SongModel>(),
                HasError = true
            };
            try
            {
                var dirInfo = new DirectoryInfo(_youtubeDlSettings.MusicPath);
                var songFiles = dirInfo.GetFiles("*.mp3");
                foreach (var songFile in songFiles)
                {
                    var songInfo = File.ReadAllText(songFile.FullName.Replace("mp3", "info.json"));
                    var song = System.Text.Json.JsonSerializer.Deserialize<SongModel>(songInfo);

                    song.Path = _songSettings.Path + "/" + songFile.Name;
                    song.ImagePath = _songSettings.Path + "/" + songFile.Name.Replace("mp3", "jpg");
                    response.Content = response.Content.Append(song);
                }
                response.HasError = false;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return Task.FromResult(response);
        }
    }
}
