using System.ComponentModel.DataAnnotations;

namespace WebsiteDataCounter.Models.WebsiteCouner
{
    public class WebsiteData
    {
        [Required]
        public string WebsiteURL { get; set; }
       
    }
}