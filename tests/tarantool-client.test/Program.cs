using System;
using iproto.Data.Packets;
using iproto.Services;
using TarantoolDnx.MsgPack.Converters;

namespace tarantool_client.test
{
    class Program
    {
        static void Main(string[] args)
        {
            var tarantoolClinet = new AsyncTarantoolClient();
            tarantoolClinet.Connect("192.168.99.100", 3301);
            tarantoolClinet.Login("test", "test");

            //var requestFactory = new RequestFactory();
            //var salt = Convert.FromBase64String("DCHe8DF5IQKb8ZphIRjOxQlMiLjooLtazaUh+SPzXi0=");
            //var packet = requestFactory.CreateAuthentication(new GreetingsPacket(string.Empty, salt), "test", "test");
            //var msgPackContext = MsgPackContextFactory.Create();
            //var serialzied = packet.Serialize(msgPackContext);

            //var sbytes = new sbyte[]
            //{
            //    43,
            //    -127, 0, 7,
            //    -126, 33, -110, -87, 99, 104, 97, 112, 45, 115, 104, 97, 49, -76, -44, -30, 16, 98, -118, -88, -70, -77, -91, 37, -80, 2, 114, -39, 19, -41, 79, 100, -99, 12, 35, -92, 116, 101, 115, 116
            //};
            //var bytes = new byte[sbytes.Length];
            //for (int i = 0; i < sbytes.Length; i++)
            //{
            //    bytes[i] = (byte)sbytes[i];
            //}

            //var deserialized = MsgPackConverter.Deserialize<object>(bytes, msgPackContext);

           
        }
    }
}
