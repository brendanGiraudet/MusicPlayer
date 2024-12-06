using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using MusicPlayerApplication.Services;
using MusicPlayerApplication.Services.CustomLogger;
using MusicPlayerApplication.Services.ModalService;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Services.SongService;
using MusicPlayerApplication.Settings;
using MusicPlayerApplication.ViewModels;
using MusicPlayerApplication.ViewModels.PlayerViewModel;
using MusicPlayerApplication.ViewModels.SongManagerViewModel;

namespace MusicPlayerApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<YoutubeDlSettings>(configuration.GetSection("YoutubeDl"));
            services.Configure<ShellSettings>(configuration.GetSection("Shell"));
            services.Configure<SongSettings>(configuration.GetSection("Song"));
            services.Configure<LogSettings>(configuration.GetSection("Log"));
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IYoutubeDlService, YoutubeDlService>();
            services.AddTransient<IShellService, ShellService>();
            services.AddTransient<ISongService, SongService>();
            services.AddTransient<IModalService, ModalService>();
            services.AddSingleton<ILoaderService, LoaderService>();
        }

        public static void AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<IYoutubeDlViewModel, YoutubeDlViewModel>();
            services.AddTransient<IPlayerViewModel, PlayerViewModel>();
            services.AddTransient<ISongManagerViewModel, SongManagerViewModel>();
        }

        public static ILoggingBuilder AddCustomLogger(this ILoggingBuilder builder)
        {
            builder.ClearProviders();

            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, CustomLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <CustomLoggerSettings, CustomLoggerProvider>(builder.Services);

            return builder;
        }
    }
}