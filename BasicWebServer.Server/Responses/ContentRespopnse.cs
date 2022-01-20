using BasicWebServer.Server.Common;
using BasicWebServer.Server.HTTP;
using System.Text;

namespace BasicWebServer.Server.Responses
{
    public class ContentRespopnse: Response
    {
        public ContentRespopnse(string content, string contentType)
            :base(StatusCode.OK)
        {
            Guard.AginstNull(content);
            Guard.AginstNull(contentType);

            this.Headers.Add(Header.ContentType, contentType);
            this.Body = content;
        }

        public override string ToString()
        {
            if(this.Body != null)
            {
                var contentLength = Encoding.UTF8.GetByteCount(this.Body).ToString();
                this.Headers.Add(Header.ContentLength, contentLength);
            }

            return base.ToString();
        }
    }
}
