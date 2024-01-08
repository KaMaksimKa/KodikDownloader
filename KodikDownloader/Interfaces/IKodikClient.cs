using KodikDownloader.Models;

namespace KodikDownloader.Interfaces
{
    public interface IKodikClient
    {
        Task<KodikLinks?> GetLinksAsync(string shareKodikLink);
    }
}
