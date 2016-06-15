using System;
using System.Collections.Generic;
using System.Linq;

namespace tarantool_client
{
    public class Schema
    {
        private readonly Index[] _indices;
        private readonly Space[] _spaces;

        public Schema(Index[] indices, Space[] spaces, Multiplexer multiplexer)
        {
            _indices = indices;

            foreach (var index in _indices)
            {
                index.Multiplexer = multiplexer;
            }
            _spaces = spaces.Select(space => CloneSpace(space, _indices.Where(i => i.SpaceId == space.Id).ToList().AsReadOnly(), multiplexer)).ToArray();
        }

        public Space CreateSpace(string spaceName, SpaceCreationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Space GetSpace(string name)
        {
            return _spaces.Single(s => s.Name == name);
        }

        public Space GetSpace(uint id)
        {
            return _spaces.Single(s => s.Id == id);
        }

        public Index GetIndex(string name)
        {
            return _indices.Single(index => index.Name == name);
        }

        public Index GetIndex(uint id)
        {
            return _indices.Single(index => index.Id == id);
        }

        private Space CloneSpace(Space space, IReadOnlyCollection<Index> indecies, Multiplexer multiplexer)
        {
            var result = new Space(space.Id, space.FieldCount, space.Name, indecies, space.Engine, space.Fields, multiplexer);

            return result;
        }

    }
}