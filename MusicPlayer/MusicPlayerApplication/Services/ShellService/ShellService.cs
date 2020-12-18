using Microsoft.Extensions.Options;
using MusicPlayerApplication.Models;
using MusicPlayerApplication.Settings;
using System.Diagnostics;

namespace MusicPlayerApplication.Services.ShellService
{
    public class ShellService : IShellService
    {
        readonly IOptions<ShellSettings> _shellSettings;
        public ShellService(IOptions<ShellSettings> shellSettings)
        {
            _shellSettings = shellSettings;
        }
        public ResponseModel Run(string cmd)
        {
            var response = new ResponseModel
            {
                HasError = true
            };

            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _shellSettings.Value.TerminalPath,
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            try
            {
                var isStarted = process.Start();
                if(!isStarted)
                {
                    var errorMessage = "Process not started !";
                    System.Console.WriteLine(errorMessage);
                    response.ErrorMessage = errorMessage;
                    return response;
                }
                process.WaitForExit();
                process.Close();
                response.HasError = false;
                return response;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
    }
}
