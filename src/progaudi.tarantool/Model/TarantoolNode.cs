using System;
using JetBrains.Annotations;

namespace ProGaudi.Tarantool.Client.Model
{
    public class TarantoolNode
    {
        public TarantoolNode()
        {
        }

        private TarantoolNode([NotNull] string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("Value cannot be null or empty.", nameof(url));

            if (!url.StartsWith("tarantool://"))
                url = "tarantool://" + url;

            Uri = new UriBuilder(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        public UriBuilder Uri { get; }

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