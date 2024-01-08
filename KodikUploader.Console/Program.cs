using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using KodikUploader.Models;
using KodikUploader.Utils;
using FFmpeg.AutoGen;
using MediaFileProcessor.Models.Common;
using MediaFileProcessor.Models.Enums;
using MediaFileProcessor.Processors;



using var client = new HttpClient();

var url = "https:" + @"//kodik.info/seria/907435/91d2a742b1ce92941bc19769eee7fefb/720p";

using var requestKodik = new HttpRequestMessage(HttpMethod.Get, url);

using var responseKodik = await client.SendAsync(requestKodik);

var textKodik = await responseKodik.Content.ReadAsStringAsync();

var textKodikWords = textKodik.Split().ToList();

var type = textKodikWords[textKodikWords.IndexOf("videoInfo.type") + 2].Trim('"', ';', '\'');
var hash = textKodikWords[textKodikWords.IndexOf("videoInfo.hash") + 2].Trim('"', ';', '\'');
var id = textKodikWords[textKodikWords.IndexOf("videoInfo.id") + 2].Trim('"', ';', '\'');
var info = @"{""advImps"":{}}";
var badUser = "true";

var urlParamsString = textKodikWords[textKodikWords.IndexOf("urlParams") + 2].Trim('\'', ';', '\'');
var urlParams = JsonSerializer.Deserialize<UrlParamsKodik>(urlParamsString);



using var requestDvi = new HttpRequestMessage(HttpMethod.Post, @"https://kodik.info/gvi");
requestDvi.Headers.Add("x-requested-with", "XMLHttpRequest");



var formModel = new Dictionary<string, string?>()
{
    ["d"] = urlParams?.D,
    ["d_sign"] = urlParams?.DSign,
    ["pd"] = urlParams?.Pd,
    ["pd_sign"] = urlParams?.PdSign,
    ["ref"] = urlParams?.Ref,
    ["ref_sign"] = urlParams?.RefSign,
    ["bad_user"] = badUser,
    ["type"] = type,
    ["hash"] = hash,
    ["id"] = id,
    ["info"] = info

};


using var formContent = new FormUrlEncodedContent(formModel);

requestDvi.Content = formContent;

using var responseDvi = await client.SendAsync(requestDvi);

var kodikLinks = await JsonSerializer.DeserializeAsync<KodikLinks>(responseDvi.Content.ReadAsStream());


if (kodikLinks == null)
    return;

foreach (var (quality, linkInfos) in kodikLinks.LinksByQuality)
{
    foreach (var linkInfo in linkInfos)
    {
        using var manifestRequest = new HttpRequestMessage(HttpMethod.Get, linkInfo.Link);

        using var manifestResponse = await client.SendAsync(manifestRequest);

        var manifestText = await manifestResponse.Content.ReadAsStringAsync();
        var manifestSplited = manifestText.Split('\n');


        for (var i = 0; i < manifestSplited.Length; i++)
        {
            if (!manifestSplited[i].Contains("#EXTINF"))
            {
                continue;
            }

            i++;

            var segmentVideoLink = linkInfo.Link.Substring(0, linkInfo.Link.LastIndexOf("/")) + manifestSplited[i].Substring(1);

            using var segmentVideoRequest = new HttpRequestMessage(HttpMethod.Get, segmentVideoLink);
            using var segmentVideoResponse = await client.SendAsync(segmentVideoRequest);

            using (var fileWriter = new FileStream($"./{i}.ts",FileMode.Create))
            {
                await segmentVideoResponse.Content.CopyToAsync(fileWriter);
            }

            

            if (i > 10)
            {
                break;
            }
        }


 
        break;
    }

    break;
}


class UrlParamsKodik
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

class KodikLinks
{
    [JsonPropertyName("links")]
    public Dictionary<string, KodikLinkInfo[]> LinksByQuality { get; init; } = null!;
}

class KodikLinkInfo
{
    [JsonPropertyName("src")]
    public string Src { get; init; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; init; } = null!;

    public string Link => "https:" + StringUtils.Atob(StringUtils.ProcessingGviSrc(Src));
}