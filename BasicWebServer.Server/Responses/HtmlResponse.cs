namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    using System;

    public class HtmlResponse: ContentRespopnse
    {
        public HtmlResponse(string text, Action<Request, Response> preRenderAction = null)
<<<<<<< HEAD
            :base(text, ContentType.Html,preRenderAction)
=======
            :base(text, ContentType.Html)
>>>>>>> 9ab3ad6c9aac44e89f6411261d295ffe007585f3
        {

        }
    }
}
