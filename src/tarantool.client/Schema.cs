using System;
using System.Linq;
using System.Threading.Tasks;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

using Tuple = Tarantool.Client.IProto.Tuple;

namespace Tarantool.Client
{
    public class Schema
    {
        private const int VSpace = 0x119;

        private const int VIndex = 0x121;

        private readonly ILogicalConnection _logicalConnection;

        public Schema(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        public async Task<Space> CreateSpaceAsync(string spaceName, SpaceCreationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Space> GetSpaceAsync(string name)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<string>>(VSpace, 0, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(name));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<string>>, ResponsePacket<Space[]>>(selectIndexRequest);
            return response.Data.Single();
        }

        public async Task<Space> GetSpaceAsync(uint id)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<uint>>(VSpace, 0, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(id));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<uint>>, ResponsePacket<Space[]>>(selectIndexRequest);
            return response.Data.Single();
        }

        public async Task<Index> GetIndexAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Index> GetIndexAsync(uint id)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<int>>(VIndex, 0, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(0));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<int>>, ResponsePacket<Index>>(selectIndexRequest);
            return response.Data;
        }
    }
}