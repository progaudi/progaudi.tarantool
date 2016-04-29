using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

namespace tarantool_client.test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tarantoolClinet = new Connection();
            tarantoolClinet.Connect("192.168.99.100", 3301);
            var response = tarantoolClinet.Login("operator", "operator");

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                Console.WriteLine("An error occured in login process:");
                Console.WriteLine(response.ErrorMessage);
                Console.WriteLine("Exiting...");
                return;
            }

            var selectRequest = new SelectPacket<iproto.Tuple<int>>(514, 0, 100, 0, Iterator.All, MyTuple.Create(2));
            var selectResponse = tarantoolClinet.SendPacket(selectRequest);
        }
    }
}
