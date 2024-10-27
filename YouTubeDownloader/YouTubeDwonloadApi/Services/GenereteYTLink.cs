namespace YouTubeDwonloadApi.Services
{
    public class GenereteYTLink
    {
        public static string? LinkVideo(string genLinkPath,string url)
        {
            CmdCommand.Run(genLinkPath,url,out string output,out string error);

            if (error != null)
            {
                return output;
            }
            return error;
        }
    }
}
