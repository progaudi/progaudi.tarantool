using System;
using System.Collections;
using System.Collections.Generic;

namespace iproto.Data
{
    public class SelectKey<TValue> : IEnumerable<KeyValuePair<string, TValue>>
    {
        public void Add(string componentKey, TValue componentValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}