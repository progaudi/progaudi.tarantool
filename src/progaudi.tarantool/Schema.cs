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

        private Dictionary<string, int> _spacesByName = new Dictionary<string, int>();
        private Dictionary<uint, int> _spacesById = new Dictionary<uint, int>();
        private SpaceMeta[] _spaces;

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
            if (!_spacesByName.TryGetValue(name, out var index))
            {
                space = default;
                return false;
            }

            return GetSpaceByIndex(index, out space);
        }

        public bool TryGetSpace<T>(uint id, out ISpace<T> space)
        {
            if (!_spacesById.TryGetValue(id, out var index))
            {
                space = default;
                return false;
            }

            return GetSpaceByIndex(index, out space);
        }

        public async Task Reload()
        {
            var byName = new Dictionary<string, int>();
            var byId = new Dictionary<uint, int>();

            var spaces = await Select<SpaceMeta>(VSpace).ConfigureAwait(false);
            for (var i = 0; i < spaces.Length; i++)
            {
                var space = spaces[i];
                byName[space.Name] = i;
                byId[space.Id] = i;
            }

            var indices = await Select<IndexMeta>(VIndex).ConfigureAwait(false);
            var indicesBySpace = indices
                .Select((x, i) => new {x, i})
                .GroupBy(x => x.x.SpaceId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.i).ToArray());

            lock (_lockObject)
            {
                _spaces = spaces;
                _spacesByName = byName;
                _spacesById = byId;

                _indices = indices;
                _indicesBySpace = indicesBySpace;

                LastReloadTime = DateTimeOffset.UtcNow;
            }
        }

        private bool GetSpaceByIndex<T>(int index, out ISpace<T> space)
        {
            var meta = _spaces[index];
            space = new Space<T>(this, meta, _indicesBySpace[meta.Id].Select(x => _indices[x]));
            return true;
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