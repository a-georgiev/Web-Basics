using System;

namespace BasicWebServer.Server.Common
{
    public static class Guard
    {
        public static void AginstNull(object value, string name = null)
        {
            if(value==null)
            {
                name ??= "Value";
                throw new ArgumentException($"{name} cannot be null.");
            }

        }
    }
}
