using System;
using System.Collections.Generic;
using System.Linq;

namespace tarantool_client
{
    public class Schema
    {
        private readonly Index[] _indices;
        private readonly Space[] _spaces;

        public Schema(Index[] indices, Space[] spaces, Connection connection)
        {
            _indices = indices;

            foreach (var index in _indices)
            {
                index.Connection = connection;
            }
            _spaces = spaces.Select(space => CloneSpace(space, _indices.Where(i => i.SpaceId == space.Id).ToList().AsReadOnly(), connection)).ToArray();
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

        private Space CloneSpace(Space space, IReadOnlyCollection<Index> indecies, Connection connection)
        {
            var result = new Space(space.Id, space.FieldCount, space.Name, indecies, space.Engine, space.Fields, connection);

            return result;
        }

    }
}