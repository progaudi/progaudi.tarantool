using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

namespace tarantool_client.test
{
    public class Program
    {
        private const int spaceId = 514;
        public static void Main(string[] args)
        {
            ConvertJava2CSharp();
            var tarantoolClient = new Connection();
            tarantoolClient.Connect("192.168.99.100", 3301);
            var response = tarantoolClient.Login("operator", "operator");

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                System.Console.WriteLine("An error occured in login process:");
                System.Console.WriteLine(response.ErrorMessage);
                System.Console.WriteLine("Exiting...");
                return;
            }

            var schema = tarantoolClient.GetSchema();

            var insertRequest = new InsertReplacePacket<Tuple<int, string>>(CommandCode.Insert, spaceId, Tuple.Create(2, "Music"));
            var insertResponse = tarantoolClient.SendPacket(insertRequest);

            var deleteRequest = new DeletePacket<Tuple<int>>(spaceId, 0, Tuple.Create(2));
            var deleteResponse = tarantoolClient.SendPacket(deleteRequest);

            insertResponse = tarantoolClient.SendPacket(insertRequest);

            var selectRequest = new SelectPacket<Tuple<int>>(spaceId, 0, 100, 0, Iterator.All, Tuple.Create(2));
            var selectResponse = tarantoolClient.SendPacket(selectRequest);

            var replaceRequest = new InsertReplacePacket<Tuple<int, string, int>>(CommandCode.Replace, spaceId,
                Tuple.Create(2, "Orange", 5));
            var replaceResponse = tarantoolClient.SendPacket(replaceRequest);

            var udateRequest = new UpdatePacket<Tuple<int>, int>(spaceId, 0, Tuple.Create(2),
                UpdateOperation<int>.CreateAddition(1, 2));
            var updateResponse = tarantoolClient.SendPacket(udateRequest);

            selectResponse = tarantoolClient.SendPacket(selectRequest);

            var upsertRequest = new UpsertPacket<Tuple<int, int>, int>(spaceId, Tuple.Create(5, 20),
                UpdateOperation<int>.CreateAddition(10, 1));
            var upsertResponse = tarantoolClient.SendPacket(upsertRequest);

            selectResponse = tarantoolClient.SendPacket(selectRequest);

            var callRequest = new CallPacket<Tuple<float>>("math.sqrt", Tuple.Create(1.3f));
            var callResponse = tarantoolClient.SendPacket(callRequest);

            var evalRequest = new EvalPacket<Tuple<int, int, int>>("return ...", Tuple.Create(1, 2, 3));
            var evalResponse = tarantoolClient.SendPacket(evalRequest);
        }

        public static void ConvertJava2CSharp()
        {
            var input = new sbyte[]
            {
                -50,
                0,
                0,
                0,
                17,
                -127,
                0,
                2,
                -126,
                16,
                -51,
                2,
                2,
                33,
                -110,
                2,
                -91,
                77,
                117,
                115,
                105,
                99,
            };
            var result = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (byte)input[i];
            }
        }
    }
}
