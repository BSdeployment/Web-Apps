using System.Diagnostics;

namespace YouTubeDwonloadApi.Services
{
    public class CmdCommand
    {
        public static void Run(string filename,string url, out string output, out string error, string directory = null)
    {
        using Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = filename,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                Arguments =  url,
                //Arguments =  command,
                CreateNoWindow = false,
                WorkingDirectory = directory ?? string.Empty,
            }
        };
        process.Start();
        process.WaitForExit();
        output = process.StandardOutput.ReadToEnd();
        error = process.StandardError.ReadToEnd();
    }
    }
}
