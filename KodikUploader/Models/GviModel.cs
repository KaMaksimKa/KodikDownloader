using System.Text.Json.Serialization;

namespace KodikUploader.Models
{
    public class GviModel
    {
        [JsonPropertyName("d")]
        public string? D { get; set; }

        [JsonPropertyName("d_sign")]
        public string? DSign { get; set; }

        [JsonPropertyName("pd")]
        public string? Pd { get; set;}

        [JsonPropertyName("pd_sign")]
        public string? PdSign { get; set;}

        [JsonPropertyName("ref")]
        public string? Ref { get; set;}

        [JsonPropertyName("ref_sign")]
        public string? RefSign { get; set;}

        [JsonPropertyName("bad_user")]
        public bool? BadUser { get; set;}

        [JsonPropertyName("type")]
        public string? Type { get; set;}

        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("info")]
        public object? Info { get; set; }


    }
}
