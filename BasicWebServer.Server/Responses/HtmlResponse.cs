using BasicWebServer.Server.HTTP;

namespace BasicWebServer.Server.Responses
{
    public class HtmlResponse: ContentRespopnse
    {
        public HtmlResponse(string text)
            :base(text, ContentType.Html)
        {

        }
    }
}
