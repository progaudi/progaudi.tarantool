using System;
using System.Linq;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class Schema : ISchema
    {
        private const int VSpace = 0x119;

        private const int SpaceById = 0;

        private const int SpaceByName = 2;

        private readonly ILogicalConnection _logicalConnection;

        public Schema(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        public Task<ISpace> CreateSpaceAsync(string spaceName, SpaceCreationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ISpace> GetSpace(string name)
        {
            var selectIndexRequest = new SelectRequest<TarantoolTuple<string>>(VSpace, SpaceByName, uint.MaxValue, 0, Iterator.Eq, TarantoolTuple.Create(name));

            var response = await _logicalConnection.SendRequest<SelectRequest<TarantoolTuple<string>>, Space>(selectIndexRequest);

            var result = response.Data.SingleOrDefault();
            if (result == null)
            {
                throw ExceptionHelper.InvalidSpaceName(name);
            }

            result.LogicalConnection = _logicalConnection;

            return result;
        }

        public async Task<ISpace> GetSpace(uint id)
        {
            var selectIndexRequest = new SelectRequest<TarantoolTuple<uint>>(VSpace, SpaceById, uint.MaxValue, 0, Iterator.Eq, TarantoolTuple.Create(id));

            var response = await _logicalConnection.SendRequest<SelectRequest<TarantoolTuple<uint>>, Space>(selectIndexRequest);

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