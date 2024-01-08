using System.Text.Json.Serialization;

namespace KodikDownloader.Models
{
    public class UrlParamsKodik
    {
        [JsonPropertyName("d")]
        public string D { get; init; } = null!;

        [JsonPropertyName("d_sign")]
        public string DSign { get; init; } = null!;

        [JsonPropertyName("pd")]
        public string Pd { get; init; } = null!;

        [JsonPropertyName("pd_sign")]
        public string PdSign { get; init; } = null!;

        [JsonPropertyName("ref")]
        public string Ref { get; init; } = null!;

        [JsonPropertyName("ref_sign")]
        public string RefSign { get; init; } = null!;
    }
}
