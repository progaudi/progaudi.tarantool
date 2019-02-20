using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client
{
    public interface ISchema
    {
        [Obsolete("Use indexer")]
        Task<ISpace> GetSpace(string name);
        [Obsolete("Use indexer")]
        Task<ISpace> GetSpace(uint id);

        ISpace this[string name] { get; }
        ISpace this[uint id] { get; }

        Task Reload();

        DateTimeOffset LastReloadTime { get; }

        IReadOnlyCollection<ISpace> Spaces { get; }
    }
}