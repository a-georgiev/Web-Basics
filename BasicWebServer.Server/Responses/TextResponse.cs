using BasicWebServer.Server.HTTP;

namespace BasicWebServer.Server.Responses
{
    public class TextResponse: ContentRespopnse
    {
        public TextResponse(string text)
            :base(text, ContentType.PlainText)
        {

        }
    }
}
