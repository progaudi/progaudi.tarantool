using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace tarantool_client
{
    public class Space
    {
        public uint Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<Index> Indices { get; }

        public void CreateIndex()
        {
            throw new NotImplementedException();
        }

        public Tuple<T1> Insert<T1>(Tuple<T1> tuple)
        {
            throw new NotImplementedException();
        }

        public IList<object> Select<T1>(Tuple<T1> selectKey)
        {
            throw new NotImplementedException();
        }

        public IList<Tuple<TResult1>> Select<TSelect1, TResult1>(Tuple<TSelect1> selectKey)
        {
            throw new NotImplementedException();
        }
    }
}