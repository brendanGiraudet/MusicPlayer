using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Settings;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MusicPlayerApplication.Services.ShellService
{
    public class ShellService : IShellService
    {
        private readonly ShellSettings _shellSettings;
        private readonly ILogger<ShellService> _logService;

        public ShellService(IOptions<ShellSettings> shellSettingsOptions, ILogger<ShellService> logService)
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

            _logService.LogInformation($"command => {escapedArgs}");

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
                    
                    _logService.LogError(errorMessage);
                    
                    response.ErrorMessage = errorMessage;
                    
                    return response;
                }

                var waitForExitTask = Task.Run(() => process.WaitForExit());
                await Task.WhenAny(waitForExitTask, Task.Delay(_shellSettings.Timeout));

                response.HasError = false;
                var message = $"The process {process.StartInfo.FileName} was executed successfully";
                
                _logService.LogInformation(message);

                return response;
            }
            catch (System.Exception ex)
            {
                _logService.LogInformation(ex.Message);

                response.ErrorMessage = ex.Message;

                return response;
            }
        }
    }
}
