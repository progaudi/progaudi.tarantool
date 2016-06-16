using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using NSubstitute;

using NUnit.Framework;

using Shouldly;

namespace Tarantool.Client.Tests
{
    [TestFixture]
    public class ConnectTest
    {
        [Test]
        public async Task ConnectWithEmptyCredentials()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriter(),
                StreamBufferSize = 1
            };

            var tarantoolClient = new Box(options);


            await tarantoolClient.ConnectAsync();
        }

        [Test]
        public async Task ConnectWithCredentials()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriter(),
                UserName = "operator",
                Password = "operator"

            };

            var tarantoolClient = new Box(options);

            await tarantoolClient.ConnectAsync();
        }

        //[Test]
        //public async Task CheckAuthenticationRequest()
        //{
        //    var greetings = "Tarantool 1.6.8 (Binary) e8a5ec82-2fd5-4798-aafa-ac41acabc727  \nsD24oB/4KdxTkEdn2es+iMnZOJQZ+8QjW9EXMrGaGWg=                    ";
        //    var greetingsBytes = Encoding.UTF8.GetBytes(greetings);

        //    var physicalConnection = Substitute.For<IPhysicalConnection>();
        //    var readsCount = 0;


        //    physicalConnection.Read(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()).Returns(
        //        x =>
        //        {
        //            if (readsCount++ == 0)
        //            {
        //                Array.Copy(greetingsBytes, (byte[])x[0], greetingsBytes.Length);
        //                return greetingsBytes.Length;
        //            }
        //            return 0;
        //        });

        //    var writeArguments = new List<byte[]>();

        //    physicalConnection.When(x => x.Write(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()))
        //        .Do(
        //            x =>
        //            {
        //                writeArguments.Add(x[0] as byte[]);
        //            });

        //    var requestQueue = Substitute.For<IRequestQueue>();
        //    requestQueue
        //        .Queue(Arg.Any<ulong>())
        //        .Returns(Task.FromResult(
        //            new byte[]
        //            {
        //                0x81,
        //                0x30,
        //                0xc0
        //            }));

        //    var responseReader = Substitute.For<IResponseReader>();
        //    var responseReaderFactory = Substitute.For<IResponseReaderFactory>();
        //    responseReaderFactory
        //        .Create(Arg.Any<ILogicalConnection>(), Arg.Any<ConnectionOptions>())
        //        .Returns(responseReader);

        //    var options = new ConnectionOptions()
        //    {
        //        EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
        //        LogWriter = new StringWriter(),
        //        UserName = "operator",
        //        Password = "operator",
        //        PhysicalConnection = physicalConnection,
        //        ResponseReaderFactory = responseReaderFactory,
        //        RequestQueue = requestQueue
        //    };

        //    var tarantoolClient = new Box(options);

        //    await tarantoolClient.ConnectAsync();

        //    var expectedHeaderBytes = new byte[]
        //    {
        //        0x3e,
        //        0x82,
        //        0x00,
        //        0x07,
        //        0x01,
        //        0x01
        //    };


        //    var expectedBodyBytes = new byte[]
        //  {
        //        0x82,
        //        0x23,
        //        0xa8,
        //        0x6f,
        //        0x70,
        //        0x65,
        //        0x72,
        //        0x61,
        //        0x74,
        //        0x6f,
        //        0x72,
        //        0x21,
        //        0x92,
        //        0xa9,
        //        0x63,
        //        0x68,
        //        0x61,
        //        0x70,
        //        0x2d,
        //        0x73,
        //        0x68,
        //        0x61,
        //        0x31,
        //        0xc4,
        //        0x14,
        //        0x3f,
        //        0xb4,
        //        0xbb,
        //        0x12,
        //        0x58,
        //        0x32,
        //        0xba,
        //        0x34,
        //        0xca,
        //        0xe1,
        //        0x72,
        //        0xd7,
        //        0xff,
        //        0xd5,
        //        0x5c,
        //        0xeb,
        //        0x3e,
        //        0x1c,
        //        0x2d,
        //        0xf7,
        //  };


        //    physicalConnection.Received().Write(Arg.Any<byte[]>(), 0, expectedHeaderBytes.Length);
        //    physicalConnection.Received().Write(Arg.Any<byte[]>(), 0, expectedBodyBytes.Length);

        //    writeArguments[0].Take(6).ToArray().ShouldBe(expectedHeaderBytes);
        //    writeArguments[1].ShouldBe(expectedBodyBytes);
        //}

    }
}
