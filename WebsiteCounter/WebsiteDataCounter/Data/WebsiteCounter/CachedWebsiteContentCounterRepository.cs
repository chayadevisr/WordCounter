using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Helpers;
using WebsiteDataCounter.Models.WebsiteCouner;


namespace WebsiteDataCounter.Data.WebsiteCounter
{
    public class CachedWebsiteContentCounterRepository : IWebsiteContentCounterRepository
    {
        /// <summary>
        /// Gets the Cached Website Content Counter Repository.
        /// </summary>
        public IWebsiteContentCounterRepository WebsiteContentCounterRepository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedWebsiteContentCounterRepository"/> class.
        /// </summary>
        public CachedWebsiteContentCounterRepository() : this(new WebsiteContentCounterRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedWebsiteContentCounterRepository"/> class.
        /// </summary>
        /// <param name="WebsiteContentCounterRepository">The Cached Website Content Counter Repository.</param>
        public CachedWebsiteContentCounterRepository(IWebsiteContentCounterRepository WebsiteContentCounterRepository)
        {
            this.WebsiteContentCounterRepository = WebsiteContentCounterRepository;
        }

        /// <summary>
        /// Gets the Website Content
        /// </summary>
        /// <returns>WebsiteCountResult</returns>
        public WebsiteCountResult GetWebsiteContent(string websiteUrl)
        {
            string key = Regex.Replace(websiteUrl, @"[^0-9a-zA-Z]+", "");
            key = key.Trim();
            if (HttpContext.Current?.Cache[key] == null)
            {
                WebsiteCountResult websiteCountResult = this.WebsiteContentCounterRepository.GetWebsiteContent(websiteUrl);
                if (websiteCountResult != null)
                {                  
                    HttpContext.Current?.Cache.Insert(key, websiteCountResult, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);                  
                }

                return websiteCountResult;
            }
            return HttpContext.Current?.Cache[key] as WebsiteCountResult;

        }
    }
}