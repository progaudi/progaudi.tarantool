using System;

namespace tarantool_client.test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tarantoolClinet = new AsyncTarantoolClient();
            tarantoolClinet.Connect("192.168.99.100", 3301);
            var response = tarantoolClinet.Login("test", "test");
            Console.WriteLine(response.ErrorMessage);
        }
    }
}
