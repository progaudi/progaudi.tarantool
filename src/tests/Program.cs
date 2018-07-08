using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace tests
{
    internal class Program
    {
        private static void Main() => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Loopback, 10000);
                var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    Blocking = false,
                    NoDelay = true
                };
                var args = new SocketAsyncEventArgs
                {
                    RemoteEndPoint = endpoint
                };
                var c = new AwaitableSocket(args, socket);
                await c.ConnectAsync(CancellationToken.None);
                args.BufferList = new[]
                {
                    new ArraySegment<byte>(Encoding.ASCII.GetBytes("Hello, world!\n")),
                    new ArraySegment<byte>(new byte[20])
                };

                await c.SendAsync();
                await c.ReceiveAsync();

                Console.WriteLine(Encoding.ASCII.GetString(args.BufferList[0]));
                Console.WriteLine(Encoding.ASCII.GetString(args.BufferList[1]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
