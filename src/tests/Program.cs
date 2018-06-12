using System;
using System.Linq;
using System.Text;
using MessagePack;
using MessagePack.Formatters;
using ProGaudi.MsgPack.Light;

using static MessagePack.MessagePackBinary;

namespace tests
{
    [MessagePackObject]
    [MsgPackArray]
    public struct A
    {
        [MsgPackArrayElement(0)]
        [Key("W")]
        public int V { get; set; }
    }

    public enum B
    {
        C,
        D
    }

    [Flags]
    public enum E
    {
        F1 = 0x1,
        F2 = 0x2,
        F3 = 0x4
    }

    public class CallRequest<T>
    {
        public CallRequest(string functionName, T tuple)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public T Tuple { get; }

        internal class Formatter : IMessagePackFormatter<CallRequest<T>>
        {
            public int Serialize(ref byte[] bytes, int offset, CallRequest<T> value, IFormatterResolver formatterResolver)
            {
                offset = WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
                offset = WriteInt32ForceInt32Block(ref bytes, offset, 0x10);
                offset = WriteString(ref bytes, offset, value.FunctionName);
                offset = WriteInt32ForceInt32Block(ref bytes, offset, 0x11);
                Console.WriteLine(true);
                return formatterResolver.GetFormatter<T>().Serialize(ref bytes, offset, value.Tuple, formatterResolver);
            }

            public CallRequest<T> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                throw new NotImplementedException();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var c = new MsgPackContext();
            //c.DiscoverConverters();
            //var b = MessagePackSerializer.Serialize(new CallRequest<A>("123", new A { V = 1 }), new PackerResolver());

            //Console.WriteLine(b.Aggregate(new StringBuilder(), (sb, b1) => sb.AppendFormat("{0:x2} ", b1)));
        }
    }
}
