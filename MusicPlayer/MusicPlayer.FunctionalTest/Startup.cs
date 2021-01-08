using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayerApplication.Services;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Settings;

namespace MusicPlayer.FunctionalTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");
            var config = configBuilder.Build();

            // Config
            services.Configure<YoutubeDlSettings>(config.GetSection("YoutubeDl"));
            services.Configure<ShellSettings>(config.GetSection("Shell"));
            services.Configure<SongSettings>(config.GetSection("Song"));

            // Services
            services.AddTransient<IYoutubeDlService, YoutubeDlService>();
            services.AddTransient<IShellService, ShellService>();
        }
    }
}
