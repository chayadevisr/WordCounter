using WebsiteContentReader.Models.ContentReader;

namespace WebsiteContentReader.Data.ContentReader
{
    public interface IWebsiteContentReaderRepository
    {
        WebsiteCountResult GetWebsiteContent(string websiteURL);
    }
}