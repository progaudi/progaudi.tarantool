using System;
using JetBrains.Annotations;

namespace ProGaudi.Tarantool.Client.Model
{
    public class TarantoolNode
    {
        private TarantoolNode([NotNull] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(nameof(url));
            }
            
            var parts = url.Split("@", StringSplitOptions.RemoveEmptyEntries);
            string port;
            if (parts.Length == 1)
            {
                (Host, port) = ParseByColon(parts[0]);
            }
            else
            {
                (Host, port) = ParseByColon(parts[1]);
                (User, Pwd) = ParseByColon(parts[0]);
            }

            Port = int.Parse(port);

            (string, string) ParseByColon(string s)
            {
                var x = s.Split(":", StringSplitOptions.RemoveEmptyEntries);

                return x.Length == 1 ? (x[0], null) : (x[0], x[1]);
            }
        }

        public string Host { get; }
        public int Port { get; }
        public string User { get; }
        public string Pwd { get; }

        public static TarantoolNode TryParse(string url, ILog log)
        {
            try
            {
                return new TarantoolNode(url);
            }
            catch (Exception e)
            {
                log?.WriteLine($"Url parsing failed. Url: {url}, error: {e.Message}");
                return null;
            }
        }
    }
}