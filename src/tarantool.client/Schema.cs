using System;
using System.Linq;
using System.Threading.Tasks;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.Utils;

using Tuple = Tarantool.Client.IProto.Tuple;

namespace Tarantool.Client
{
    public class Schema
    {
        private const int VSpace = 0x119;

        private const int SpaceById = 0;

        private const int SpaceByName = 2;

        private readonly LogicalConnection _logicalConnection;

        public Schema(LogicalConnection logicalConnection)
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

            var result = response.Data.SingleOrDefault();
            if (result == null)
            {
                throw ExceptionHelper.InvalidSpaceName(name);
            }

            result.LogicalConnection = _logicalConnection;

            return result;
        }
       
        public async Task<Space> GetSpaceAsync(uint id)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<uint>>(VSpace, SpaceById, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(id));

            var response = await _logicalConnection.SendRequest<SelectPacket<IProto.Tuple<uint>>, ResponsePacket<Space[]>>(selectIndexRequest);

            var result = response.Data.SingleOrDefault();
            if (result == null)
            {
                throw ExceptionHelper.InvalidSpaceId(id);
            }

            result.LogicalConnection = _logicalConnection;

            return result;
        }
    }
}