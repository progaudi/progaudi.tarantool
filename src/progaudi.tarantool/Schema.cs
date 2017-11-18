using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class Schema : ISchema
    {
        private const int VSpace = 0x119;

        private const int VIndex = 0x121;

        internal const uint PrimaryIndexId = 0;

        private readonly ILogicalConnection _logicalConnection;

        private Dictionary<string, ISpace> _indexByName = new Dictionary<string, ISpace>();

        private Dictionary<uint, ISpace> _indexById = new Dictionary<uint, ISpace>();

        public Schema(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        public Task<ISpace> GetSpace(string name) => Task.FromResult(this[name]);

        public Task<ISpace> GetSpace(uint id) => Task.FromResult(this[id]);

        public ISpace this[string name] => _indexByName.TryGetValue(name, out var space) ? space : throw ExceptionHelper.InvalidSpaceName(name);

        public ISpace this[uint id] => _indexById.TryGetValue(id, out var space) ? space : throw ExceptionHelper.InvalidSpaceId(id);

        public DateTimeOffset LastReloadTime { get; private set; }

        public async Task Reload()
        {
            var byName = new Dictionary<string, ISpace>();
            var byId = new Dictionary<uint, ISpace>();

            var spaces = await Select<Space>(VSpace).ConfigureAwait(false);
            foreach (var space in spaces)
            {
                byName[space.Name] = space;
                byId[space.Id] = space;
                space.LogicalConnection = _logicalConnection;
                space.SetIndices(await Select<Index>(VIndex, Iterator.Eq, space.Id).ConfigureAwait(false));
            }

            Interlocked.Exchange(ref _indexByName, byName);
            Interlocked.Exchange(ref _indexById, byId);
            LastReloadTime = DateTimeOffset.UtcNow;
        }

        private async Task<T[]> Select<T>(uint spaceId, Iterator iterator = Iterator.All, uint id = 0u)
        {
            var request = new SelectRequest<ValueTuple<uint>>(spaceId, PrimaryIndexId, uint.MaxValue, 0, iterator, ValueTuple.Create(id));

            var response = await _logicalConnection
                .SendRequest<SelectRequest<ValueTuple<uint>>, T>(request)
                .ConfigureAwait(false);
            return response.Data;
        }
    }
}