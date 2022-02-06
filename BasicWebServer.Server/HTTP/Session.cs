using BasicWebServer.Server.Common;
using System.Collections.Generic;

namespace BasicWebServer.Server.HTTP
{
    public class Session
    {
        public const string SessionCookieName = "MyWebServerID";
        public const string SessionCurrentDateKey = "CurrentDate";
        public const string SessionUserKey = "AuthenticationUserId";

        private Dictionary<string, string> data;
        public Session(string id)
        {
            Guard.AginstNull(id);

            this.ID = id;

            this.data = new Dictionary<string, string>();
        }

        public string ID { get; init; }

        public string this[string key]
        {
            get => this.data[key];
            set => this.data[key] = value;
        }
        public void Clear() => this.data.Clear();
        public bool ContainsKey(string key) => this.data.ContainsKey(key);
    }
}
