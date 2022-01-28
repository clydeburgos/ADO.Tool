using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ADO.Tool.DTOs;

namespace ADO.Tool.Extension
{
    static internal class HttpRequestExtension
    {
        public static async Task<string> SendRequest<T>(HttpRequestTypes type, T payload, string personalAccessToken, string baseADOUrl, string orgUrl, string suffixUrl)
        {
            using (var client = new HttpClient())
            {
                string apiUrl = $"{baseADOUrl}/{orgUrl}/{suffixUrl}";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalAccessToken))));

                var payloadToJson = new StringContent(
                    JsonSerializer.Serialize<T>(payload, null),
                    Encoding.UTF8,
                    "application/json");

                if (type == HttpRequestTypes.GET)
                {
                    using var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else if (type == HttpRequestTypes.POST)
                {
                    using var response = await client.PostAsync(apiUrl, payloadToJson);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else if (type == HttpRequestTypes.PUT)
                {
                    using var response = await client.PutAsync(apiUrl, payloadToJson);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                return string.Empty;
            }
        }
    }
}
