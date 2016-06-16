using System;
using System.Threading.Tasks;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

using Tuple = Tarantool.Client.IProto.Tuple;

namespace Tarantool.Client
{
    public class Schema
    {
        private const int VSpace = 281;

        private const int VIndex = 289;

        private readonly IConnection _connection;


        public Schema(IConnection connection)
        {
            _connection = connection;
        }

        public Space CreateSpace(string spaceName, SpaceCreationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Space GetSpace(string name)
        {
            throw new NotImplementedException();
        }

        public Space GetSpace(uint id)
        {
            throw new NotImplementedException();
        }

        public Index GetIndex(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Index> GetIndex(uint id)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<int>>(VIndex, 0, UInt32.MaxValue, 0, Iterator.All, Tuple.Create(0));

            return await _connection.SendPacket<SelectPacket<IProto.Tuple<int>>, Index>(selectIndexRequest);
        }
    }
}