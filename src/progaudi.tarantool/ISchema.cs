using System;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client
{
    public interface ISchema
    {
        /// <summary>
        /// Gets typed space by <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">Type of record, stored in space.</typeparam>
        /// <param name="name">Name of space</param>
        /// <param name="space">Typed wrapper for space, created on every call. It is advisable to cache it.</param>
        /// <returns><c>true</c>, if space info presents on client, <c>false</c> otherwise.</returns>
        bool TryGetSpace<T>(string name, out ISpace<T> space);

        /// <summary>
        /// Gets typed space by  <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">Type of record, stored in space.</typeparam>
        /// <param name="id">Id of space</param>
        /// <param name="space">Typed wrapper for space, created on every call. It is advisable to cache it.</param>
        /// <returns><c>true</c>, if space info presents on client, <c>false</c> otherwise.</returns>
        bool TryGetSpace<T>(uint id, out ISpace<T> space);

        /// <summary>
        /// Reload scheme.
        /// </summary>
        /// <returns></returns>
        Task Reload();

        /// <summary>
        /// When schema was reloaded last time.
        /// </summary>
        DateTimeOffset LastReloadTime { get; }
    }
}