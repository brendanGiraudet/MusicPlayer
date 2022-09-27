using Microsoft.Extensions.Options;
using MusicPlayerApplication.Enumerations;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Services.LogService;
using MusicPlayerApplication.Settings;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ShellService
{
    public class ShellService : IShellService
    {
        readonly ShellSettings _shellSettings;
        readonly ILogService _logService;
        public ShellService(IOptions<ShellSettings> shellSettingsOptions, ILogService logService)
        {
            _shellSettings = shellSettingsOptions.Value;
            _logService = logService;
        }
        public async Task<ResponseModel<bool>> RunAsync(string cmd)
        {
            var task = new TaskCompletionSource<ResponseModel<bool>>();
            var response = new ResponseModel<bool>
            {
                HasError = true
            };

            var escapedArgs = cmd.Replace("\"", "\\\"");
            System.Console.WriteLine($"command => {escapedArgs}");

            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _shellSettings.TerminalPath,
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                var isStarted = process.Start();
                if (!isStarted)
                {
                    var errorMessage = "Process not started !";
                    await _logService.Log(LogLevel.Errors.ToString(), errorMessage);
                    response.ErrorMessage = errorMessage;
                    return response;
                }

                var waitForExitTask = Task.Run(() => process.WaitForExit());
                await Task.WhenAny(waitForExitTask, Task.Delay(_shellSettings.Timeout));

                response.HasError = false;
                var message = $"The process {process.StartInfo.FileName} was executed successfully";
                await _logService.Log(LogLevel.Informations.ToString(), message);

                return response;
            }
            catch (System.Exception ex)
            {
                await _logService.Log(LogLevel.Errors.ToString(), ex.Message);
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
    }
}
