using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client
{
    public class Schema : ISchema
    {
        private const int VSpace = 0x119;

        private const int VIndex = 0x121;

        internal const uint PrimaryIndexId = 0;

        private readonly ILogicalConnection _logicalConnection;
        private NameIdLazyWrapper<SpaceMeta> _spaces;

        private Dictionary<uint, int[]> _indicesBySpace = new Dictionary<uint, int[]>();
        private IndexMeta[] _indices;

        private readonly object _lockObject = new object();

        public Schema(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        public DateTimeOffset LastReloadTime { get; private set; }

        public bool TryGetSpace<T>(string name, out ISpace<T> space)
        {
            SpaceMeta meta;
            lock (_lockObject)
            {
                meta = _spaces[name];
            }

            space = new Space<T>(this, meta, _indicesBySpace[meta.Id].Select(x => _indices[x]));
            return true;
        }

        public bool TryGetSpace<T>(uint id, out ISpace<T> space)
        {
            SpaceMeta meta;
            lock (_lockObject)
            {
                meta = _spaces[id];
            }

            space = new Space<T>(this, meta, _indicesBySpace[meta.Id].Select(x => _indices[x]));
            return true;
        }

        public async Task Reload()
        {
            var spaces = await Select<SpaceMeta>(VSpace).ConfigureAwait(false);
            var indices = await Select<IndexMeta>(VIndex).ConfigureAwait(false);
            var indicesBySpace = indices
                .Select((x, i) => new {x, i})
                .GroupBy(x => x.x.SpaceId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.i).ToArray());

            lock (_lockObject)
            {
                _spaces = new NameIdLazyWrapper<SpaceMeta>(spaces, x => x.Id, x => x.Name);

                _indices = indices;
                _indicesBySpace = indicesBySpace;

                LastReloadTime = DateTimeOffset.UtcNow;
            }
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