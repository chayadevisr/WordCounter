using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteContentReader.Models.ContentReader
{
    public class WebsiteCountResult
    {
        
        [JsonProperty("websiteURL")]
        public string WebsiteURL { get; set; }

        [JsonProperty("ImageUrl")]
        public List<string> ImageUrl { get; set; }

        [JsonProperty("WordCount")]
        public int WordCount { get; set; }

        [JsonProperty("TopWordCount")]
        public Dictionary<string, int> TopWordCount { get; set; }
    }
}