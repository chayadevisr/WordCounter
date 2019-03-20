using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentApi.Models
{
    public class WebContentDetails
    {
        public string WebsiteURL { get; set; }
        public List<string> ImageUrl { get; set; }
        public int WordCount { get; set; }
        public Dictionary<string, int> TopWordCount { get; set; }
    }
}