using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SongService> _logService;
        private string[] _enabledMusicFileExtensions = { "wav", "mp3", "opus", "best", "aac", "flac", "m4a", "vorbis", "webm" };
        private string[] _enabledImageFileExtensions = { ".jpg", ".webp" };
        private string _infoFileExtension = ".info.json";

        public SongService(IOptions<YoutubeDlSettings> youtubeDlSettings,
            IOptions<SongSettings> songSettings, ILogger<SongService> logService)
        {
            _youtubeDlSettings = youtubeDlSettings.Value;
            _songSettings = songSettings.Value;
            _logService = logService;
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

                var message = $"{songFileName} was deleted with successfully";
                _logService.LogInformation(message);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                _logService.LogError(ex.Message);
            }

            return Task.FromResult(response);
        }

        Task<ResponseModel<HashSet<SongModel>>> ISongService.GetSongsAsync()
        {
            var response = new ResponseModel<HashSet<SongModel>>
            {
                Content = new(),
                HasError = true
            };
            try
            {
                var dirInfo = new DirectoryInfo(_youtubeDlSettings.MusicPath);

                var songFiles = new List<FileInfo>();

                foreach (var enableMusicFileExtension in _enabledMusicFileExtensions)
                {
                    songFiles.AddRange(dirInfo.GetFiles($"*.{enableMusicFileExtension}"));
                }

                foreach (var songFile in songFiles)
                {
                    var songInfoFilename = $"{ChangeFilenameExtension(songFile.FullName, _infoFileExtension)}";

                    if (File.Exists(songInfoFilename))
                    {
                        var songInfo = File.ReadAllText(songInfoFilename);

                        SongModel? song = GetSongModel(songFile, songInfo);

                        if (song is not null)
                            response.Content.Add(song);
                    }
                    else
                    {
                        _logService.LogWarning($"Fichier de nom {songFile.FullName}, Fichier d'info {songInfoFilename} n'existe pas");
                    }
                }

                response.HasError = false;

                response.Content = response.Content.OrderByDescending(c => c.CreationDate).ToHashSet();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                _logService.LogError(ex.Message);
            }

            return Task.FromResult(response);
        }

        private SongModel? GetSongModel(FileInfo songFile, string songInfo)
        {
            if (string.IsNullOrEmpty(songInfo)) return null;

            try
            {
                var song = System.Text.Json.JsonSerializer.Deserialize<SongModel>(songInfo);

                song.Path = $"{_songSettings.Path}/{songFile.Name}";

                song.ImagePath = GetImagePath(songFile.Name);

                song.FileName = GetFileNameWithoutExtension(songFile.Name);

                song.CreationDate = songFile.CreationTime;

                return song;
            }
            catch (System.Exception ex)
            {
                _logService.LogError($"{nameof(GetSongModel)} ({songInfo}) : {ex.Message}");

                return null;
            }
        }

        private string GetImagePath(string songName)
        {
            foreach (var enableImageExtension in _enabledImageFileExtensions)
            {
                var imageName = ChangeFilenameExtension(songName, enableImageExtension);
                var imagePathToCheck = $"{_youtubeDlSettings.MusicPath}/{imageName}";

                if (File.Exists(imagePathToCheck))
                    return $"{_songSettings.Path}/{imageName}";
            }
            return string.Empty;
        }

        private string ChangeFilenameExtension(string nameFile, string extension)
        {
            var filenameWithoutExtension = GetFileNameWithoutExtension(nameFile);
            return $"{filenameWithoutExtension}{extension}";
        }

        private string GetFileNameWithoutExtension(string filenameWithExtension)
        {
            var indexOfDot = filenameWithExtension.LastIndexOf('.');
            return filenameWithExtension.Substring(0, indexOfDot);
        }
    }
}
