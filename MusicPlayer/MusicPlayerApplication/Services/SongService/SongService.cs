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

        public Task<ResponseModel<bool>> RemoveByNameAsync(string songFileName)
        {
            var response = new ResponseModel<bool>
            {
                Content = false,
                HasError = true
            };

            try
            {
                var dirInfo = new DirectoryInfo(_youtubeDlSettings.MusicPath);
                var songFiles = dirInfo.GetFiles($"{songFileName}.*");

                foreach (var song in songFiles)
                {
                    File.Delete(song.FullName);
                }

                response.Content = true;
                response.HasError = false;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return Task.FromResult(response);
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
                    var webpImagePathToCheck = _youtubeDlSettings.MusicPath + "/" + songFile.Name.Replace("mp3", "webp");
                    var webpImagePath = _songSettings.Path + "/" + songFile.Name.Replace("mp3", "webp");
                    var jpgImagePath = _songSettings.Path + "/" + songFile.Name.Replace("mp3", "jpg");
                    var webpImageExist = File.Exists(webpImagePathToCheck);
                    song.ImagePath = webpImageExist ? webpImagePath : jpgImagePath;
                    song.FileName = songFile.Name.Replace(".mp3", "");
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
