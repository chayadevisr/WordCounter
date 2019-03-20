using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebsiteDataCounter.Data.WebsiteCounter;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using WebsiteDataCounter.Models.WebsiteCouner;
using System;

namespace WebsiteDataCounter.Controllers.WebsiteCounter
{
    public class WebsiteContentCounterController : Controller
    {
        /// <summary>
        /// Gets the Book Repository
        /// </summary>
        public IWebsiteContentCounterRepository WebsiteContentCounterRepository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteCounterController"/> class.
        /// </summary>
        public WebsiteContentCounterController() : this(new CachedWebsiteContentCounterRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteContentCounterController"/> class.
        /// </summary>
        /// <param name="WebsiteContentCounterRepository">The Book Repository.</param>
        public WebsiteContentCounterController(IWebsiteContentCounterRepository WebsiteContentCounterRepository)
        {
            this.WebsiteContentCounterRepository = WebsiteContentCounterRepository;
        }


        public ActionResult WebsiteData()

        {
            return View("~/Views/WebsiteContentCounter/WebsiteData.cshtml");
        }

       
        public ActionResult WebsiteContentCountResult(WebsiteData websiteData)
        {
            if (ModelState.IsValid)
            {
                //Returns action to be executed on successfull completion of call
                return View("~/Views/WebsiteContentCounter/WebsiteContentCountResult.cshtml",
                    this.WebsiteContentCounterRepository.GetWebsiteContent(websiteData.WebsiteURL));
            }
            ModelState.AddModelError("WebsiteURL","please enter website url");
            return View("~/Views/WebsiteContentCounter/WebsiteData.cshtml",websiteData);

        }

        public ActionResult DisplayGraph(string websiteUrl)
        {
            Dictionary<string, int> topWordList = this.WebsiteContentCounterRepository.GetWebsiteContent(websiteUrl).TopWordCount;
            List<string> xVal = new List<string>();
            List<string> yVal = new List<string>();
            var sortedDict = (from entry in topWordList
                              orderby entry.Value descending
                              select entry
                  ).Take(10)
                  .ToDictionary(pair => pair.Key, pair => pair.Value);
            Dictionary<string, int>.KeyCollection keys = sortedDict.Keys;

            foreach (var word in keys)
            {
                xVal.Add(word);
                yVal.Add(topWordList[word].ToString());
            }

            var wordCountChart = new Chart(width: 800, height: 400, themePath : "~/Content/theme.xml")
                .AddTitle("Top 10 word count in the website")
                .AddSeries(
                    name: "Top 10 word count",
                    xValue: xVal,
                    yValues: yVal
                    );
            return File(wordCountChart.ToWebImage().GetBytes(), "image/jpeg");
        }
    }
}
