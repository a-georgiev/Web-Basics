﻿using BasicWebServer.Server.Common;

namespace BasicWebServer.Server.HTTP
{
    public class Header
    {
        public const string ContentType = "Content-Type";
        public const string ContentLength = "Content-Length";
        public const string ContentDisposition = "Content-Disposition";
        public const string Date = "Date";
        public const string Location = "Location";
        public const string Server = "Server";

        public Header(string name, string value)
        {
            Guard.AginstNull(name, nameof(name));
            Guard.AginstNull(value, nameof(value));

            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString() => $"{this.Name}: {this.Value}";
            
    }
}
