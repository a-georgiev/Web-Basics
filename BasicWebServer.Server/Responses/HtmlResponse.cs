﻿namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    using System;

    public class HtmlResponse: ContentRespopnse
    {
        public HtmlResponse(string text, Action<Request, Response> preRenderAction = null)
            :base(text, ContentType.Html,preRenderAction)
        {

        }
    }
}
