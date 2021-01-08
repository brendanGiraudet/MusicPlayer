﻿using Microsoft.Extensions.Options;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services
{
    public class YoutubeDlService : IYoutubeDlService
    {
        readonly IOptions<YoutubeDlSettings> _youtubeDlSettings;
        readonly IShellService _shellService;
        public YoutubeDlService(IOptions<YoutubeDlSettings> youtubeDlSettings,
            IShellService shellService)
        {
            _youtubeDlSettings = youtubeDlSettings;
            _shellService = shellService;
        }
        public async Task<ResponseModel> DownloadMusicAsync(string url)
        {
            var proxy = _youtubeDlSettings.Value.Proxy;
            //var command = $"youtube-dl --proxy {proxy} --write-thumbnail {url}";
            var command = $"cd {_youtubeDlSettings.Value.MusicPath} && youtube-dl --write-thumbnail {url}";

            return await _shellService.RunAsync(command);
        }
    }
}