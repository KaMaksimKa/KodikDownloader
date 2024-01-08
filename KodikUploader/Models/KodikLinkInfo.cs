using KodikDownloader.Utils;
using System.Text.Json.Serialization;

namespace KodikDownloader.Models
{
    public class KodikLinkInfo
    {
        [JsonPropertyName("src")]
        public string Src { get; init; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; init; } = null!;

        public string Link => "https:" + StringUtils.Atob(StringUtils.ProcessingGviSrc(Src));
    }
}
