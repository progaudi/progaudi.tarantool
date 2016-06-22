using System;
using System.Linq;
using System.Threading.Tasks;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Utils;

using Tuple = Tarantool.Client.Model.Tuple;

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
            var selectIndexRequest = new SelectRequest<Model.Tuple<string>>(VSpace, SpaceByName, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(name));

            var response = await _logicalConnection.SendRequest<SelectRequest<Model.Tuple<string>>, DataResponse<Space[]>>(selectIndexRequest);

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
            var selectIndexRequest = new SelectRequest<Model.Tuple<uint>>(VSpace, SpaceById, uint.MaxValue, 0, Iterator.Eq, Tuple.Create(id));

            var response = await _logicalConnection.SendRequest<SelectRequest<Model.Tuple<uint>>, DataResponse<Space[]>>(selectIndexRequest);

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