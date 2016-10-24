using System.Net;

namespace ProGaudi.Tarantool.Client.Model
{
    public class NodeOptions
    {
        public EndPoint EndPoint { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool GuestMode { get; set; } = true;
    }
}