using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface ISpace<T>
    {
        uint Id { get; }

        int OwnerId { get; }

        string Name { get; }

        StorageEngine Engine { get; }

        uint FieldCount { get; }

        SpaceOptions Options { get; }

        SpaceField[] Fields { get; }

        /// <summary>
        /// Gets an index by name. Uses local cache.
        /// </summary>
        IIndex<T> this[string name] { get; }

        /// <summary>
        /// Gets an index by id. Uses local cache.
        /// </summary>
        IIndex<T> this[uint id] { get; }

        /// <summary>
        /// Gets an index by name, but does not throw exceptions. Uses local cache.
        /// </summary>
        bool TryGetIndex(string name, out IIndex<T> index);

        /// <summary>
        /// Gets an index by id, but does not throw exceptions. Uses local cache.
        /// </summary>
        bool TryGetIndex(uint id, out IIndex<T> index);

        /// <summary>
        /// Gets an index by name and change type of tuple, but does not throw exceptions. Uses local cache.
        /// </summary>
        bool TryGetIndex<TCastedValue>(string name, out IIndex<TCastedValue> index);

        /// <summary>
        /// Gets an index by id and change type of tuple, but does not throw exceptions. Uses local cache.
        /// </summary>
        bool TryGetIndex<TCastedValue>(uint id, out IIndex<TCastedValue> index);

        /// <summary>
        /// Inserts a tuple into space. If a space contains sequense, then corresponding element should be nil.
        /// </summary>
        /// <returns>Inserted tuple</returns>
        /// <exception cref="DuplicateKeyException">If primary key is duplicated</exception>
        Task<T> Insert(T tuple);

        /// <summary>
        /// Inserts a tuple into space. If a space contains sequense, then corresponding element should be nil.
        /// </summary>
        /// <returns>Inserted tuple</returns>
        /// <exception cref="DuplicateKeyException">If primary key is duplicated</exception>
        Task<T> Insert<TInsertable>(in TInsertable tuple);

        /// <summary>
        /// Select tuples using primary index of the space.
        /// </summary>
        Task<T[]> Select<TKey>(TKey selectKey, Iterator iterator = Iterator.Eq, uint limit = uint.MaxValue, uint offset = 0u);

        /// <summary>
        /// Gets a single element from space by key.
        /// </summary>
        /// <remarks>
        /// When <paramref name="options"/> is <see cref="GetOptions.Eval"/>, then we will send eval request to
        /// Tarantool with code <code>return box.space.space-name:get(key)</code>, otherwise we will send select request
        /// with limit = 2 and validate response on client side.
        ///
        /// You should measure which one is beneficial for you.
        /// </remarks>
        /// <param name="key">Key for element to get.</param>
        /// <param name="options">How we should get it: via lua code or via select with limit 2.</param>
        /// <exception cref="MoreThanOneTupleMatchesException">When there are two or more tuples with given key.</exception>
        Task<T> Get<TKey>(TKey key, GetOptions options = GetOptions.Eval);

        /// <summary>
        /// Replaces the tuple in space.
        /// </summary>
        /// <returns>Replaced tuple</returns>
        Task<T> Replace(T tuple);

        /// <summary>
        /// Same as <see cref="Replace"/>.
        /// </summary>
        Task<T> Put(T tuple);

        /// <summary>
        /// Updates a tuple in space. It is always safe to merge several update operation in one, Tarantool will preserve order.
        /// </summary>
        /// <returns>Updated tuple</returns>
        Task<T> Update<TKey>(TKey key, UpdateOperation[] updateOperations);

        /// <summary>
        /// Updates or inserts tuple.
        /// </summary>
        /// <remarks>
        /// Sends an upsert request to Tarantool. This is what happens on Tarantool's side:
        /// 
        /// If there is an existing tuple which matches the key fields of tuple_value, then the request has the same effect as
        /// <see cref="Update{TKey}"/> and the <paramref name="updateOperations"/> are used. If there is no existing tuple which
        /// matches the key fields of <paramref name="tuple"/>, then the request has the same effect as <see cref="Insert"/> and
        /// the <paramref name="tuple"/> is used. However, unlike insert or update, upsert will not read a tuple and perform error
        /// checks before returning – this is a design feature which enhances throughput but requires more caution on the part of the user.
        ///
        /// It is illegal to modify a primary-key field.
        /// It is illegal to use upsert with a space that has a unique secondary index.
        /// </remarks>
        Task Upsert(T tuple, UpdateOperation[] updateOperations);

        /// <summary>
        /// Updates or inserts tuple.
        /// </summary>
        /// <remarks>
        /// Sends an upsert request to Tarantool. This is what happens on Tarantool's side:
        /// 
        /// If there is an existing tuple which matches the key fields of tuple_value, then the request has the same effect as
        /// <see cref="Update{TKey}"/> and the <paramref name="updateOperations"/> are used. If there is no existing tuple which
        /// matches the key fields of <paramref name="tuple"/>, then the request has the same effect as <see cref="Insert"/> and
        /// the <paramref name="tuple"/> is used. However, unlike insert or update, upsert will not read a tuple and perform error
        /// checks before returning – this is a design feature which enhances throughput but requires more caution on the part of the user.
        ///
        /// It is illegal to modify a primary-key field.
        /// It is illegal to use upsert with a space that has a unique secondary index.
        /// </remarks>
        Task Upsert<TInsertable>(TInsertable tuple, UpdateOperation[] updateOperations);

        /// <summary>
        /// Deletes tuple from space.
        /// </summary>
        /// <remarks>
        /// Vynil storage engine will return <c>null</c>, because of how LSM-trees are working.
        /// </remarks>
        /// <returns>Deleted tuple.</returns>
        Task<T> Delete<TKey>(TKey key);

        /// <summary>
        /// Return count of tuples.
        /// </summary>
        /// <remarks>
        /// If compared with <see cref="Length"/>, this methos works slower, because it scans space, according to <paramref name="key"/>
        /// and <paramref name="iterator"/>
        /// </remarks>
        Task<int> Count<TKey>(TKey key, Iterator iterator);

        /// <summary>
        /// Return count of tuples.
        /// </summary>
        /// <remarks>
        /// If compared with <see cref="Length"/>, this methos works slower, because it scans the entire space.
        /// </remarks>
        Task<int> Count();

        /// <summary>
        /// Return the number of tuples in the space.
        /// </summary>
        /// <remarks>
        /// If compared with <see cref="Count"/>, this method works faster because it does not scan the entire space to count
        /// the tuples, but vinyl does not support it.
        /// </remarks>
        Task<int> Length();

        /// <summary>
        /// Number of bytes in the space.
        /// </summary>
        /// <remarks>
        /// This number, which is stored in Tarantool’s internal memory, represents the total number of bytes in all tuples, not
        /// including index keys. For a measure of index size, you'll need to iterate on all indexes
        /// </remarks>
        Task<int> ByteSize();
    }
}
