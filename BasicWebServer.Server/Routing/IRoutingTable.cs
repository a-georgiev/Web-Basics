﻿
namespace BasicWebServer.Server.Routing
{
    using System;
    using BasicWebServer.Server.HTTP;

    public interface IRoutingTable
    {
        IRoutingTable Map(Method method, string path, Func<Request, Response> responseFunction);

        IRoutingTable MapGet(string path, Func<Request, Response> responseFunction);

        IRoutingTable MapPost(string url, Func<Request, Response> responseFunction);
    }
}
