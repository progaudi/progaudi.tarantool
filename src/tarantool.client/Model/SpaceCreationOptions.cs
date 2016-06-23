using System;
using System.Collections.Generic;

using Tarantool.Client.Model.Enums;

namespace Tarantool.Client.Model
{
    public class SpaceCreationOptions
    {
        public bool Temporary { get; set; } = false;

        public uint? Id { get; set; } = null;

        public uint FieldCount { get; set; } = 0;

        public bool IfNotExists { get; set; } = false;

        public StorageEngine StorageEngine { get; set; } = StorageEngine.Memtx;

        public string User { get; set; } = null;

        public IDictionary<string, Type> Format { get; } = new Dictionary<string, Type>();
    }
}