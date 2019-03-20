using ContentApi.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace ContentApi.Controllers
{
    public class GetWebContentController : ApiController
    {
        public Dictionary<string, int> topWordCountList = new Dictionary<string, int>();

        /// <summary>
        /// Get the website details 
        /// </summary>
        /// <param name="website">WebAddress</param>
        /// <returns>WebContentDetails</returns>
        [HttpPost]
        [Route("api/GetWebContent/GetWebsiteContent")]
        public WebContentDetails GetWebsiteContent([FromBody] WebAddress website)
        {
            WebContentDetails websiteContentCountResult = new WebContentDetails();
            websiteContentCountResult.WebsiteURL = website.websiteURL;
            DownloadWebsiteContent(website.websiteURL, websiteContentCountResult);

            return websiteContentCountResult;
        }

        /// <summary>
        /// Download the file and save as html to get and process the file
        /// </summary>
        /// <param name="websiteURL">string</param>
        /// <param name="websiteContentCountResult">WebContentDetails</param>
        private void DownloadWebsiteContent(string websiteURL, WebContentDetails websiteContentCountResult)
        {
            if (!string.IsNullOrEmpty(websiteURL))
            {
                string fileName = Regex.Replace(websiteURL, @"[^0-9a-zA-Z]+", "");
                string path = @"D:\" + fileName.Trim() + @".html";
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.DownloadFile(websiteURL, path);
                }
                ProcessContent(path, websiteContentCountResult);
            }
        }

        /// <summary>
        /// Process the file to get the list of image and number of words
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="websiteContentCountResult"></param>
        private void ProcessContent(string filePath, WebContentDetails websiteContentCountResult)
        {
            string fileContent = File.ReadAllText(filePath);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(fileContent);
            HtmlNode bodyHtml = doc.DocumentNode.SelectSingleNode("//body");
            string content = bodyHtml.InnerHtml;
            int wordCount = 0;
            List<string> ImageUrlList = new List<string>();
            if (!string.IsNullOrEmpty(content))
            {

                ExtractImages(ImageUrlList, content);
                wordCount = CountWords(content);

            }
            websiteContentCountResult.ImageUrl = ImageUrlList;
            websiteContentCountResult.WordCount = wordCount;
            websiteContentCountResult.TopWordCount = topWordCountList;
        }

        /// <summary>
        /// Count the words present in the string
        /// </summary>
        /// <param name="txtToCount">string</param>
        /// <returns>int</returns>
        private int CountWords(string txtToCount)
        {
            string pattern = @"(?<!<[^>]*)";
            Regex regex = new Regex(pattern);
            int count = 0;
            txtToCount.Trim();

            //remove html tag from the line
            txtToCount = Regex.Replace(txtToCount, @"<[^>]*>", "");

            //remove special char except space,-, Underscore
            txtToCount = Regex.Replace(txtToCount, @"[ ](?=[ ])|[^-_A-Za-z0-9 ]+", "");

            txtToCount.Trim();
            string[] wordList = txtToCount.Split(' ');
            if (!string.IsNullOrEmpty(txtToCount))
            {
                foreach (string word in wordList)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        count++;
                        if (topWordCountList.ContainsKey(word))
                        {
                            int value = topWordCountList[word];
                            topWordCountList[word] = value + 1;
                        }
                        else
                        {
                            topWordCountList.Add(word, 1);
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Extracts the image listed
        /// </summary>
        /// <param name="ImageUrlList">List(string)</param>
        /// <param name="line">string </param>
        private static void ExtractImages(List<string> ImageUrlList, string line)
        {
            foreach (Match m in Regex.Matches(line, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                string src = m.Groups[1].Value;
                if (src.StartsWith(@"/"))
                {
                    src = "http:" + src;
                }
                ImageUrlList.Add(src);
            }
        }
    }
}

