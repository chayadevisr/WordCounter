using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using WebsiteContentReader.Models.ContentReader;

namespace WebsiteContentReader.Data.ContentReader
{
    public class CachedWebsiteContentReaderRepository : IWebsiteContentReaderRepository
    {
        /// <summary>
        /// Gets the Cached Website Content Counter Repository.
        /// </summary>
        public IWebsiteContentReaderRepository WebsiteContentReaderRepository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedWebsiteContentReaderRepository"/> class.
        /// </summary>
        public CachedWebsiteContentReaderRepository() : this(new WebsiteContentReaderRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedWebsiteContentReaderRepository"/> class.
        /// </summary>
        /// <param name="WebsiteContentReaderRepository">The Cached Website Content Counter Repository.</param>
        public CachedWebsiteContentReaderRepository(IWebsiteContentReaderRepository WebsiteContentReaderRepository)
        {
            this.WebsiteContentReaderRepository = WebsiteContentReaderRepository;
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
                WebsiteCountResult websiteCountResult = this.WebsiteContentReaderRepository.GetWebsiteContent(websiteUrl);
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