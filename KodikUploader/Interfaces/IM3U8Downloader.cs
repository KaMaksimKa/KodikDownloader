namespace KodikDownloader.Interfaces
{
    public interface IM3U8Downloader
    {
        void DownloadByManifest(string manifestLink, string outputPath);

        Stream DownloadByManifest(string manifestLink);
    }
}
