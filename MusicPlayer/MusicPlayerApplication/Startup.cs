using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicPlayerApplication.Data;
using MusicPlayerApplication.Services;
using MusicPlayerApplication.Services.ModalService;
using MusicPlayerApplication.Services.ShellService;
using MusicPlayerApplication.Services.SongService;
using MusicPlayerApplication.Settings;
using MusicPlayerApplication.ViewModels;
using MusicPlayerApplication.ViewModels.PlayerViewModel;

namespace MusicPlayerApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Other
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Config
            services.Configure<YoutubeDlSettings>(Configuration.GetSection("YoutubeDl"));
            services.Configure<ShellSettings>(Configuration.GetSection("Shell"));
            services.Configure<SongSettings>(Configuration.GetSection("Song"));

            // Services
            services.AddTransient<IYoutubeDlService, YoutubeDlService>();
            services.AddTransient<IShellService, ShellService>();
            services.AddTransient<ISongService, SongService>();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<IModalService, ModalService>();
            services.AddSingleton<ILoaderService, LoaderService>();

            // ViewModel
            services.AddTransient<IYoutubeDlViewModel, YoutubeDlViewModel>();
            services.AddTransient<IPlayerViewModel, PlayerViewModel>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
