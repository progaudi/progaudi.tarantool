using System.Collections.Generic;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface IIndex<T>
    {
        uint Id { get; }

        uint SpaceId { get; }

        string Name { get; }

        bool Unique { get; }

        IndexType Type { get; }

        IReadOnlyList<IndexPart> Parts { get; }

        /// <summary>
        /// Select tuples using primary index of the space.
        /// </summary>
        Task<T[]> Select<TKey>(TKey key, Iterator iterator = Iterator.Eq, uint limit = uint.MaxValue, uint offset = 0);

        /// <summary>
        /// Inserts a tuple into space. If a space contains sequense, then corresponding element should be nil.
        /// </summary>
        /// <remarks>Index ought to be unique.</remarks>
        /// <returns>Inserted tuple</returns>
        /// <exception cref="DuplicateKeyException">If primary key is duplicated</exception>
        Task<T> Insert(ref T tuple);

        /// <summary>
        /// Inserts a tuple into space. If a space contains sequense, then corresponding element should be nil.
        /// </summary>
        /// <remarks>Index ought to be unique.</remarks>
        /// <returns>Inserted tuple</returns>
        /// <exception cref="DuplicateKeyException">If primary key is duplicated</exception>
        Task<T> Insert<TInsertable>(ref TInsertable tuple);

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
        /// <remarks>Index ought to be unique.</remarks>
        /// <returns>Replaced tuple</returns>
        Task<T> Replace(T tuple);

        /// <summary>
        /// Same as <see cref="Replace"/>.
        /// </summary>
        Task<T> Put(T tuple);

        /// <summary>
        /// Updates a tuple in space. It is always safe to merge several update operation in one, Tarantool will preserve order.
        /// </summary>
        /// <remarks>Index ought to be unique.</remarks>
        /// <returns>Updated tuple</returns>
        Task<T> Update<TKey>(TKey key, UpdateOperation[] updateOperations);

        /// <summary>
        /// Updates or inserts tuple.
        /// </summary>
        /// <remarks>
        /// Index ought to be unique.
        /// 
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
        /// Index ought to be unique.
        /// 
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
        /// Index ought to be unique.
        /// 
        /// Vynil storage engine will return <c>null</c>, because of how LSM-trees are working.
        /// </remarks>
        /// <returns>Deleted tuple.</returns>
        Task<T> Delete<TKey>(TKey key);

        /// <summary>
        /// Find the minimum value in the specified index.
        /// </summary>
        /// <remarks>
        /// Index should be of <see cref="IndexType.Tree"/> type.
        /// </remarks>
        Task<T> Min();

        /// <summary>
        /// Find the first value in the specified index that greater or equal to <paramref name="key"/>
        /// </summary>
        /// <remarks>
        /// Index should be of <see cref="IndexType.Tree"/> type.
        /// Starting with Tarantool version 2.0, will return nothing if key value is not equal to a value in the index.
        /// </remarks>
        Task<T> Min<TKey>(TKey key);

        /// <summary>
        /// Find the maximum value in the specified index.
        /// </summary>
        /// <remarks>
        /// Index should be of <see cref="IndexType.Tree"/> type.
        /// </remarks>
        Task<T> Max();

        /// <summary>
        /// Find the first value in the specified index that less or equal to <paramref name="key"/>
        /// </summary>
        /// <remarks>
        /// Index should be of <see cref="IndexType.Tree"/> type.
        /// Starting with Tarantool version 2.0, will return nothing if key value is not equal to a value in the index.
        /// </remarks>
        Task<T> Max<TKey>(TKey key);

        /// <summary>
        /// Find a random value in the specified index.
        /// </summary>
        /// <remarks>
        /// This method is useful when it’s important to get insight into data distribution in an index without having to iterate over the entire data set.
        /// Vinyl engine does not support this method.
        /// </remarks>
        Task<T> Random(uint seed);

        /// <summary>
        /// Scans index and returns count of tuples.
        /// </summary>
        Task<int> Count<TKey>(TKey key, Iterator iterator);

        /// <summary>
        /// Scans index and returns count of tuples.
        /// </summary>
        Task<int> Count();

        /// <summary>
        /// Returns the total number of bytes taken by the index.
        /// </summary>
        Task<int> ByteSize();
    }
}
