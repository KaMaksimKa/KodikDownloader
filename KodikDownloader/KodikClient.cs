using KodikDownloader.Interfaces;
using KodikDownloader.Models;
using System.Text.Json;

namespace KodikDownloader
{
    public class KodikClient: IKodikClient
    {
        private readonly HttpClient _httpClient;
        private const string _kodikGviLink = @"https://kodik.info/gvi";

        public KodikClient()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// example shareKodikLink: //kodik.info/seria/1256743/409f5450e2e617cd2ceb30f030796e79/720p
        /// </summary>
        /// <param name="shareKodikLink">example: //kodik.info/seria/1256743/409f5450e2e617cd2ceb30f030796e79/720p </param>
        /// <returns></returns>
        public async Task<KodikLinks?> GetLinksAsync(string shareKodikLink)
        {
            var url = $"https:{shareKodikLink}";

            using var requestKodik = new HttpRequestMessage(HttpMethod.Get, url);

            using var responseKodik = await _httpClient.SendAsync(requestKodik);

            var textKodik = await responseKodik.Content.ReadAsStringAsync();

            var textKodikWords = textKodik.Split().ToList();

            var type = textKodikWords[textKodikWords.IndexOf("videoInfo.type") + 2].Trim('"', ';', '\'');
            var hash = textKodikWords[textKodikWords.IndexOf("videoInfo.hash") + 2].Trim('"', ';', '\'');
            var id = textKodikWords[textKodikWords.IndexOf("videoInfo.id") + 2].Trim('"', ';', '\'');
            var info = @"{""advImps"":{}}";
            var badUser = "true";

            var urlParamsString = textKodikWords[textKodikWords.IndexOf("urlParams") + 2].Trim('\'', ';', '\'');
            var urlParams = JsonSerializer.Deserialize<UrlParamsKodik>(urlParamsString);

            using var requestDvi = new HttpRequestMessage(HttpMethod.Post, _kodikGviLink);
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

            using var responseDvi = await _httpClient.SendAsync(requestDvi);
            var kodikLinks = await JsonSerializer.DeserializeAsync<KodikLinks>(responseDvi.Content.ReadAsStream());

            return kodikLinks;
        }
    }
}
