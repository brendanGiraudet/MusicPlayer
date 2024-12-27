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

        public async Task<ResponseModel<HashSet<SongModel>>> GetSongsAsync()
        {
            var response = new ResponseModel<HashSet<SongModel>>
            {
                Content = [],
                HasError = true
            };

            try
            {
                var dirInfo = new DirectoryInfo(_youtubeDlSettings.MusicPath);

                // Récupération des fichiers en une seule passe
                var songFiles = _enabledMusicFileExtensions
                    .SelectMany(ext => dirInfo.GetFiles($"*.{ext}", SearchOption.TopDirectoryOnly))
                    .ToList();

                // Traitement parallèle des fichiers
                var tasks = songFiles.Select(async songFile =>
                {
                    var songInfoFilename = ChangeFilenameExtension(songFile.FullName, _infoFileExtension);

                    if (File.Exists(songInfoFilename))
                    {
                        return await GetSongModelAsync(songFile, songInfoFilename); // Génère le modèle
                    }
                    else
                    {
                        return null; // Ignore si le fichier info n'existe pas
                    }
                });

                // Attente des résultats
                var songs = await Task.WhenAll(tasks);

                // Filtrer les résultats valides
                response.Content = songs
                    .Where(song => song is not null)
                    .OrderByDescending(song => song!.CreationDate)
                    .ToHashSet()!;

                response.HasError = false;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                _logService.LogError(ex.Message);
            }

            return response;
        }


        private async Task<SongModel?> GetSongModelAsync(FileInfo songFile, string filename)
        {
            if (string.IsNullOrEmpty(filename)) return null;

            SongModel song = new()
            {
                Title = "Non défini",
                Artist = "Non défini"
            };

            try
            {
                var songInfo = await File.ReadAllTextAsync(filename);

                song = System.Text.Json.JsonSerializer.Deserialize<SongModel>(songInfo);
            }
            catch (System.Exception ex)
            {
                _logService.LogError($"{nameof(GetSongModelAsync)} ({filename}) : {ex.Message}");
            }

            song.Path = $"{_songSettings.Path}/{songFile.Name}";

            song.ImagePath = GetImagePath(songFile.Name);

            song.FileName = GetFileNameWithoutExtension(songFile.Name);

            song.CreationDate = songFile.CreationTime;

            return song;
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
