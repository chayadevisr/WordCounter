using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using WebsiteContentReader.Models.ContentReader;

namespace WebsiteContentReader.Data.ContentReader
{
    public class WebsiteContentReaderRepository : IWebsiteContentReaderRepository
    {
        /// <summary>
        /// Get the website content from content API
        /// </summary>
        /// <param name="websiteURL"></param>
        /// <returns></returns>
        public WebsiteCountResult GetWebsiteContent(string websiteURL)
        {
            if (websiteURL.StartsWith("www"))
            {
                websiteURL = string.Concat("http://", websiteURL);
            }
            else
            {
                websiteURL = (Regex.IsMatch(websiteURL, "https?://.*")) ? websiteURL : string.Concat("http://", websiteURL);
            }
            WebsiteCountResult websiteContentCountResult = new WebsiteCountResult();
            websiteContentCountResult.WebsiteURL = websiteURL;

            var httpClientHandler = new HttpClientHandler();

            using (var client = new HttpClient(httpClientHandler))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.BaseAddress = new Uri("http://localhost:50220/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Anything");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout.Add(TimeSpan.FromSeconds(200));
                string url = string.Concat("api/GetWebContent/GetWebsiteContent");
                var text = "{ websiteURL : '" + websiteURL + "'}";

                var content = new StringContent(JObject.Parse(text).ToString(), Encoding.UTF8, "application/json");
                var portalResponse = client.PostAsync(url, content).Result;
                if (portalResponse.IsSuccessStatusCode)
                {
                    return portalResponse.Content.ReadAsAsync<WebsiteCountResult>().Result;
                }
                else
                {
                    throw new Exception("Error while fecting Owner data from API" + portalResponse.Content.ReadAsStringAsync().Result);
                }
            }
        }
    }
}