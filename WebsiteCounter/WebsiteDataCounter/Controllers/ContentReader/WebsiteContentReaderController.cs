using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Web.Mvc;
using WebsiteContentReader.Data.ContentReader;
using WebsiteContentReader.Models.ContentReader;

namespace WebsiteContentReader.Controllers.ContentReader
{
    public class WebsiteContentReaderController : Controller
    {
        /// <summary>
        /// Gets the Book Repository
        /// </summary>
        public IWebsiteContentReaderRepository WebsiteContentReaderRepository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteCounterController"/> class.
        /// </summary>
        public WebsiteContentReaderController() : this(new CachedWebsiteContentReaderRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteContentReaderController"/> class.
        /// </summary>
        /// <param name="WebsiteContentReaderRepository">The Book Repository.</param>
        public WebsiteContentReaderController(IWebsiteContentReaderRepository WebsiteContentReaderRepository)
        {
            this.WebsiteContentReaderRepository = WebsiteContentReaderRepository;
        }

        /// <summary>
        /// Redirect to view for geting website url to be processed
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult WebsiteData()
        {
            return View("~/Views/WebsiteContentReader/WebsiteData.cshtml");
        }

        /// <summary>
        /// Getst the View for the display the image carousel & word count for the website
        /// </summary>
        /// <param name="websiteData"></param>
        /// <returns></returns>
        public ActionResult WebsiteContentCountResult(WebsiteData websiteData)
        {
            Uri uriResult;
            bool validUrl = ValidateURL(websiteData.WebsiteURL, out uriResult);
            if (ModelState.IsValid && validUrl && checkWebsiteExists(uriResult.AbsoluteUri))
            {
                //Returns action to be executed on successfull completion of call
                return View("~/Views/WebsiteContentReader/WebsiteContentCountResult.cshtml",
                    this.WebsiteContentReaderRepository.GetWebsiteContent(uriResult.AbsoluteUri));
            }

            //Returns the validation message 
            ModelState.AddModelError("WebsiteURL", "please enter website url");
            return View("~/Views/WebsiteContentReader/WebsiteData.cshtml", websiteData);
        }

        /// <summary>
        /// Validates if website exists
        /// </summary>
        /// <param name="websiteUrl"></param>
        /// <returns></returns>
        public bool checkWebsiteExists(string websiteUrl)
        {
            try
            {
                WebClient wc = new WebClient();
                string HTMLSource = wc.DownloadString(websiteUrl);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validate the url entered
        /// </summary>
        /// <param name="url">string</param>
        /// <param name="resultURI">Uri</param>
        /// <returns>bool</returns>
        public bool ValidateURL(string url, out Uri resultURI)
        {
            if (!Regex.IsMatch(url, @"^https?:\/\/", RegexOptions.IgnoreCase))
                url = "http://" + url;

            if (Uri.TryCreate(url, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }

        /// <summary>
        /// Creates the graph for top 10 words for the give website url
        /// </summary>
        /// <param name="websiteUrl">string</param>
        /// <returns>ActionResult</returns>
        public ActionResult DisplayGraph(string websiteUrl)
        {
            if (!string.IsNullOrEmpty(websiteUrl)) { }
            Dictionary<string, int> topWordList = !string.IsNullOrEmpty(websiteUrl)?
                this.WebsiteContentReaderRepository.GetWebsiteContent(websiteUrl).TopWordCount 
                : new Dictionary<string, int>();
            List<string> xVal = new List<string>();
            List<string> yVal = new List<string>();
            var sortedDict = (from entry in topWordList
                              orderby entry.Value descending
                              select entry
                  ).Take(10)
                  .ToDictionary(pair => pair.Key, pair => pair.Value);

            //Forms X & Y axis values to be displayed in the chart
            Dictionary<string, int>.KeyCollection keys = sortedDict.Keys;

            foreach (var word in keys)
            {
                xVal.Add(word);
                yVal.Add(topWordList[word].ToString());
            }


            //Creates the chart with x & y axis values
            var wordCountChart = new Chart(width: 800, height: 400, themePath: "~/Content/theme.xml")
                .AddTitle("Top 10 word count in the website")
                .AddSeries(
                    name: "Top 10 word count",
                    xValue: xVal,
                    yValues: yVal
                    );

            //return the file with chart image in image/jpeg format.
            return File(wordCountChart.ToWebImage().GetBytes(), "image/jpeg");
        }
    }
}
