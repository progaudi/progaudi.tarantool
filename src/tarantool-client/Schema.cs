using System;
using System.Linq;

namespace tarantool_client
{
    public class Schema
    {
        private readonly Index[] _indices;
        private readonly Space[] _spaces;

        public Schema(Index[] indices, Space[] spaces)
        {
            _indices = indices;
            _spaces = spaces;
        }
        
        public Space CreateSpace(string spaceName, SpaceCreationOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Space GetSpace(string name)
        {
            return _spaces.Single(space => space.Name == name);
        }

        public Space GetSpace(uint id)
        {
            return _spaces.Single(space => space.Id == id);
        }

        public Index GetIndex(string name)
        {
            return _indices.Single(index => index.Name == name);
        }

        public Index GetIndex(uint id)
        {
            return _indices.Single(index => index.Id == id);
        }
    }
}