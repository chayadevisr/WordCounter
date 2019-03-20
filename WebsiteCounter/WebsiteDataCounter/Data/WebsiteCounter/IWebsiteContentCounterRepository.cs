using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using WebsiteDataCounter.Models.WebsiteCouner;

namespace WebsiteDataCounter.Data.WebsiteCounter
{
    public interface IWebsiteContentCounterRepository
    {
        WebsiteCountResult GetWebsiteContent(string websiteURL);
    }
}