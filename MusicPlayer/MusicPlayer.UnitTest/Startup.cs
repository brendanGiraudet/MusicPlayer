using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayerApplication.Services;
using MusicPlayerApplication.Settings;

namespace MusicPlayer.UnitTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");
            var config = configBuilder.Build();

            services.Configure<YoutubeDlSettings>(config.GetSection("YoutubeDl"));
            services.Configure<ShellSettings>(config.GetSection("Shell"));
            services.AddTransient<IYoutubeDlService, YoutubeDlService>();
        }
    }
}
