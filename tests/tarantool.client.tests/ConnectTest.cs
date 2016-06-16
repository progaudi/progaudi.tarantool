﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

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


        //private static void TreeIndexMethodsTest(Index index)
        //{
        //    var min2 = index.Min<Tuple<int, int, int>, Tuple<int>>(Tuple.Create(3));
        //    var min = index.Min<Tuple<int, string, double>>();

        //    var max = index.Max<Tuple<int, int, int>>();
        //    var max2 = index.Max<Tuple<int, string, double>, Tuple<int>>(Tuple.Create(4));
        //}

        //private static void IndexMethodsTest(Index index)
        //{
        //    var insertResponse = index.Insert(Tuple.Create(2, "Music"));
        //    var deleteResponse = index.Delete<Tuple<int, string, double>, Tuple<int>>(Tuple.Create(2));
        //    insertResponse = index.Insert(Tuple.Create(2, "Music"));
        //    var selectResponse = index.Select<Tuple<int, string>, Tuple<int>>(Tuple.Create(2));
        //    var replaceResponse = index.Replace(Tuple.Create(2, "Car", -245.3));
        //    var updateResponse = index.Update<Tuple<int, string, double>, Tuple<int>, int>(Tuple.Create(2),
        //        UpdateOperation<int>.CreateAddition(3, 100));
        //    var upsertResponse = index.Upsert(Tuple.Create(5),
        //        UpdateOperation<int>.CreateAssign(2, 2));
        //    upsertResponse = index.Upsert(Tuple.Create(5),
        //        UpdateOperation<int>.CreateAddition(-2, 2));
        //}

        //private static void SpaceMethodsTest(Space tester)
        //{
        //    var insertResponse = tester.Insert(Tuple.Create(2, "Music"));
        //    var deleteResponse = tester.Delete(Tuple.Create(2));
        //    insertResponse = tester.Insert(Tuple.Create(2, "Music"));
        //    var selectResponse = tester.Select<Tuple<int>, Tuple<int, string>>(Tuple.Create(2));
        //    var replaceResponse = tester.Replace(Tuple.Create(2, "Car", -24.5));
        //    var updateResponse = tester.Update(Tuple.Create(2), UpdateOperation<int>.CreateAddition(1, 2));
        //    var upsertResponse = tester.Upsert(Tuple.Create(5), UpdateOperation<int>.CreateAddition(1, 2));
        //}

        //private static void SendPacketMethodTest(Box tarantoolClient)
        //{
        //    var insertRequest = new InsertReplacePacket<Tuple<int, string>>(CommandCode.Insert, spaceId,
        //        Tuple.Create(2, "Music"));
        //    var insertResponse = tarantoolClient.SendRequest(insertRequest);

        //    var deleteRequest = new DeletePacket<Tuple<int>>(spaceId, 0, Tuple.Create(2));
        //    var deleteResponse = tarantoolClient.SendRequest(deleteRequest);

        //    insertResponse = tarantoolClient.SendRequest(insertRequest);

        //    var selectRequest = new SelectPacket<Tuple<int>>(spaceId, 0, 100, 0, Iterator.All, Tuple.Create(2));
        //    var selectResponse = tarantoolClient.SendRequest(selectRequest);

        //    var replaceRequest = new InsertReplacePacket<Tuple<int, string, int>>(CommandCode.Replace, spaceId,
        //        Tuple.Create(2, "Orange", 5));
        //    var replaceResponse = tarantoolClient.SendRequest(replaceRequest);

        //    var udateRequest = new UpdatePacket<Tuple<int>, int>(spaceId, 0, Tuple.Create(2),
        //        UpdateOperation<int>.CreateAddition(1, 2));
        //    var updateResponse = tarantoolClient.SendRequest(udateRequest);

        //    selectResponse = tarantoolClient.SendRequest(selectRequest);

        //    var upsertRequest = new UpsertPacket<Tuple<int, int>, int>(spaceId, Tuple.Create(5, 20),
        //        UpdateOperation<int>.CreateAddition(10, 1));
        //    var upsertResponse = tarantoolClient.SendRequest(upsertRequest);

        //    selectResponse = tarantoolClient.SendRequest(selectRequest);

        //    var callRequest = new CallPacket<Tuple<float>>("math.sqrt", Tuple.Create(1.3f));
        //    var callResponse = tarantoolClient.SendRequest(callRequest);

        //    var evalRequest = new EvalPacket<Tuple<int, int, int>>("return ...", Tuple.Create(1, 2, 3));
        //    var evalResponse = tarantoolClient.SendRequest(evalRequest);
        //}
    }
}
