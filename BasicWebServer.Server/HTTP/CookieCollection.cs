
namespace BasicWebServer.Server.HTTP
{
    using System.Collections;
    using System.Collections.Generic;
    public class CookieCollection : IEnumerable<Cookie>
    {
        private readonly Dictionary<string, Cookie> cookies;

<<<<<<< HEAD
        public CookieCollection()
            => this.cookies = new Dictionary<string, Cookie>();
=======
>>>>>>> 9ab3ad6c9aac44e89f6411261d295ffe007585f3

        public string this[string name]
                => this.cookies[name].Value;

        public void Add(string name, string value)
            => this.cookies[name] = new Cookie(name, value);

        public bool Contains(string name)
            => this.cookies.ContainsKey(name);

        public IEnumerator<Cookie> GetEnumerator()
            => this.cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
