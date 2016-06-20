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

        private const int SpaceById = 0;

        private const int SpaceByName = 2;

        private const int IndexById = 0;

        private const int IndexByName = 2;

        private readonly ILogicalConnection _logicalConnection;

        public Schema(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        public Task<Space> CreateSpaceAsync(string spaceName, SpaceCreationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Space> GetSpaceAsync(string name)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<string>>(VSpace, SpaceByName, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(name));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<string>>, ResponsePacket<Space[]>>(selectIndexRequest);

            if(!response.Data.Any())
                throw new ArgumentException($"Space with name '{name}' was found!");

            return response.Data.Single();
        }

        public async Task<Space> GetSpaceAsync(uint id)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<uint>>(VSpace, SpaceById, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(id));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<uint>>, ResponsePacket<Space[]>>(selectIndexRequest);

            if (!response.Data.Any())
                throw new ArgumentException($"Space with id '{id}' was not found!");

            return response.Data.Single();
        }

        public Task<Index> GetIndexAsync(string name)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<string>>(VIndex, IndexByName, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(name));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<string>>, ResponsePacket<Index[]>>(selectIndexRequest);

            if (!response.Data.Any())
                throw new ArgumentException($"Index with name '{name}' was not found!");

            return response.Data.Single();
        }

        public async Task<Index> GetIndexAsync(uint id)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<uint>>(VIndex, IndexById, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(id));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<uint>>, ResponsePacket<Index[]>>(selectIndexRequest);

            if (!response.Data.Any())
                throw new ArgumentException($"Index with id '{id}' was found!");

            return response.Data.Single();
        }
    }
}