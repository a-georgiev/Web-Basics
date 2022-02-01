namespace BasicWebServer.Server.HTTP
{
    using BasicWebServer.Server.Common;

    public class Cookie
    {
        public Cookie(string name, string value)
        {
            Guard.AginstNull(name, nameof(name));
            Guard.AginstNull(value, nameof(value));

            this.Name = name;
            this.Value = value;
        }

        public string Name { get; init; }
        public string Value { get; init; }

        public override string ToString()
            => $"{this.Name}={this.Value}";

    }
}
