using KodikDownloader;



var kodikClient = new KodikClient();
var m3u8Downloader = new M3U8Downloader("ffmpeg.exe");

var links = await kodikClient.GetLinksAsync("//kodik.info/seria/1256743/409f5450e2e617cd2ceb30f030796e79/720p");

if (links == null)
    return;

var memoryStream = m3u8Downloader.DownloadByManifest(links.LinksByQuality.First().Value.First().Link);

using (var stream = new FileStream($"download/{Guid.NewGuid()}.mp4",FileMode.Create))
{
    memoryStream.CopyTo(stream);
}





