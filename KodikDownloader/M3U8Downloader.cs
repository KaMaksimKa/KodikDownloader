using KodikDownloader.Interfaces;
using System.Diagnostics;

namespace KodikDownloader
{
    public class M3U8Downloader : IM3U8Downloader
    {
        private readonly string _ffmpegPath;
        public M3U8Downloader(string ffmpegPath)
        {
            _ffmpegPath = ffmpegPath;
        }

        public void DownloadByManifest(string manifestUrl, string outputPath)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = _ffmpegPath;

                process.StartInfo.Arguments = $"-i {manifestUrl} -c copy -bsf:a aac_adtstoasc {outputPath}";

                process.StartInfo.WorkingDirectory = ".";

                process.Start();

                process.WaitForExit();
            }
        }

        public Stream DownloadByManifest(string manifestLink)
        {
            var tempFolderPath = Path.GetTempPath();
            var tempFileName = $"{Guid.NewGuid()}.mp4";
            var tempFilePath = Path.Combine(tempFolderPath, tempFileName);

            DownloadByManifest(manifestLink, tempFilePath);

            var memoryStream = new MemoryStream();

            var fileInfo = new FileInfo(tempFilePath);

            if (fileInfo.Exists)
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    fileStream.CopyTo(memoryStream);
                }
                fileInfo.Delete();
            }
            else
            {
                throw new InvalidOperationException("Ошибка при скачивании файла, возможно указан неверный путь до ffmpeg");
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
