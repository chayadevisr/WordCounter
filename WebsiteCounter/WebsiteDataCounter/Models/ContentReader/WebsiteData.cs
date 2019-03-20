using System.ComponentModel.DataAnnotations;

namespace WebsiteContentReader.Models.ContentReader
{
    public class WebsiteData
    {
        [Required]
        public string WebsiteURL { get; set; }
       
    }
}