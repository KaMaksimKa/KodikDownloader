using System.Text.Json.Serialization;

namespace KodikDownloader.Models
{
    public class KodikLinks
    {
        [JsonPropertyName("links")]
        public Dictionary<string, KodikLinkInfo[]> LinksByQuality { get; init; } = null!;
    }
}
