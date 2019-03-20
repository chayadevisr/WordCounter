using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentApi.Models
{
    public class WebAddress
    {
        [JsonProperty("websiteURL")]
        public string websiteURL { get; set; }
    }
}