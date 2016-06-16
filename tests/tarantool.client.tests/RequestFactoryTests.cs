using System;
using System.Text;

using MsgPack.Light;

using NUnit.Framework;

using Shouldly;

using Tarantool.Client.IProto;
using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Services;


namespace Tarantool.Client.Tests
{
    [TestFixture]
    public class RequestFactoryTests
    {
        [Test]
        public void CreatAuthPacket()
        {
            var requestFactory = new AuthenticationRequestFactory();
            var salt = Encoding.UTF8.GetBytes("Tarantool 1.6.8 (Binary) e8a5ec82-2fd5-4798-aafa-ac41acabc727   DCHe8DF5IQKb8ZphIRjOxQlMiLjooLtazaUh+SPzXi0=");
            var packet = requestFactory.CreateAuthentication(new GreetingsResponse(salt), "test", "test");
            var msgPackContext = MsgPackContextFactory.Create();
            var serialzied = MsgPackSerializer.Serialize(packet, msgPackContext);

            var expected = new byte[]
            {
                0x82,
                0x23,
                0xa4,
                0x74,
                0x65,
                0x73,
                0x74,
                0x21,
                0x92,
                0xa9,
                0x63,
                0x68,
                0x61,
                0x70,
                0x2d,
                0x73,
                0x68,
                0x61,
                0x31,
                0xc4,
                0x14,
                0xd4,
                0xe2,
                0x10,
                0x62,
                0x8a,
                0xa8,
                0xba,
                0xb3,
                0xa5,
                0x25,
                0xb0,
                0x02,
                0x72,
                0xd9,
                0x13,
                0xd7,
                0x4f,
                0x64,
                0x9d,
                0x0c,
            };

            serialzied.ShouldBe(expected);
        }
    }
}